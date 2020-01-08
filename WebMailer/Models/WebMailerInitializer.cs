using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebMailer.Models
{
    public class WebMailerInitializer : CreateDatabaseIfNotExists<WebMailerContext>
    {
        protected override void Seed(WebMailerContext context)
        {
            base.Seed(context);

            UserInformation userInfo = new UserInformation
            {
                UserName = "Administrador",
                Email = "email@dominio.com",
                FirstName = "Nombre",
                LastName = "Apellido",
                RegisterDate = DateTime.Today
            };
            context.UsersInformation.Add(userInfo);
            context.SaveChanges();
        }
    }
}