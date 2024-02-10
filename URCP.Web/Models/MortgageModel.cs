using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URCP.Core.Entities;
using URCP.Core.Enum;

namespace URCP.Web.Models
{
    public class MortgageModel
    {
        public Sadad.SadadModel Sadad { get; set; }
        public MortgagedAndOwnerModel Mortgaged { get; set; }
        public MortgagedAndOwnerModel MortgageOwner { get; set; }
        public MortgageImplementationAgentModel MortgageImplementationAgent { get; set; }
        public MortgageAttachmentModel ContractAttachment { get; set; }
        public MortgageCancelationModel MortgageCancelation { get; set; }
        public MortgageObjectionModel MortgageObjection { get; set; }
        public FinancialSharesDetailModel FinancialSharesDetail { get; set; }
        public BankDepositDetailModel BankDepositDetail { get; set; }
        public MortgageOtherMoneyDetailModel MortgageOtherMoneyDetail { get; set; }
        public MortgageBankAccountDetailModel BankAccountDetail { get; set; }


        public List<string> NotAllowedWords { get; set; }

        public int Status { get; set; }

        [Display(Name = "MortgageNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Number { get; set; }

        [Display(Name = "القيد")]
        public string ContractReferenceNumber { get; set; }

        [Display(Name = "ملفات أخري")]
        public string OtherReferenceNumber { get; set; }

        public int RequestId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ItemReferenceNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string ItemReferenceNumber { get; set; } // sulaiman

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MortgageType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int TypeId { get; set; }

        [Display(Name = "MortgageType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageType { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MoneyDetails", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageMoneyDescription { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "EstimatedValueOfMoney", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageMoneyApproximately { get; set; }
        public string MortgageMoneyApproximatelyForDisplay { get { return decimal.Parse(MortgageMoneyApproximately).ToString("C", CultureInfo.CreateSpecificCulture("ar-SA")); ; } }


        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ValueOfDept", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Debt { get; set; }
        public string DebtForDisplay { get { return decimal.Parse(Debt).ToString("C", CultureInfo.CreateSpecificCulture("ar-SA")); } }


        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "EndOfContract", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string ContractExpireDateString { get; set; }

        public DateTime ContractExpireDate { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "StartOfContract", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string ContractCreateDateString { get; set; }

        public DateTime ContractCreateDate { get; set; }


        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ExecutionMethod", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int MortgageImplementationMethodId { get; set; }

        [Display(Name = "ExecutionMethod", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageImplementationMethod { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MoneyStatus", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int MoneyStatusId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MoneyType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int MoneyTypeId { get; set; }

        #region Select List
        public SelectList MortgagedAndOwnerCategories { get; set; }
        public SelectList MortgagedAndOwnerRoles { get; set; }
        public SelectList MortgageTypes { get; set; }
        public SelectList MortgageImplementationMethods { get; set; }
        public SelectList Nationalities { get; set; }
        public SelectList MoneyTypes { get; set; }
        public SelectList MoneyStatuses { get; set; }
        public SelectList Banks { get; set; }
        public SelectList Currencies { get; set; }
        public SelectList DepositTypes { get; set; }
        public SelectList FinancialSharesTypes { get; set; }
        
        #endregion

        [Display(Name = "Agreement", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public bool Agreed { get; set; }

        [Display(Name = "HasExecutingAgent", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public bool HasExecutingAgent { get; set; } = true;

        public List<AttachmentFileModel> OtherAttachmes { get; set; } = new List<AttachmentFileModel>();

        #region Behavior

        public void Init()
        {
            MortgageImplementationAgent = MortgageImplementationAgent ?? new MortgageImplementationAgentModel();
            ContractAttachment = ContractAttachment ?? new MortgageAttachmentModel();
            BankDepositDetail = BankDepositDetail ?? new BankDepositDetailModel();
            Sadad = Sadad ?? new Sadad.SadadModel();
        }

        #endregion
    }

    public class MortgagedAndOwnerModel
    {
        public bool IsMortgaged = true;
        public MortgagedAndOwnerModel()
        {

        }

        public MortgagedAndOwnerModel(bool isMortgaged)
        {
            this.IsMortgaged = isMortgaged;
        }

        public int Id { get; set; }

        [Display(Name = "MortgagorType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public int CategoryId { get; set; }

        [Display(Name = "MortgagorRole", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public int? RoleId { get; set; }

        [Display(Name = "IdNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string IdentityNumber { get; set; }
        public string RetrievedIdentityNumber { get; set; }

        [Display(Name = "DateOfBirth", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string BirthdateString { get; set; }

        public DateTime? Birthdate { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Name { get; set; }

        [Display(Name = "MobileNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MobileNumber { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Nationality { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "CommercialRegisterNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        public string CommercialRegisterNumber { get; set; }
    }

    public class MortgageImplementationAgentModel
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "Name", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Name { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public int? NationallyId { get; set; }

        [Display(Name = "Nationality", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Nationality { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [StringLength(maximumLength: 10, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar), MinimumLength = 10)]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "MobileNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Mobile { get; set; }

        [StringLength(maximumLength: 20, ErrorMessageResourceName = "MaxMinLength", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar), MinimumLength = 2)]
        [Display(Name = "LicenseNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string LicenseNumber { get; set; }

    }

    public class MortgageAttachmentModel
    {
        [Required(ErrorMessageResourceName = "RequiredMessage", ErrorMessageResourceType = typeof(Resources.SharedStrings_Ar))]
        [Display(Name = "ContractFile", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string ContractFile { get; set; }

        [Display(Name = "OtherFile", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string OtherFile { get; set; }

        public List<AttachmentFileModel> OtherFiles { get; set; } = new List<AttachmentFileModel>();
    }

    public class AttachmentFileModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public string FileReference { get; set; }
    }
}
