using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unity;
using Unity.Lifetime;
using URCP.Core;
using URCP.SqlServerRepository;
using Microsoft.Practices.Unity.Configuration;

namespace URCP.Domain.UnitTest
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        static IUnityContainer Container = new UnityContainer();
        private static UserService _userService;
        private static RoleService _roleService;
        
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());
            Container.RegisterType<BaseService>();
            Container.LoadConfiguration();
            URCP.Domain.Extensions.Init(Container);
            _userService = Container.Resolve<UserService>();
            _roleService = Container.Resolve<RoleService>();

            var adminUser = _userService.FindByNameAsync("superadmin").Result;


            if (adminUser == null)
            {
                var adminRole = _roleService.FindByName(RoleNames.Admin);
                User firstUser = new User("superadmin", "مدير النظام", "Administrator", true, "00966565969703", "Admin@admin.com", false, UserType.Superadmin)
                {
                    IdentityType = IdentityType.Iqama,
                    IdentityNumber = "0000000000",
                    Birthdate = DateTime.Now.AddYears(-20)
                };
                firstUser.UpdateRoles(new System.Collections.Generic.List<IdentityRole>() { adminRole });

                _userService.SkipPermission();
                var u = _userService.Create(firstUser, "P@ssw0rd");
                _userService.Commit();

            }
        }
    }
}
