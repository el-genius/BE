using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using URCP.Core.Basis;
using URCP.Core.Lookups;
using URCP.Core.Model;

namespace URCP.Core.Entities
{
    public class MortgageFinancialSharesDetail : AuditableEntity
    {
        #region Ctor
        public MortgageFinancialSharesDetail()
        {

        }

        public MortgageFinancialSharesDetail(
            Mortgage mortgage,
            Lookup sharesType,
            Guid fileReference,
            decimal numberOfShares,
            decimal valueOfShares,
            string crNumber,
            string fileName) : this()
        {
            this.Mortgage = mortgage ?? throw new ArgumentNullException("Mortgage is null");
            this.MortgageId = this.Mortgage.Id;
            this.CrNumber = crNumber;
            this.NumberOfShares = numberOfShares;
            this.ValueOfShares = valueOfShares;
            this.SharesType = sharesType ?? throw new ArgumentNullException("SharesType is null");
            this.SharesTypeId = this.SharesType.Id;
            this.FileName = fileName;
            this.FileReference = fileReference;
        } 
        #endregion

        #region Prop
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "MortgageId field is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "FinancialSharesCrNumber field length is invalid", MinimumLength = 0)]
        public string CrNumber { get; set; }

        [Required(ErrorMessage = "NumberOfShares field is required")]
        public decimal NumberOfShares { get; set; }

        [Required(ErrorMessage = "ValueofShares field is required")]
        public decimal ValueOfShares { get; set; }

        [Required(ErrorMessage = "SharesTypeId field is required")]
        [ForeignKey(nameof(SharesType))]
        public int SharesTypeId { get; set; }

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

        public Lookup SharesType { get; set; }
        #endregion
         
        #region Behavior
        public MortgageFinancialSharesDetail AttachMortgage(Mortgage mortgage)
        {
            this.Mortgage = mortgage ?? throw new ArgumentNullException($"{nameof(this.Mortgage)} can not be null");
            this.MortgageId = this.Mortgage.Id;

            return this;
        }
        #endregion

    }
}
