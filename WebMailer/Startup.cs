using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMailer.Models;

[assembly: OwinStartupAttribute(typeof(WebMailer.Startup))]
namespace WebMailer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitializeUserRole();
        }

        [HandleError(View = "Error")]
        private void InitializeUserRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (context.Users.FirstOrDefault(x => x.UserName == "Administrador") == null && !role.RoleExists("Administrador"))
            {
                var AdminUser = new ApplicationUser()
                {
                    UserName = "Administrador",
                    Email = "administrador@dominio.com",
                    EmailConfirmed = true
                };
                var result = user.Create(AdminUser, "P@ssw0rd");

                var AdminRole = new ApplicationRole()
                {
                    Name = "Administrador"
                };

                var result2 = role.Create(AdminRole);

                if (result.Succeeded && result2.Succeeded)
                {
                    var result3 = user.AddToRole(AdminUser.Id, "Administrador");
                }
            }
        }
    }
}