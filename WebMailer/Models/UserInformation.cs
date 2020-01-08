using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMailer.Models
{
    public class UserInformation
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Correo")]

        public string PasswordEmail { get; set; }

        [Display(Name = "Correo electrónico corporativo ")]
        public string CorporateEmail { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MMyy}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDate { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<UserLocation> Locations { get; set; }
    }
}