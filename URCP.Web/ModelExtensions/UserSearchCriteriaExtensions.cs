using URCP.Core; 
using URCP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URCP.Core.Util;

namespace URCP.Web
{
    public static class UserSearchCriteriaExtensions
    {
        public static UserSearchCriteria ToEntity(this UserSearchCriteriaModel model)
        {
            return new UserSearchCriteria
            {
                FullName = model.FullName,
                Mobile = model.Mobile,
                RoleName = model.RoleName,
                UserName = model.UserName,
                Email = model.Email,
                IsActive = model.IsActive,
                IsActiveDirectory = model.IsADUser,
                PageSize = KeyConfig.PageSize,
                PageNumber = model.PageNumber,
                Sort = model.Sort,
                SortDirection = model.SortDirection,
            };
        }
    }
}