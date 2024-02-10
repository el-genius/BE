using MCI.Mvc.Validation.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class LoginViewModel
    {
        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "طول الحقل غير صحيح")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "طول الحقل غير صحيح")]
        [Display(Name = "كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool? IsCaptchaRequired { get; set; }

        [Required(ErrorMessage = "ادخل رمز المرور")]
        [Display(Name = "رمز المرور")]
        [Captcha(ErrorMessage = "رمز غير صحيح")]
        [StringLength(6, MinimumLength = 4)]
        public string Captcha { get; set; }
    }
}