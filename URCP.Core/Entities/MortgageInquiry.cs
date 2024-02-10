using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;
using URCP.Core.Enum;
using URCP.Core.Model;

namespace URCP.Core.Entities
{
    [Table(name: "MortgeageInquiries")]
    public class MortgageInquiry : AuditableEntity
    {
        public MortgageInquiry()
        {

        }

        public MortgageInquiry(MortgageInquiryType type, string mortgageNumber, MortgageInquiryIdentityType mortgageInquiryIdentityType, string mortgageInquiryIdentityNumber)
        {
            this.Type = type;
            this.MortgageNumber = mortgageNumber;
            this.MortgageInquiryIdentityType = mortgageInquiryIdentityType;
            this.MortgageInquiryIdentityNumber = mortgageInquiryIdentityNumber;
        }

        [NotMapped]
        public string MortgageNumber { get; private set; }

        [NotMapped]
        public AttachmentModel ReportAttachmentModel { get; set; }

        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string Number
        {
            get
            {
                return String.Format("{0:0000000000}", this.Id);
            }
        }

        [Required(ErrorMessage = "RequestId is required")]
        [ForeignKey(nameof(Request))]
        public int RequestId { get; private set; }

        public MortgageInquiryType Type { get; private set; }
         
        public MortgageInquiryIdentityType? MortgageInquiryIdentityType { get; set; }
         
        [StringLength(maximumLength: 10, ErrorMessage = "MortgageInquiryIdentity field length is invalid", MinimumLength = 0)]
        public string MortgageInquiryIdentityNumber { get; set; }

        public List<MortgageInquiryDetail> MortgageInquiryDetails { get; private set; }

        public Guid ReportReference { get; private set; }
         
        public MortgageInquiryStatus Status { get; set; }

        [NotMapped]
        public Invoice Invoice { get; set; }

        #region Navigation
        public Request Request { get; private set; }
        #endregion

        #region Behavior
        public MortgageInquiry ChangeStatus(MortgageInquiryStatus status)
        {
            this.Status = status;
            SetUpdate();

            return this;
        }

        public MortgageInquiry AttachMortgageInquiryDetails(List<MortgageInquiryDetail> details)
        {
            this.MortgageInquiryDetails = details;
            return this;
        }

        public MortgageInquiry SetContractRefrence(Guid contractReference)
        {
            this.ReportReference = contractReference;
            SetUpdate();

            return this;
        }

        public MortgageInquiry AttachRequest(Request request)
        {
            this.Request = request ?? throw new ArgumentNullException($"{nameof(this.Request)} can not be null");
            this.RequestId = this.Request.Id;

            return this;
        }
        #endregion

    }
}
