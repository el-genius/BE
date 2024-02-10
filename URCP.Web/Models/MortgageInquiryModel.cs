using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace URCP.Web.Models
{
    public class MortgageInquiryModel
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "TypesOfMortgageInquery", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int InquiryType { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "IdentityType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int? Identitytype { get; set; }

        #region Select List
        public SelectList InquiryTypes { get; set; }
        public SelectList Identitytypes { get; set; }
        #endregion

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ItemReferenceNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageReferenceNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MortgageNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "IdentityNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string IdentityNumber { get; set; }
        public Sadad.SadadModel Sadad { get; set; } = new Sadad.SadadModel();
    }
}