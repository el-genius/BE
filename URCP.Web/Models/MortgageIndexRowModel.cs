using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class MortgageIndexRowModel
    {
        public WebGridList<MortgageGridModel> MortgageGrid { get; set; }
    }
}