using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;

namespace URCP.Core.Entities
{
    public class Invoice : AuditableEntity
    {
        public Invoice()
        {
            this.Status = InvoiceStatus.AwaitGenerate;
        }

        public Invoice(double cost, double vatCost, double total) 
            : this()
        {  
            this.Cost = cost;
            this.VatCost = vatCost;
            this.TotalCost = total;
            this.Status = InvoiceStatus.AwaitGenerate;
        }

        #region Prop
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "RequestId field is required")]
        [ForeignKey(nameof(Request))]
        public int RequestId { get; set; }
        
        public double? Cost { get; set; }
        
        public double? VatCost { get; set; }
        
        public double? TotalCost { get; set; }

        public long? BillId { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "SadadNumber ength is invalid", MinimumLength = 0)]
        public string BillReferenceNumber { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "SadadNumber length is invalid", MinimumLength = 0)]
        public string SadadNumber { get; set; }

        public DateTime? BillIssueDateTime { get; set; }

        public DateTime? BillDueDateTime { get; set; }

        public DateTime? PaymentDateTime { get; private set; }

        [Required(ErrorMessage = "InvoiceStatus")]
        public InvoiceStatus Status { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "ErrorCode length is invalid", MinimumLength = 0)]
        public string ErrorCode { get; set; }
        #endregion

        #region Navigation
        public Request Request { get; set; }
        #endregion
         

        #region Behavior
        public Invoice AttachRequest(Request request)
        {
            this.Request = request ?? throw new ArgumentNullException($"{nameof(this.Request)} can not be null");
            this.RequestId = this.Request.Id;

            return this;
        }

        public Invoice Update(long billId, Guid referenceNumber, long? sadadNumber, DateTime? billIssueDateTime, DateTime? billDueDateTime,
            double cost, double? vat, double total)
        {
            this.BillId = billId;
            this.BillReferenceNumber = referenceNumber.ToString();
            this.SadadNumber = sadadNumber.ToString();
            this.BillIssueDateTime = billIssueDateTime;
            this.BillDueDateTime = billDueDateTime;
            this.Cost = cost;
            this.VatCost = vat;
            this.TotalCost = total;
            this.Status = InvoiceStatus.AwaitPayment;
            this.SetUpdate();

            return this;
        }

        /// <summary>
        /// Set paymentDateTime and change bill status to paid
        /// </summary>
        /// <param name="paymentDateTime"></param>
        /// <returns></returns>
        public Invoice Update(DateTime paymentDateTime)
        {
            this.PaymentDateTime = paymentDateTime;
            this.Status = InvoiceStatus.Paid;
            this.SetUpdate();

            return this;
        }

        public Invoice ChangeStaus(InvoiceStatus status)
        {
            this.Status = status;
            this.SetUpdate();

            return this;
        }
        #endregion
    }
}
