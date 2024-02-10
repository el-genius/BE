using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;
using URCP.Core.Lookups;

namespace URCP.Core.Entities
{
    /// <summary>
    /// وكيل التنفيذ 
    /// </summary>
    public class MortgageImplementationAgent : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [StringLength(maximumLength: 250, ErrorMessage = "Name field length is invalid", MinimumLength = 0)]
        public string Name { get; set; }

        [ForeignKey(nameof(Nationally))]
        public int? NationallyId { get; set; }

        [Required(ErrorMessage = "IdentityNumber field is required")]
        [StringLength(maximumLength: 10, ErrorMessage = "IdentityNumber field length is invalid", MinimumLength = 0)]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Mobile field is required")]
        [StringLength(maximumLength: 20, ErrorMessage = "Mobile field length is invalid", MinimumLength = 0)]
        public string Mobile { get; set; }

        [StringLength(maximumLength: 20, ErrorMessage = "LicenseNumber field length is invalid", MinimumLength = 2)]
        public string LicenseNumber { get; set; }

        #region Navigation
        public Country Nationally { get; set; }
        #endregion
    }
}
