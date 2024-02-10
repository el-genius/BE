using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.ViewModels
{
    public class CountryViewModel
    {
        public String CountryID { get; set; }

        public String ArabicName { get; set; }
         
        public String EnglishName { get; set; } 

        public String AreaCode { get; set; }

        #region Ctor
        public CountryViewModel()
        {

        }

        public CountryViewModel(String countryID, String arabicName, String englishName, String areaCode)
        {
            this.CountryID = countryID;
            this.ArabicName = arabicName;
            this.EnglishName = englishName;
            this.AreaCode = areaCode;
        }
        #endregion
    }
}