using System.Linq;
using URCP.Core;
using URCP.Domain;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections;

namespace URCP.Web.Models
{
    public class UserIndexModel
    {

        public UserSearchCriteriaModel userSearchCriteriaModel { get; set; }

        public WebGridList<UserModel> Items { get; set; }

        public Dictionary<string, string> RoleNames { get; set; }

        public UserIndexModel()
        {
            RoleNames = new Dictionary<string, string>();
            userSearchCriteriaModel = new UserSearchCriteriaModel();
            Activation = new Dictionary<string, string>();
            ActiveDirectory = new Dictionary<string, string>();
        }

        public UserIndexModel FillDDLs()
        {
            RoleNames = Core.RoleNames.GetRolesWithCaptions();

            Activation.Add("", "الكل");
            Activation.Add("True", "مفعل");
            Activation.Add("False", "غير مفعل");

            ActiveDirectory.Add("", "الكل");
            ActiveDirectory.Add("True", "نعم");
            ActiveDirectory.Add("False", "لا");

            return this;
        }

        public Dictionary<string, string> Activation { get; set; }
        public Dictionary<string, string> ActiveDirectory { get; set; }

    }
}