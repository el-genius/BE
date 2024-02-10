using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;

namespace URCP.Domain.Interface
{
    public interface IRoleService
    {
        List<IdentityRole> GetByNames(string[] roleNames);

        IdentityRole Create(IdentityRole role);

        IdentityRole FindById(int roleId);

        IQueryResult<IdentityRole> FindByIds(List<string> ids);

        IdentityRole FindByName(string roleName);

        IdentityRole Update(IdentityRole role);
    }
}
