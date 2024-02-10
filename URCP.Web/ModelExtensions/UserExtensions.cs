using URCP.Core;
using URCP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace URCP.Web
{
    public static class UserExtensions
    {
        public static UserModel ToModel(this User entity)
        {
            return new UserModel
            {
                UserID = entity.Id,
                UserName = entity.UserName,
                Mobile = entity.Mobile,
                FullName = entity.FullName,
                Email = entity.Email,
                IsActive = entity.Active,
                IsADUser = entity.IsADUser,
                Roles = entity.Roles.Select(r => r.Name).ToArray()
            };
        }

        public static List<UserModel> ToModels(this IQueryResult<User> users)
        {
            var result = from c in users.Items select c.ToModel();
            return result.ToList();
        }

        public static WebGridList<UserModel> ToWebGridListModel(this IQueryResult<User> users)
        {
            var result = new WebGridList<UserModel>
            {
                List = users.ToModels(),
                PageSize = users.PageSize,
                RowCount = users.TotalCount,
                PageIndex = users.PageNumber
            };
            return result;
        }

        public static UserEditModel ToEditModel(this UserModel model)
        {
            return new UserEditModel(model).FillDDLs();
        }
    }
}