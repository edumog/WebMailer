using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMailer.Models
{
    public class UserLocation
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int LocationId { get; set; }

        public virtual UserInformation User { get; set; }
        public virtual Location Location { get; set; }
    }
}