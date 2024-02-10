using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using Unity;
using Unity.Lifetime;
using Unity.Wcf;
using URCP.Domain;
using URCP.Domain.Interface;
using URCP.RepositoryInterface;
using URCP.ServicesRepository;
using URCP.ServicesRepository.Interface;
using URCP.SqlServerRepository;

namespace URCP.WebSvc
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var serviceHost = new UnityServiceHost(serviceType, baseAddresses);
            var container = new UnityContainer();

            //configure container
            container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());

            //var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = @"D:\Unity\Unity.config" };
            //var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            //var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");
            //container.LoadConfiguration(unitySection);

            container.LoadConfiguration();

            serviceHost.Container = container;

            var userService = serviceHost.Container.Resolve<IUserService>();
            RoleAuthorizationManager.UserService = userService;

            return serviceHost;
        }
    }
}