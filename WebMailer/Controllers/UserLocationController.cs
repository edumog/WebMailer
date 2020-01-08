using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMailer.Models;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class UserLocationController : Controller
    {
        private IWebMailerContext context = new WebMailerContext();

        [Authorize(Roles = "Administrador")]
        public ActionResult Assignment(int id = 0)
        {
            if(id == 0) return View("Error");
            Session["Usr"] = id;
            UserInformation user = context.FindUserInformation(id);
            ViewBag.userName = user.FirstName + " " + user.LastName;
            IList<Location> userLocations = (from UserLocation usrLoc in context.UsersLocations
                                             join UserInformation usr in context.UsersInformation
                                             on usrLoc.User.UserId equals usr.UserId
                                             join Location loc in context.Locations
                                             on usrLoc.Location.LocationID equals loc.LocationID
                                             where usrLoc.User.UserId == id
                                             select loc).ToList();
            return View("assignment", userLocations);
        }

        public ActionResult _UserLocations()
        {
            int id = (int)Session["Usr"];
            IList<Location> userLocations = (from UserLocation usrLoc in context.UsersLocations
                                             join UserInformation usr in context.UsersInformation
                                             on usrLoc.User.UserId equals usr.UserId
                                             join Location loc in context.Locations
                                             on usrLoc.Location.LocationID equals loc.LocationID
                                             where usrLoc.User.UserId == id
                                             select loc).ToList();

            IList<Location> notAssi = (from Location loc in context.Locations
                                       select loc
                                    ).ToList();

            for (int i = 0; i < notAssi.Count(); i++)
            {
                for (int j = 0; j < userLocations.Count(); j++)
                {
                    if (notAssi.ElementAt(i).LocationID == userLocations.ElementAt(j).LocationID)
                    {
                        notAssi.RemoveAt(i);
                    }
                }
            }
            return PartialView("_UserLocations", notAssi);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RevokeLocation(int loc)
        {
            int idUsr = (int)Session["usr"];
            UserLocation location = context.FindUserLocation(idUsr, loc);
            if(location != null)
            {
                context.Remove(location);
                context.SaveChanges();
            }
            return RedirectToAction("Assignment", "UserLocation", new { id = idUsr });
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult AssignLocation(int loc)
        {
            int idUsr = (int)Session["usr"];
            if (context.FindUserLocation(idUsr, loc) == null)
            {
                UserLocation location = new UserLocation
                {
                    UserId = idUsr,
                    LocationId = loc
                };
                context.Add(location);
                context.SaveChanges();
            }
            return RedirectToAction("Assignment", "UserLocation", new { id = idUsr });
        }
    }
}