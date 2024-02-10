using System;
using System.ComponentModel.DataAnnotations;
using URCP.Core.Enum;

namespace URCP.Web.Models
{
    public class MortgageInquiryGridModel /// fix this and change it to mroge model
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public MortgageInquiryStatus Status { get; set; }
        public Guid ReportReference { get; set; }
        public bool MyMortgageInquires { get; set; }

        [Display(Name = "RequestNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string RequestNumber { get; set; }

        [Display(Name = "RequestDate", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string RequestDate { get; set; }


        [Display(Name = "RequestStatus", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string StatusName { get; set; }

        [Display(Name = "MortgagorName", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageOwnerName { get; set; }

        [Display(Name = "MortgageNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageNumber { get; set; }
    }
}