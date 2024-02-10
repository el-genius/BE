using System.ComponentModel.DataAnnotations;
using URCP.Core.Enum;

namespace URCP.Web.Models
{
    public class RequestGridModel
    {
        [Display(Name = "رقم الطلب")]
        public int RequestId { get; set; }

        [Display(Name = "اسم الراهن")]
        public string MortgagorName { get; set; }

        [Display(Name = "تاريخ الطلب")]
        public string RequestDate { get; set; }

        [Display(Name = "حالة الطلب")]
        public RequestStatus RequestStatus { get; set; }

    }
}