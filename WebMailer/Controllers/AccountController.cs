using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMailer.Models;

namespace WebMailer.Controllers
{
    [HandleError(View = "Error")]
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private IWebMailerContext context = new WebMailerContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) { return RedirectToLocal(returnUrl); }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                try
                {
                    var user = await UserManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        ModelState.AddModelError("", $"El usuario llamado {model.UserName} no esta registrado");
                        return View(model);
                    }
                    if (!ModelState.IsValid || !user.EmailConfirmed)
                    {
                        return View(model);
                    }
                    // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
                    // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true

                    var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal(returnUrl);
                        //case SignInStatus.LockedOut:
                        //    return View("Lockout");
                        case SignInStatus.Failure:
                            ModelState.AddModelError("", "Intento de inicio de sesión no válido verifique la contraseña o nombre de usuario");
                            return View(model);
                        default:
                            return View(model);
                    }
                }
                catch
                {
                    return View("Error");
                }
            }
            else return RedirectToAction("Default", "Users");
            
        }

        [AllowAnonymous]
        [Authorize(Roles = "Administrador")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                string password = GeneratePassword();
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    AddUserInformation(model);
                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    //Enviar correo electrónico con este vínculo
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", $"<div style=\"font-family: Times New Roman, Georgia, Serif; \"><hgroup><h1>Confirmación de cuenta WebMailer Lourtec</h1><h2>Inicie sesión con las siguientes credenciales</h2></hgroup><table style=\"font: 18px Times New Roman; \"><tr><td>Nombre de usuario: </td><td>{model.UserName}</td></tr><tr><td>Contraseña: </td><td>{password}</td></tr></table><span style=\"font: bold 20px Times New Roman; \">Recuerde cambiar su contraseña</span> <h2>Para confirmar la cuenta, haga clic <a href=\"{callbackUrl}\">aquí</a></h2></div>");
                    ViewBag.controller = "Users";
                    ViewBag.action = "GetUsers";
                    ViewBag.confirmation = $"El ususario {model.UserName} se ha registrado exitosamente";
                    return View("Confirm");
                }
                AddErrors(result);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }
        [Authorize(Roles = "Administrador")]
        public ViewResult DeleteConfirmed(string name)
        {
            ViewBag.name = name;
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(string name)
        {
            try
            {
                ApplicationUser user = await UserManager.FindByNameAsync(name);
                if (user != null)
                {
                    if (UserManager.IsInRoleAsync(user.Id, "Administrador").Result)
                    {
                        var result = await UserManager.RemoveFromRolesAsync(user.Id, "Administrador");
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First());
                        }
                    }
                    var delete = await UserManager.DeleteAsync(user);
                    if (!delete.Succeeded)
                    {
                        throw new Exception(delete.Errors.First());
                    }
                    UserInformation userInfo = context.UsersInformation.FirstOrDefault(x => x.UserName == name);
                    RemoveUserCampaigns(userInfo);
                    RemoveUserCampaigns(userInfo);
                    context.Remove(userInfo);
                    context.SaveChanges();
                    ViewBag.controller = "Users";
                    ViewBag.action = "GetUsers";
                    ViewBag.confirmation = $"El usuario {name} ha sido borrado";
                    return View("Confirm");
                }
                else
                {
                    ViewBag.confirmation = $"El usuario {name} no existe";
                    return View("Confirm");
                }
            }
            catch
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                if (result.Succeeded)
                {
                    ViewBag.confirmation = "Su cuenta esta activada. Por favor inicie sesión con las credenciales proporcionadas y cambie la contraseña. ";
                    return View("Confirm");
                }
                else
                {
                    ViewBag.confirmation = "No fue posible activar su cuenta ";
                    return View("Confirm");
                }
            }
            catch
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ViewBag.confirmation = "El usuario no existe o no ha confirmado su cuenta";
                    return View("Confirm");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", $"<div style=\"font-family: Times New Roman, Georgia, Serif; \" >Por favor reestablezca su contraseña haciendo clic aquí: <a href=\"{callbackUrl}\">link</a></div>");

                ViewBag.confirmation = $"Se ha enviado un enlace al correo {model.Email} para restablecer su contraseña";
                return View("Confirm");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<ActionResult> ChangePassword()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            ViewBag.Email = user.Email;
            ViewBag.Code = code;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", $"El corro electrónico {model.Email} no esta registrado ");
                return View();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                ViewBag.confirmation = "Su contraseña ha sido restablecida ";
                return View("Confirm");
            }AddErrors(result);
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> AddRole(AddRoleViewModel role)
        {
            if (!UserManager.IsInRoleAsync(role.UserId, role.RoleName).Result)
            {
                var result = await UserManager.AddToRoleAsync(role.UserId, role.RoleName);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
            ViewBag.controller = "Users";
            ViewBag.action = "GetUsers";
            ViewBag.confirmation = "Rol asignado";
            return View("Confirm");
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        #region "Aux"

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddUserInformation(RegisterViewModel model)
        {
            var user = new UserInformation
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                RegisterDate = DateTime.Today
            };
            context.Add(user);
            context.SaveChanges();
        }

        private void RemoveUserLocations(UserInformation userInfo)
        {
            IList<UserLocation> locations = (from UserLocation loc in context.UsersLocations
                                             where loc.UserId == userInfo.UserId
                                             select loc).ToList();
            context.RemoveRange(locations);
        }

        private void RemoveUserCampaigns(UserInformation userInfo)
        {
            IList<Campaign> campaign = (from Campaign camp in context.Campaigns
                                        where camp.UserId == userInfo.UserId
                                        select camp).ToList();

            foreach (var item in campaign)
            {
                item.UserId = null;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Default", "Users");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            int pos = random.Next(0, 5);
            int[] obj = new int[3];
            string alph = "abcdefghijklmnñopqrstuvwxyz", upercase = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ", numbers = "0123456789", symbols = ".@*/_:;?¡¿*][{}!+-=";
            char[] pass = new char[6];
            alph.ToArray();
            upercase.ToArray();
            numbers.ToArray();
            symbols.ToArray();          
            for(int item = 0; item <= 5; item++)
            {
                pass[item] = alph[random.Next(0, 26)];
            }
            for(var item = 0; item <= 2; item++)
            {
                while (obj.Contains(pos))
                {
                    pos = random.Next(0, 5);
                }
                obj[item] = pos;
                switch (item)
                {
                    case 0:
                        pass[pos] = numbers[random.Next(0, 9)];
                        break;
                    case 1:
                        pass[pos] = symbols[random.Next(0, 17)];
                        break;
                    case 2:
                        pass[pos] = upercase[random.Next(0, 26)];
                        break;
                }
            }
            string password = new string(pass);
            return password;
        }

        public string NameAvailability(string name)
        {
            ApplicationUser user = UserManager.FindByName(name);
            if (user == null)
            {
                return "";
            }
            else return "Este nombre de usuario no está disponible.";
        }
        #endregion
    }
}