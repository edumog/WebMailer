using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return RedirectToAction("Default", "Users");
        }

        public PartialViewResult _menu1()
        {
            return PartialView("_menu1");
        }

        public PartialViewResult _CampaignsMenu()
        {
            return PartialView("_CampaignsMenu");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult _adminMenu()
        {
            return PartialView("_adminMenu");
        }
    }
}