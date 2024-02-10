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
    public class MortgageOtherMoneyDetail : AuditableEntity
    {
        /// <summary>
        /// نوع المال المرهون - أموال أخرى 
        /// </summary>
        public MortgageOtherMoneyDetail()
        {

        }

        public MortgageOtherMoneyDetail(Mortgage mortgage, string typeOfTransferredMoney, decimal amount, string category)
            : this()
        {
            this.Mortgage = mortgage;
            this.TypeOfTransferredMoney = typeOfTransferredMoney;
            this.Amount = amount;
            this.Category = category; 
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "MortgageId field is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; set; }

        [Required(ErrorMessage = "TypeOfTransferredMoney field is required")]
        [StringLength(maximumLength: 500, ErrorMessage = "TypeOfTransferredMoney field length is invalid", MinimumLength = 1)]
        public string TypeOfTransferredMoney { get; set; }

        [Required(ErrorMessage = "Amount field is required")] 
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Category field is required")]
        [StringLength(maximumLength: 500, ErrorMessage = "Category field length is invalid", MinimumLength = 1)]
        public string Category { get; set; }

        #region Navigation
        public Mortgage Mortgage { get; set; }
        #endregion

        #region Behavior
        public MortgageOtherMoneyDetail AttachMortgage(Mortgage mortgage)
        {
            this.Mortgage = mortgage ?? throw new ArgumentNullException($"{nameof(this.Mortgage)} can not be null");
            this.MortgageId = this.Mortgage.Id;

            return this;
        }
        #endregion
    }
}
