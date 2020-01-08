using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMailer.Models;
using WebMailer.Utilities;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class UsersController : Controller
    {
        private IWebMailerContext context = new WebMailerContext();
        private ApplicationDbContext context2 = new ApplicationDbContext();

        public ActionResult Default()
        {
            return View("Index");
        }

       public ActionResult UpdateUserInformation()
       {
            ApplicationUser user = context2.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            UserInformation usInfo = context.UsersInformation.FirstOrDefault(x => x.Email == user.Email);
            UpdateModel usUpd = new UpdateModel
            {   
                FirstName = usInfo.FirstName,
                LastName = usInfo.LastName,
                Email = user.Email,
                PasswordEmail = PasswordManager.Decrypt(usInfo.PasswordEmail),
                CorporateEmail = usInfo.CorporateEmail
            };
            return View(usUpd);
       }

        [HttpPost]
        public ActionResult UpdateUserInformation(UpdateModel usr)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = context2.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                UserInformation Update = context.UsersInformation.FirstOrDefault(x => x.Email == user.Email);
                Update.FirstName = usr.FirstName;
                Update.LastName = usr.LastName;
                Update.Email = usr.Email;
                Update.PasswordEmail = PasswordManager.Encrypt(usr.PasswordEmail);
                Update.CorporateEmail = usr.CorporateEmail;
                context.SaveChanges();
                user.Email = usr.Email;
                context2.SaveChanges();
            }
            return RedirectToAction("Default", "Users");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult GetUsers()
        {
            List<UserInformation> Users = (from p in context.UsersInformation
                                           orderby p.UserId descending
                                           select p).ToList();
            return View("GetUsers", Users);
        }
    }
}