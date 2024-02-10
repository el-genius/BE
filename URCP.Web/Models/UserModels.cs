using System.Linq;
using URCP.Core;
using URCP.Domain;
using MCI.Mvc.Validation.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace URCP.Web.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن 256 حرف")]
        public string UserName { get; set; }

        [Display(Name = "التفعيل")]
        public bool IsActive { get; set; }

        [Display(Name = "مستخدم سجل نشط")]
        public bool IsADUser { get; set; }

        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(200, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string FullName { get; set; }


        [RegularExpression(@"^05\d{8}$", ErrorMessage = "{0} غير صحيح")]
        [StringLength(10, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = "رقم الجوال")]
        public string Mobile { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [EmailAddress(ErrorMessage = "{0} غير صحيح")]
        public string Email { get; set; }


        [Display(Name = "الصلاحيات")]
        [Required(ErrorMessage = "الرجاء أختيار {0}")]
        public string[] Roles { get; set; }

        public UserModel()
        {

        }
    }

    public class UserEditModel
    {
        public int UserID { get; set; }

        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن 256 حرف")]
        public string UserName { get; set; }

        [Display(Name = "التفعيل")]
        public bool IsActive { get; set; }

        [Display(Name = "مستخدم سجل نشط")]
        public bool IsADUser { get; set; }

        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(200, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        public string FullName { get; set; }

        [RegularExpression(@"^05\d{8}$", ErrorMessage = "{0} غير صحيح")]
        [StringLength(10, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = "رقم الجوال")]
        public string Mobile { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [StringLength(256, ErrorMessage = "يجب أن لايزيد عن {1} حرف")]
        [EmailAddress(ErrorMessage = "{0} غير صحيح")]
        public string Email { get; set; }


        [Display(Name = "الصلاحيات")]
        [Required(ErrorMessage = "الرجاء أختيار {0}")]
        public string[] Roles { get; set; }

        public Dictionary<string, string> RoleNames { get; set; }

        public UserEditModel()
        {
            RoleNames = new Dictionary<string, string>();
        }

        public UserEditModel(UserModel model)
        {
            this.Email = model.Email;
            this.FullName = model.FullName;
            this.IsActive = model.IsActive;
            this.Mobile = model.Mobile;
            this.Roles = model.Roles;
            this.UserID = model.UserID;
            this.UserName = model.UserName;
            this.IsADUser = model.IsADUser;
        }

        public UserEditModel FillDDLs()
        {
            var Roles = Core.RoleNames.GetRolesWithCaptions(); 
            this.RoleNames = Roles;
            return this;
        }
    }

    public class UserAddModel : UserEditModel
    {
        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = "كلمة المرور")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = " الرجاء إدخال كلمة المرور على الأقل 6 أحرف ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال {0}")]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمات المرور المدخلة غير متطابقة.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = " الرجاء إدخال كلمة المرور على الأقل 6 أحرف ")]
        public string PasswordConfirm { get; set; }

        public bool IsChecked { get; set; }

        public UserAddModel() : base() { }

        public UserAddModel(UserModel model)
            : base(model)
        {
        }
    }
}