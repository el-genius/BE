using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.ViewModels
{
    public class CityViewModel
    {
        public int CityID { get; set; }

        public String ArabicName { get; set; }

        public String EnglishName { get; set; }

        public int RegionID { get; set; } 
    }
}