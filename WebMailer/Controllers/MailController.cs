using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebMailer.Models;
using WebMailer.Utilities;
using WebMailer.SmtpClients;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class MailController : Controller
    {
        private IWebMailerContext context = new WebMailerContext(); 
        public ActionResult SendMail(Mail mail)
        {
            if (ModelState.IsValid)
            {
                UserInformation Us = context.UsersInformation.FirstOrDefault(x => x.UserName == User.Identity.Name);
                Campaign cam = context.Campaigns.FirstOrDefault(x => x.CampaignID == mail.IdCam);
                if(cam == null || Us == null) { return RedirectToAction("Default", "Users"); }
                int idCam = mail.IdCam, idLoc = mail.IdLoc;
                string to = mail.To.ToString(), from = Us.FirstName+" "+Us.LastName;
                string[] To = { };
                to = to.Trim();
                To = to.Split(' ');
                string pass = PasswordManager.Decrypt(Us.PasswordEmail), usr = Us.Email;
                try
                {
                    IAbstractFactorySmtp factory = new FactorySmtp(usr, pass);
                    var client = factory.CreateSmtpClient();
                    MailMessage msg = new MailMessage
                    {
                        IsBodyHtml = true,
                        From = new MailAddress(Us.Email, from),
                        Subject = cam.CampaignName,
                        Body = MailContentBuilder(cam.Content, from)
                    };
                    foreach (var item in To)
                    {
                        msg.To.Add(item);
                    }
                    
                    client.Send(msg);
                }
                catch
                {
                    ViewBag.confirmation = "No fue posible enviar los correos"; 
                    return View("Confirm");
                }
                ViewBag.confirmation = "Correos enviados";
                return View("Confirm");
            }
            ViewBag.confirmation = "No fue posible enviar los correos";
            return View("Confirm");
        }

        public string MailContentBuilder(string content, string from)
        {
            content = $"<body>{content}<footer>Atte. {from}</footer></body>";
            content = $"<!DOCTYPE html> <html> <head><meta charset=\"utf - 8\" /></head>{content}</html>";
            return content; 
        }
    }
}