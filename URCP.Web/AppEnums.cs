using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URCP.Web
{
    public enum Test
    {
        [Display(Name = "معلومات")]
        Info = 1,
        Danger,
        Success,
        Warning
    }

    public enum StationSearchByType
    {
        //[Display(Name = "معلومات")]
        StationName = 1,
        LicenseNo,
        CrOwner,
        NId
    }
}