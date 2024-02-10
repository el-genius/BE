using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace URCP.Web.Models
{
    public class BankDepositDetailModel
    {
        public int Id { get; set; }
        public int MortgageId { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string AccountNumber { get; set; }

        [Display(Name = "BankName", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public int BankId { get; set; }

        [Display(Name = "BankName", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string BankName { get; set; }

        [Display(Name = "DepositNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string DepositNumber { get; set; }

        [Display(Name = "MortgagedAmount", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public decimal MortgagedAmount { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public int CurrencyId { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string CurrencyName { get; set; }

        [Display(Name = "DepositType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public int DepositTypeId { get; set; }

        [Display(Name = "DepositType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string DepositTypeName { get; set; }

        [Display(Name = "BankConfirmation", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Guid FileReference { get; set; }

    }
}