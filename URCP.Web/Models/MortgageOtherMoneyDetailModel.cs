using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class MortgageOtherMoneyDetailModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "TypeOfTransferredMoney", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string TypeOfTransferredMoney { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MortgageAmount", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public decimal Amount { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "OtherMoneyCategory", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Category { get; set; }

    }
}