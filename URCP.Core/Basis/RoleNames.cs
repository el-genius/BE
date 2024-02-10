using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace URCP.Core
{
    public static class RoleNames
    {
        /// <summary>
        /// This user can access the complete system privileges.
        /// </summary>

        [PermissionCaption("مدير نظام")]
        public const string Admin = "/";

        #region Users
        [PermissionCaption("إدارة المستخدمين")]
        public const string UserManagement = "/User";

        [PermissionCaption("إضافة مستخدم")]
        public const string CreateUser = "/User/Create";

        [PermissionCaption("تعديل مستخدم")]
        public const string UpdateUser = "/User/Update";
        #endregion

        #region Lookups 
        [PermissionCaption("إدارة القوائم")]
        public const string LookupManagement = "/LookupManagement";

        [PermissionCaption("إضافة القوائم")]
        public const string CreateLookup = "/Lookup/Create";

        [PermissionCaption("تعديل القوائم")]
        public const string UpdateLookup = "/Lookup/Update";
        #endregion

        [PermissionCaption("إدارة الرهون العكسية")]
        public const string ReverseMortgageManagement = "/ReverseMortgageManagement";

        [PermissionCaption("إنشاء عقد رهن عكسي")]
        public const string CreateReverseMortgage = "/ReverseMortgageManagement/Create";

        [PermissionCaption("موافقة رهن عكسي")]
        public const string ApproveReverseMortgage = "/ReverseMortgageManagement/ApproveReverseMortgage";
        
        [PermissionCaption("موافقة الجهة")]
        public const string ApproveMortgage = "/MortgageManagement/ApproveMortgage";

        [PermissionCaption("إدارة الرهون")]
        public const string MortgageManagement = "/MortgageManagement";

        [PermissionCaption("إنشاء عقد رهن")]
        public const string CreateMortgage = "/MortgageManagement/Create";

        [PermissionCaption("تعديل الرهون")]
        public const string UpdateMortgage = "/MortgageManagement/Update";

        [PermissionCaption("اعتراض على قيد رهن")]
        public const string MortgageObjection = "/MortgageManagement/Objection";

        [PermissionCaption("إدارة اطلاع")]
        public const string InquiryManagement = "/InquiryManagement";

        [PermissionCaption("إضافة اطلاع")]
        public const string CreateInquiry = "/InquiryManagement/Create";

        /// <summary>
        /// This user can access the some of the system privileges in his/her organization.
        /// </summary>

        public static Dictionary<string, string> GetRolesWithCaptions()
        {
            var result = new Dictionary<string, string>();

            foreach (var field in typeof(RoleNames).GetFields())
            {
                if (field.IsPublic && field.IsLiteral)
                {
                    string name = field.GetValue(null) as string;
                    string caption = name;

                    object[] attrs = field.GetCustomAttributes(typeof(PermissionCaptionAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                        caption = ((PermissionCaptionAttribute)attrs[0]).Caption;

                    result.Add(name, caption);
                }
            }

            return result;
        }

        public static string GetRoleCaption(string roleName)
        {
            Type roleNamesType = typeof(RoleNames);
            System.Reflection.FieldInfo myFieldInfo = roleNamesType.GetField(roleName);

            string caption = "";

            object[] attrs = myFieldInfo.GetCustomAttributes(typeof(PermissionCaptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                caption = ((PermissionCaptionAttribute)attrs[0]).Caption;

            return caption;
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class PermissionCaptionAttribute : Attribute
    {
        // This is a positional argument
        public PermissionCaptionAttribute(string caption)
        {
            this.Caption = caption;
        }

        public string Caption { get; private set; }
    }
}
