using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMailer.Models;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class LocationController : Controller
    {
        private IWebMailerContext context = new WebMailerContext();

        [Authorize(Roles = "Administrador")]
        public ActionResult Locations()
        {
            List<Location> locations = (from Location loc in context.Locations
                                        orderby loc.LocationID ascending
                                        select loc).ToList();
            return View(locations);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RegisterLocation()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult RegisterLocation(Location loc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (context.Locations.FirstOrDefault(x => x.LocationName == loc.LocationName) == null)
                    {
                        Location location = new Location
                        {
                            LocationName = loc.LocationName
                        };
                        context.Add<Location>(location);
                        context.SaveChanges();
                        ViewBag.controller = "Location";
                        ViewBag.action = "Locations";
                        ViewBag.confirmation = "Sede registrada";
                        return View("Confirm");
                    }else
                    {
                        ViewBag.controller = "Location";
                        ViewBag.action = "Locations";
                        ViewBag.confirmation = $"Ya existe una sede llamada \"{loc.LocationName}\".";
                        return View("Confirm");
                    }
                    
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Registration Error", "Registration error: " + e.StatusCode.ToString());

                }
            }
            return View("Error");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            try
            {
                if(id == 0)
                {
                    throw new Exception();
                }
                string location = context.FindLocation(id).LocationName;
                ViewBag.id = id;
                ViewBag.name = location;
                return View();
            }
            catch { return View("Error"); }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                Location location = context.FindLocation(id);
                if (location != null && id !=0)
                {
                    IList<UserLocation> usrLoc = (from UserLocation uslo in context.UsersLocations
                                                  where uslo.LocationId == id
                                                  select uslo).ToList();
                    context.RemoveRange(usrLoc);
                    IList<Campaign> locCam = (from Campaign cam in context.Campaigns
                                              where cam.LocationID == id
                                              select cam).ToList();
                    context.RemoveRange(locCam);
                    context.Remove(location);
                    context.SaveChanges();
                    ViewBag.controller = "Location";
                    ViewBag.action = "Locations";
                    ViewBag.confirmation = "Sede borrada.";
                    return View("Confirm");
                }
                ViewBag.controller = "Location";
                ViewBag.action = "Locations";
                ViewBag.confirmation = "El usuario no existe.";
                return View("Confirm");
            }
            catch { return View("Error"); }
        }

        public PartialViewResult _Locations()
        {
            int id = context.UsersInformation.FirstOrDefault(x => x.UserName == User.Identity.Name).UserId;
            IList<Location> locations = (from Location loc in context.Locations
                                        join UserLocation usrLoc in context.UsersLocations
                                        on loc.LocationID equals usrLoc.LocationId
                                        where usrLoc.UserId == id
                                        orderby loc.LocationID ascending
                                        select loc).ToList();

            if (locations.Count >= 1)
            {
                return PartialView("_Locations", locations);
            }

            Location item = new Location()
            {
                LocationID = 0,
                LocationName = "Sin sedes"
            };

            locations.Add(item);
            return PartialView("_Locations", locations);
        }
    }
}