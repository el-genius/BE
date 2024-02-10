using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using URCP.Core.Util;
using URCP.Domain;
using URCP.Domain.Interface;
using URCP.RepositoryInterface;
using URCP.ServicesRepository;
using URCP.ServicesRepository.Interface;
using URCP.SqlServerRepository;
using URCP.WinSvc.Security;

namespace URCP.WinSvc
{
    public class Program
    {
        static IUnityContainer Container = new UnityContainer();


        private static UserService _userService;
        private static Service _service;



        static void Main(string[] args)
        {
            Container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());
            Container.LoadConfiguration();  

            _userService = Container.Resolve<UserService>();
            _service = Container.Resolve<Service>();


            var user = _userService.FindByUserName(KeyConfig.WinSvcAuthorizedUser);

            if (user != null)
            {
                var roleNames = _userService.GetRoles(user).ToArray();

                var originalIdentity = new GenericIdentity(user.Name);
                var originalPrincipal = new GenericPrincipal(originalIdentity, roleNames);
                var myUserProfilePrincipal = new UserProfilePrincipal(originalPrincipal, originalIdentity, user);

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.SetThreadPrincipal(myUserProfilePrincipal);

                _service.Run();
            }
        } 
    }
}
