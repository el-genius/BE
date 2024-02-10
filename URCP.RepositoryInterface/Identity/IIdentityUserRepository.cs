using URCP.Core;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace URCP.RepositoryInterface
{
    public interface IIdentityUserRepository : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserEmailStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>,
        IUserRoleStore<User, int>
    { 
    }
}
