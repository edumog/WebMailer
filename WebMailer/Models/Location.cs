using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebMailer.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Nombre de la sede")]
        public string LocationName { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<UserInformation> Locations { get; set; }
    }
}