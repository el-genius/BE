using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URCP.Resources;

namespace URCP.Web.Models
{
    public class MortgageObjectionModel
    {
        public int MortgageId { get; set; }

        [Display(Name = "ObjectionReason", ResourceType = typeof(SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(SharedStrings_Ar))]
        public string Reason { get; set; }

        [Display(Name = "ObjectionDate", ResourceType = typeof(SharedStrings_Ar))]
        public DateTime? ObjectionDate { get; set; }
    }
}