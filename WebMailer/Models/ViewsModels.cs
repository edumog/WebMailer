using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMailer.Models
{

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electronico")]
        public string Email { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MMyy}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDate { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@*/\"|°¬!#$%&=?¡¿'´+¨*-.,;:_~´`^^<>^~{}()])[a-zA-Z0-9@*/\"|°¬!#$%&=?¡¿'´+¨*-.,;:_~´`^^<>^~{}()]{6,10}$", ErrorMessage = "La contraseña debe tener una longitud de al menos 6 caracteres, una letra mayúscula, un número y un símbolo ")]
        [DataType(DataType.Password)]
        [Display(Name = "Contaseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class RoleViewModel
    {
        public string Name { get; set; }
    }

    public class Mail
    {
        public int IdLoc { get; set; }

        [Required]
        public int IdCam { get; set; }

        [Required]
        [MaxLength]
        public string To { get; set; }
    }

    public class Campaigns
    {
        public int CampaignID { get; set; }

        [StringLength(80)]
        [Display(Name = "Nombre de la campaña")]
        [Required]
        public string CampaignName { get; set; }
    }

    public class GetCampaigns : Campaigns
    {
        [Required]
        public string LocationName { get; set; }
    }

    public class CreateCampaign
    {
        [StringLength(80)]
        [Required]
        public string CampaignName { get; set; }

        [Required]
        public int IdLoc { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
    public class UpdateModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Contraseña Correo")]
        public string PasswordEmail { get; set; }

        [Display(Name = "Correo electrónico corporativo ")]
        public string CorporateEmail { get; set; }
    }

    public class AddRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}