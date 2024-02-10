using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URCP.Web.Models.ReportModel
{
    public class MorgtageReportModel
    {
        [Display(Name = "اسم المرتهن")]
        public string MortgagedName { get; set; }

        [Display(Name = "رقم الهوية / رقم الإقامة/ السجل التجاري")]
        public string MortgagedIdNumber { get; set; }

        [Display(Name = "اسم الراهن")]
        public string MortgageOwner { get; set; }

        [Display(Name = "رقم الهوية / رقم الإقامة/ السجل التجاري")]
        public string MortgageOwnerIdNumber { get; set; }

        [Display(Name = "وصف المال المرهون")]
        public string Description { get; set; }
    }
}