using URCP.Core;
using URCP.RepositoryInterface; 
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace URCP.Domain
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        private IActiveDirectoryRepository _activeDirectoryRepository;

        public ApplicationSignInManager(UserService userManager, IActiveDirectoryRepository activeDirectoryRepository, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            _activeDirectoryRepository = activeDirectoryRepository; 
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserService)UserManager);
        } 
    }
}
