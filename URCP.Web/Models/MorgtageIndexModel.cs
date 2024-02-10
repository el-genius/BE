using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class MorgtageIndexModel
    {
        public WebGridList<MorgtageGridModel> MorgagesAsMortgagedGrid { get; set; }
        public WebGridList<MorgtageGridModel> MorgagesAsMortgageOwnerGrid { get; set; }
    }
}