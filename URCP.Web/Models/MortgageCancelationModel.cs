using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URCP.Resources;

namespace URCP.Web.Models
{
    public class MortgageCancelationModel
    {
        public int MortageId { get; set; }

        [Display(Name = "CancelationReason", ResourceType = typeof(SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(SharedStrings_Ar))]
        public string Reason { get; set; }

        [Display(Name = "CancelationDate", ResourceType = typeof(SharedStrings_Ar))]
        public DateTime? CancelationDate { get; set; }
    }
}