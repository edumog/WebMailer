using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebMailer.Models;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class CampaignsController : Controller
    { 
        private IWebMailerContext applicationDbContext = new WebMailerContext();

        public ActionResult GetCampaignsFromAuthenticatedUser()
        {
            int authenticatedUserId = AuthenticatedUser().UserId;
            IEnumerable<GetCampaigns> campaigns = applicationDbContext.FindCampaignsByUserAndLocations(authenticatedUserId);
            return View(campaigns);
        }

        public UserInformation AuthenticatedUser()
        {
            return applicationDbContext.FindUserInformation(User.Identity.Name);
        }

        [AllowAnonymous]
        public JsonResult GetCampaignsInLocation(int locationId)
        {
            IEnumerable<Campaigns> campaigns = applicationDbContext.FindCampaignsByLocation(locationId);
            return Json( new { campaigns }, JsonRequestBehavior.AllowGet );
        }

        public ActionResult CreateCampaign()
        {
            return View();
        }

        [HttpPost]
        public ViewResult CreateCampaign(CreateCampaign campaign)
        {
            if (ValidateCampaign(campaign))
            {
                SaveCampaign(campaign);
                ViewBag.confirmation = "Campaña guardada";
            }else
                ViewBag.confirmation = "No se pudo crear la campaña";
            return View("Confirm");
        }

        private bool ValidateCampaign(CreateCampaign campaign)
        {
            return ModelState.IsValid && campaign.IdLoc != 0;
        }

        private void SaveCampaign(CreateCampaign campaign)
        {
            UserInformation user = AuthenticatedUser();
            Campaign newCampaign = new Campaign()
            {
                CampaignName = campaign.CampaignName,
                LocationID = campaign.IdLoc,
                UserId = user.UserId,
                Content = campaign.Content
            };
            applicationDbContext.Add(newCampaign);
            applicationDbContext.SaveChanges();
        }

        public ActionResult GetCampaignToEdit(int idCampaign)
        {
            Campaign campaign = FindCampaignById(idCampaign);
            return GetCampaignToEdit(campaign);
        }

        private Campaign FindCampaignById(int idCampaign)
        {
            return applicationDbContext.FindCampaign(idCampaign);
        }

        private ActionResult GetCampaignToEdit(Campaign campaign)
        {
            if (campaign != null)
            {
                return SendCampaignToEdit(campaign);
            }
            else
                return CampaignNotFound();
        }

        private ActionResult SendCampaignToEdit(Campaign campaign)
        {
            return View(campaign);
        }

        private ActionResult CampaignNotFound()
        {
            ViewBag.confirmation = "La campaña no existe ";
            return View("confirm");
        }

        [HttpPost]
        public ActionResult Edit (Campaign updateCampaign)
        {
            Campaign campaign = FindCampaignById(updateCampaign.CampaignID);
            SaveChanges(campaign, updateCampaign);
            ViewBag.confirmation = "Campaña actualizada.";
            return View("Confirm");
        }

        private void SaveChanges(Campaign campaign, Campaign updateCampaign)
        {
            campaign.CampaignName = updateCampaign.CampaignName;
            campaign.LocationID = updateCampaign.LocationID;
            campaign.Content = updateCampaign.Content;
            applicationDbContext.SaveChanges();
        }

        public ViewResult DeleteConfirmed(int id = 0)
        {
            if (id == 0) return View("Error");
            string cam = applicationDbContext.FindCampaign(id).CampaignName;
            ViewBag.id = id;
            ViewBag.name = cam;
            return View();
        }

        public ActionResult Delete(int id = 0)
        {
            if (id == 0) return View("Error");
            Campaign cam = applicationDbContext.FindCampaign(id);
            if (cam != null)
            {
                applicationDbContext.Remove(cam);
                applicationDbContext.SaveChanges();
                ViewBag.confirmation = $"Campaña \"{cam.CampaignName}\" borrada";
                ViewBag.action = "GetCampaigns";
                ViewBag.controller = "Campaigns";
                return View("Confirm");
            }
            ViewBag.confirmation = "La campaña no existe";
            return View("Confirm");
            
        }
    }
}