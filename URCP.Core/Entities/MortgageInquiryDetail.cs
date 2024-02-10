using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URCP.Core.Entities
{
    public class MortgageInquiryDetail
    {
        public MortgageInquiryDetail()
        {

        }

        public MortgageInquiryDetail(MortgageInquiry mortgageInquiry, Mortgage mortgage, string mortgageInfo)
        {
            this.MortgageInquiry = mortgageInquiry ?? throw new ArgumentException($"{nameof(mortgageInquiry)} can not be null");
            this.MortgageInquiryId = this.MortgageInquiry.Id;

            this.Mortgage = mortgage ?? throw new ArgumentNullException($"{nameof(mortgage)} can not be null");
            this.MortgageId = this.Mortgage.Id;

            if(String.IsNullOrWhiteSpace(mortgageInfo))
                throw new ArgumentNullException($"{nameof(mortgageInfo)} can not be null");

            this.MortgageInfo = mortgageInfo;
        }

        [Key]
        public int Id { get; private set; }

        [Required(ErrorMessage = "RequestId is required")]
        [ForeignKey(nameof(MortgageInquiry))]
        public int MortgageInquiryId { get; private set; }

        [Required(ErrorMessage = "RequestId is required")]
        [ForeignKey(nameof(Mortgage))]
        public int MortgageId { get; private set; }

        [Required(ErrorMessage = "MortgageInfo is required")]
        [StringLength(maximumLength: 5000, ErrorMessage = "MortgageInfo field length is invalid", MinimumLength = 2)]
        public string MortgageInfo { get; private set; }


        #region Navigation
        public Mortgage Mortgage { get; private set; }

        public MortgageInquiry MortgageInquiry { get; private set; }
        #endregion
    }
}
