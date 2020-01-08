using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebMailer.Models
{
    public class Campaign
    {
        [Key]
        public int CampaignID { get; set; }

        public int LocationID { get; set; }
        public int? UserId { get; set; }

        [StringLength(80)]
        [Display(Name = "Nombre de la campaña")]
        [Required]
        public string CampaignName { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public virtual Location Location { get; set; }
        public virtual UserInformation User { get; set; }
    }
}