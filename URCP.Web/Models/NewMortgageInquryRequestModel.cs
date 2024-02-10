using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URCP.Resources;

namespace URCP.Web.Models
{
    public class MortgageInquryRequestIndexModel
    {
        public NewMortgageInquryRequestModel NewMortgageInquryRequest { get; set; }
        public NewMrotgagedStatusModel NewMrotgagedStatus { get; set; }
    }

    public class NewMortgageInquryRequestModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "IdNumberOrCR", ResourceType = typeof(SharedStrings_Ar))]
        public string IdNumberOrCR { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        [Display(Name = "MortgagorId", ResourceType = typeof(SharedStrings_Ar))]
        public string ReferenceNumber { get; set; }

        [Display(Name = "NoData", ResourceType = typeof(SharedStrings_Ar))]
        public bool? NotFound { get; set; }
    }

    // الراهن ** 
    public class NewMrotgagedStatusModel
    {
        [Display(Name = "ItemReferenceNumber", ResourceType = typeof(SharedStrings_Ar))]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "NoData", ResourceType = typeof(SharedStrings_Ar))]
        public bool? NotFound { get; set; }
    }
}