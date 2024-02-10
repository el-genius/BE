using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URCP.Core.Enum;

namespace URCP.Web.Models
{
    public class MortgageGridModel
    {
        [Display(Name = "رقم القيد")]
        public int MortgageId { get; set; }

        [Display(Name = "تاريخ قيد القيد")]
        public string MortgageStartDate { get; set; }

        [Display(Name = "اسم الراهن")]
        public string MortgagorName { get; set; }

        [Display(Name = "اسم المرتهن")]
        public string MortgageeName { get; set; }

        [Display(Name = "تاريخ انتهاء القيد")]
        public string MortgageExprDate { get; set; }

        [Display(Name = "إجراءات القيد")]
        public string MortgageAction { get; set; }

        [Display(Name = "حالة القيد")]
        public MortgageStatus MortgageStatus { get; set; }
    }
}