using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMailer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    name: "Index",
                    url: "Inicio",
                    defaults: new { controller = "Users", action = "Default" }
                );
            routes.MapRoute(
                    name: "UserInformation",
                    url: "Actualizar_información",
                    defaults: new { controller = "Users", action = "UpdateUserInformation" }
                );
            routes.MapRoute(
                name: "GetUsers",
                url: "Usuarios",
                defaults: new { controller = "Users", action = "GetUsers" }
                );
            routes.MapRoute(
                    name: "Campaigns",
                    url: "Campañas",
                    defaults: new { controller = "Campaigns", action = "GetCampaignsFromAuthenticatedUser" }
                );
            routes.MapRoute(
                    name: "CreateCampaign",
                    url: "Crear_campaña_nueva",
                    defaults: new { controller = "Campaigns", action = "CreateCampaign" }
                );
            routes.MapRoute(
                name: "EditCampaign",
                url: "Editar_Campaña {idCampaign}",
                defaults: new { controller = "Campaigns", action = "GetCampaignToEdit" },
                constraints: new { idCampaign = "[0-9]+" }
                );
            routes.MapRoute(
                name: "DeleteCampaign",
                url: "Borrar_campaña {id}",
                defaults: new {controller = "Campaigns", action = "Delete" },
                constraints: new {id = "[0-9]+"}
                );
            routes.MapRoute(
                name: "CreteUser",
                url: "Crear_usuario",
                defaults: new {controller = "Account", action = "Register" }
                );
            routes.MapRoute(
               name: "AddRol",
               url: "Rol_asignado",
               defaults: new { controller = "Account", action = "AddRole" }
               );
            routes.MapRoute(
                name: "ChangePassword",
                url: "Actualizar_contraseña",
                defaults: new { controller = "Account", action = "ChangePassword" }
                );
            routes.MapRoute(
                name: "ResetPassword",
                url: "Contraseña_reestablecida",
                defaults: new { controller = "Account", action = "ResetPassword" }
                );
            routes.MapRoute(
                name: "ForgotPassword",
                url: "Recuperar_contraseña",
                defaults: new { controller = "Account", action = "ForgotPassword" }
                );
            routes.MapRoute(
                name: "DeleteUserConfirmation",
                url: "Borrar_usuario {name}",
                defaults: new { controller = "Account", action = "DeleteConfirmed" },
                constraints: new { name = "[A-Z]*[a-z]*[0-9]*" }
                );
            routes.MapRoute(
                name: "DeleteUser",
                url: "borrarUsuario {name}",
                defaults: new { controller = "Account", action = "Delete" },
                constraints: new { name = "[A-Z]*[a-z]*[0-9]*" }
                );
            routes.MapRoute(
                name: "AddRole",
                url: "Asignar_rol {userName}",
                defaults: new { controller = "Role", action = "GetRoles" },
                constraints: new { userName = "[A-Z]*[a-z]*[0-9]*"}
                );
            routes.MapRoute(
                name: "Locations",
                url: "Sedes",
                defaults: new { controller = "Location", action = "Locations" }
                );
            routes.MapRoute(
                name: "RegisterLocation",
                url: "Registrar_nueva_sede",
                defaults: new { controller = "Location", action = "RegisterLocation" }
                );
            routes.MapRoute(
                name: "DeleteLocation",
                url: "Borrar_sede {id}",
                defaults: new { controller = "Location", action = "DeleteConfirmed" },
                constraints: new { id = "[0-9]+" }
                );
            routes.MapRoute(
                name: "DeleteLoc",
                url: "Borrar_sede",
                defaults: new { controller = "Location", action = "Locations" }
                );
            
            routes.MapRoute(
                name: "AssignLocations",
                url: "Asignar_sedes {id}",
                defaults: new { controller = "UserLocation", action = "Assignment"},
                constraints: new { id = "[0-9]+" }
                );
            routes.MapRoute(
                name: "SendMail",
                url: "Enviar_correos",
                defaults: new { controller = "Mail", action = "SendMail" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{returnUrl}",
                defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional }
            );
        }
    }
}