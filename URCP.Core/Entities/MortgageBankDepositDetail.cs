using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;
using URCP.Core.Lookups;
using URCP.Core.Model;

namespace URCP.Core.Entities
{
    public class MortgageBankDepositDetail : AuditableEntity
    {
        #region Prop
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "MortgageId field is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; set; }

        [Required(ErrorMessage = "AccountNumber field is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "AccountNumber field length is invalid", MinimumLength = 1)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "BankId field is required")]
        [ForeignKey(nameof(Bank))]
        public int BankId { get; set; }

        [Required(ErrorMessage = "DepositNumber field is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "DepositNumber field length is invalid", MinimumLength = 1)]
        public string DepositNumber { get; set; }

        [Required(ErrorMessage = "MortgagedAmount field is required")]
        public decimal MortgagedAmount { get; set; }

        [Required(ErrorMessage = "CurrencyId field is required")]
        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "DepositTypeId field is required")]
        [ForeignKey(nameof(DepositType))]
        public int DepositTypeId { get; set; }

        [Required(ErrorMessage = "FileName field is required")]
        [StringLength(maximumLength: 200)]
        public string FileName { get; set; }

        [Required(ErrorMessage = "FileReference field is required")]
        public Guid FileReference { get; set; }
        #endregion

        #region NotMapped
        [NotMapped]
        public AttachmentModel AttachmentModel { get; set; }
        #endregion

        #region Navigation
        public Mortgage Mortgage { get; set; }

        public Lookup DepositType { get; set; }

        public Lookup Currency { get; set; }

        public Lookup Bank { get; set; }
        #endregion

        #region Behavior
        public MortgageBankDepositDetail AttachMortgage(Mortgage mortgage)
        {
            this.Mortgage = mortgage ?? throw new ArgumentNullException($"{nameof(this.Mortgage)} can not be null");
            this.MortgageId = this.Mortgage.Id;

            return this;
        }
        #endregion


    }
}
