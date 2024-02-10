using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace URCP.Web.Models
{
    public class FinancialSharesDetailModel
    {
        public int Id { get; set; }
        public int MortgageId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "CommercialRegisterNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string CrNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "NumberOfShares", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public decimal NumberOfShares { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ValueOfShares", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public decimal ValueOfShares { get; set; }

        [Display(Name = "SharesType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int SharesTypeId { get; set; }

        [Display(Name = "SharesType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string SharesTypeName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "AttachedLatter", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public Guid FileReference { get; set; }

    }
}