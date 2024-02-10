using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using URCP.Core.Basis;
using URCP.Core.Lookups;

namespace URCP.Core.Entities
{
    public class MortgageAttachment : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "MortgageId field is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; set; }

        [Required(ErrorMessage = "MortgageId field is required")]
        [StringLength(maximumLength: 200)]
        public string FileName { get; set; }

        public Guid FileReference { get; set; }

        #region Navigation
        public Mortgage Mortgage { get; set; }

        #endregion

        #region CTOR
        public MortgageAttachment()
        {

        }
        public MortgageAttachment(Mortgage mortgage, Guid referenceNumber, string fileName)
        {
            this.Mortgage = mortgage ?? throw new ArgumentNullException($"{nameof(mortgage)} can not be null");
            this.MortgageId = this.Mortgage.Id;

            if (referenceNumber == Guid.Empty)
                throw new ArgumentNullException($"{nameof(referenceNumber)} can not be null");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException($"{nameof(fileName)} can not be null");

            this.FileReference = referenceNumber;
            this.FileName = fileName;
        }

        #endregion
    }
}
