using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using WebMailer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class RoleController : Controller
    {
        private ApplicationDbContext context2 = new ApplicationDbContext();
        private ApplicationRoleManager _roleManager;

        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleViewModel model)
        {
            if (!RoleManager.RoleExists(model.Name))
            {
                var role = new ApplicationRole() { Name = model.Name };
                await RoleManager.CreateAsync(role);
            }
            return RedirectToAction("Default", "users");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult GetRoles(string userName)
        {
            if (userName == null) RedirectToAction("Default", "users");
            ApplicationUser user = context2.Users.FirstOrDefault(x => x.UserName == userName);
            if(user != null)
            {
                var roles = RoleManager.Roles.ToList();
                ViewBag.roles = roles;
                ViewBag.user = user;
                return View();
            }
            ViewBag.confirmation = "El usuario no existe";
            return View("Confirm");
        }

    }
}