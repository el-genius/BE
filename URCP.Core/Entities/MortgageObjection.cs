using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;
using URCP.Core.Enum;

namespace URCP.Core.Entities
{
    public class MortgageObjection : AuditableEntity
    {
        public MortgageObjection()
        {

        }

        public MortgageObjection(int mortgageId, string reason)
        {
            this.MortgageId = mortgageId;
            this.Reason = reason;
        }
          
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="RequestId is required")]
        [ForeignKey(nameof(Request))]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "MortgageId is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; set; }

        [StringLength(maximumLength: 2000, ErrorMessage = "Reason field length is invalid", MinimumLength = 3)]
        public string Reason { get; set; }

        public MortgageObjectionStatus Status { get; private set; }

        #region Navigation
        public Request Request { get; set; }

        public Mortgage Mortgage { get; set; }
        #endregion

        #region Behavior
        public MortgageObjection AttachRequest(Request request)
        {
            this.Request = request ?? throw new ArgumentNullException($"{nameof(this.Request)} can not be null");
            this.RequestId = this.Request.Id;

            return this;
        }

        public MortgageObjection ChangeStatus(MortgageObjectionStatus status)
        {
            this.Status = status;
            SetUpdate();

            return this;
        }
        #endregion
    }
}
