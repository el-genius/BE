using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class MortgageInquiresModel
    {
        public WebGridList<MortgageInquiryGridModel> MortgageInquiryGrid { get; set; }
    }
}