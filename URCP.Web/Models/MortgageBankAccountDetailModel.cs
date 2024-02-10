using System;
using System.ComponentModel.DataAnnotations;

namespace URCP.Web.Models
{
    public class MortgageBankAccountDetailModel
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

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "IBAN", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [StringLength(250, MinimumLength = 10, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string IBAN { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "BankAccountTypeId", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int BankAccountTypeId { get; set; }

        [Display(Name = "BankAccountTypeId", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string BankAccountTypeName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "AmountOfMortgage", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public decimal MortgagedAmount { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "Currency", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int CurrencyId { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string CurrencyName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ApprovalFromBank", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Guid FileReference { get; set; }

    }
}