using URCP.Core;
using Microsoft.AspNet.Identity;

namespace URCP.RepositoryInterface
{
    public interface IIdentityRoleRepository : IRoleStore<IdentityRole, int>
    {

    }
}
