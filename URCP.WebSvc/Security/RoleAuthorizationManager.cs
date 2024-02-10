using URCP.Domain;
using System;
using System.Linq;
using System.Diagnostics;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using URCP.Domain.Interface;
using Unity.Attributes;

namespace URCP.WebSvc
{
    public class RoleAuthorizationManager : ServiceAuthorizationManager
    {
        [Dependency("gfdgf")]
        public static IUserService UserService { get; set; }
        //private readonly IUserService _userService;
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                var user = UserService.FindByUserName("superadmin");
                var roleNames = UserService.GetRoles(user).ToArray();

                var originalIdentity = new GenericIdentity(user.UserName);
                var originalPrincipal = new GenericPrincipal(originalIdentity, roleNames);
                var myUserProfilePrincipal = new UserProfilePrincipal(originalPrincipal, originalIdentity, user);

                operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] =
                    Thread.CurrentPrincipal = myUserProfilePrincipal;

                return true;
            }
            catch (NullReferenceException e)
            {
                var msg = String.Format("Login Information was incorrect please check. Details exception {0}", e.InnerException);
                Trace.TraceWarning(msg);
                throw new FaultException(msg);
            }
            catch (Exception e)
            {
                var msg = String.Format("Please send the credentials information in the correct format. Details exception {0}", e.InnerException);
                Trace.TraceWarning(msg);
                throw new FaultException(msg);
            }
        }
    }
}