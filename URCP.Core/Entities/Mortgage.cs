using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using URCP.Core.Basis;
using URCP.Core.Enum;
using URCP.Core.Lookups;
using URCP.Core.Model;

namespace URCP.Core.Entities
{
    public class Mortgage : AuditableEntity
    {
        #region Ctor
        public Mortgage()
        {

        }

        public Mortgage(MortgageStatus status)
            : this()
        {
            this.Status = status;
        } 
        #endregion

        [Key]
        public int Id { get; set; }
         
        [Required(ErrorMessage = "RequestId field is required")]
        [ForeignKey(nameof(Request))]
        public int RequestId { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "ItemReferenceNumber field length is invalid", MinimumLength = 3)]
        public string ItemReferenceNumber { get; set; }

        [Required(ErrorMessage = "MortgagedId field is required")]
        [ForeignKey(nameof(Mortgaged))]
        public int MortgagedId { get; set; }

        [Required(ErrorMessage = "MortgagedCategoryId field is required")]
        [ForeignKey(nameof(MortgagedCategory))]
        public int MortgagedCategoryId { get; set; }

        [ForeignKey(nameof(MortgagedRole))]
        public int? MortgagedRoleId { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "MortgagedCrNumber field length is invalid", MinimumLength = 0)]
        public string MortgagedCrNumber { get; set; }

        [Required(ErrorMessage = "MortgageOwnerId field is required")]
        [ForeignKey(nameof(MortgageOwner))]
        public int MortgageOwnerId { get; set; }

        [Required(ErrorMessage = "MortgageOwnerCategoryId field is required")]
        [ForeignKey(nameof(MortgageOwnerCategory))]
        public int MortgageOwnerCategoryId { get; set; }

        [ForeignKey(nameof(MortgageOwnerRole))]
        public int? MortgageOwnerRoleId { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "MortgageOwnerCrNumber field length is invalid", MinimumLength = 0)]
        public string MortgageOwnerCrNumber { get; set; }

        [Required(ErrorMessage = "TypeId field is required")]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public MortgageStatus Status { get; set; }

        [StringLength(maximumLength: 2000, ErrorMessage = "MortgageMoneyDescription field length is invalid", MinimumLength = 3)]
        public string MortgageMoneyDescription { get; set; }

        [Required(ErrorMessage = "MortgageMoneyApproximately field is required")]
        public decimal MortgageMoneyApproximately { get; set; }

        [Required(ErrorMessage = "Debt field is required")]
        public decimal Debt { get; set; }

        [Required(ErrorMessage = "ContractExpireDate field is required")]
        public DateTime ContractExpireDate { get; set; }

        [Required(ErrorMessage = "MortgageImplementationMethodId field is required")]
        [ForeignKey(nameof(MortgageImplementationMethod))]
        public int MortgageImplementationMethodId { get; set; }

        [Required(ErrorMessage = "MortgageImplementationMethodId field is required")]
        [ForeignKey(nameof(MoneyStatus))]
        public int MoneyStatusId { get; set; }

        [Required(ErrorMessage = "MortgageImplementationMethodId field is required")]
        [ForeignKey(nameof(MoneyType))]
        public int MoneyTypeId { get; set; }

        [ForeignKey(nameof(MortgageImplementationAgent))]
        public int? MortgageImplementationAgentId { get; set; }

        public int? Priority { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "CancelReason field length is invalid", MinimumLength = 0)]
        public string CancelReason { get; set; }

        public DateTime? CancelDate { get; set; }

        [Required(ErrorMessage = "ContractAttachmentReference field is required")]
        public Guid ContractAttachmentReference { get; set; }

        public List<MortgageAttachment> OtherAttachments { get; private set; }

       

        #region NotMapped
        [NotMapped]
        public string Number
        {
            get
            {
                return String.Format("{0:5000000000}", this.Id);
            }
        }

        [NotMapped]
        public AttachmentModel ContractAttachmentModel { get; set; }

        [NotMapped]
        public List<AttachmentModel> OtherAttachmentsModel { get; set; }

        [NotMapped]
        public Invoice Invoice { get; set; }

        [NotMapped]
        public MortgageFinancialSharesDetail MortgageFinancialSharesDetail { get; set; }

        [NotMapped]
        public MortgageBankAccountDetail MortgageBankAccountDetail { get; set; }

        [NotMapped]
        public MortgageBankDepositDetail MortgageBankDepositDetail { get; set; }

        [NotMapped]
        public MortgageOtherMoneyDetail MortgageOtherMoneyDetail { get; set; } 
        #endregion

        #region Navigation
        public Request Request { get; set; }

        public MortgageImplementationAgent MortgageImplementationAgent { get; set; }

        public User Mortgaged { get; set; }

        public Lookup MortgagedCategory { get; set; }

        public Lookup MortgagedRole { get; set; }

        public User MortgageOwner { get; set; }

        public Lookup MortgageOwnerCategory { get; set; }

        public Lookup MortgageOwnerRole { get; set; }
        public Lookup MoneyStatus { get; set; }

        public Lookup MoneyType { get; set; }

        public Lookup Type { get; set; }

        public Lookup MortgageImplementationMethod { get; set; } 
        #endregion

        #region Behavior
        public Mortgage AttachRequest(Request request)
        {
            this.Request = request ?? throw new ArgumentNullException($"{nameof(this.Request)} can not be null");
            this.RequestId = this.Request.Id;

            return this;
        }

        public Mortgage AttachMortgageImplementationAgent(MortgageImplementationAgent mortgageImplementationAgent)
        {
            this.MortgageImplementationAgent = mortgageImplementationAgent ?? throw new ArgumentNullException($"{nameof(this.MortgageImplementationAgent)} can not be null");
            this.MortgageImplementationAgentId = this.MortgageImplementationAgent.Id;

            return this;
        }

        public Mortgage AttachMortgaged(User mortgaged)
        {
            this.Mortgaged = mortgaged ?? throw new ArgumentNullException($"{nameof(this.Mortgaged)} can not be null");
            this.MortgagedId = this.Mortgaged.Id;

            return this;
        }

        public Mortgage AttachMortgageOwner(User mortgageOwner)
        {
            this.MortgageOwner = mortgageOwner ?? throw new ArgumentNullException($"{nameof(this.MortgageOwner)} can not be null");
            this.MortgageOwnerId = this.MortgageOwner.Id;

            return this;
        }

        public Mortgage ChangeStatus(MortgageStatus status)
        {
            this.Status = status;
            SetUpdate();

            return this;
        }

        public Mortgage AddAttachment(MortgageAttachment attachment)
        {
            this.OtherAttachments = this.OtherAttachments ?? new List<MortgageAttachment>();
            this.OtherAttachments.Add(attachment);
            return this;
        }

        #endregion
    }
}
