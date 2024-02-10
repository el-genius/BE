using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using URCP.Core.Util;
using URCP.Domain;
using URCP.Domain.Interface;
using URCP.Payment.Security;
using URCP.RepositoryInterface;
using URCP.ServicesRepository;
using URCP.ServicesRepository.Interface;
using URCP.SqlServerRepository;

namespace URCP.Payment
{
    class Program
    {
        static IUnityContainer Container = new UnityContainer();

        static void Main(string[] args)
        {
            Container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());
            Container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            Container.RegisterType<IGenericRepository, GenericRepository>();
            Container.RegisterType<IGenericQueryRepository, GenericQueryRepository>();
            Container.RegisterType<IIdentityUserRepository, IdentityUserRepository>();
            Container.RegisterType<IIdentityRoleRepository, IdentityRoleRepository>();
            Container.RegisterType<IMortgageImplementationAgentService, MortgageImplementationAgentService>();
            Container.RegisterType<IRequestService, RequestService>();
            Container.RegisterType<IRoleService, RoleService>();
            Container.RegisterType<IUserService, UserService>();
            Container.RegisterType<IMortgageService, MortgageService>();
            Container.RegisterType<IInvoiceService, InvoiceService>();
            Container.RegisterType<IeFileService, eFileService>();
            Container.RegisterType<IPaymentService, PaymentService>();
            Container.RegisterType<IeFileRepository, eFileRepository>();
            Container.RegisterType<IPaymentRepository, PaymentRepository>();
            Container.RegisterType<IAttachmentRepository, AttachmentRepository>();
            Container.RegisterType<IAttachmentService, AttachmentService>();
            var _userService = Container.Resolve<UserService>();


            var user = _userService.FindByUserName(KeyConfig.WinSvcAuthorizedUser);

            if (user != null)
            {
                var roleNames = _userService.GetRoles(user).ToArray();

                var originalIdentity = new GenericIdentity(user.Name);
                var originalPrincipal = new GenericPrincipal(originalIdentity, roleNames);
                var myUserProfilePrincipal = new UserProfilePrincipal(originalPrincipal, originalIdentity, user);

                var currentDomain = AppDomain.CurrentDomain;
                currentDomain.SetThreadPrincipal(myUserProfilePrincipal);

                Console.WriteLine("enter sadad number");
                var sadadNumber = Console.ReadLine();

                var _invoiceService = Container.Resolve<IInvoiceService>();
                try
                {
                    var invoice = _invoiceService.FindBy(new Core.SearchEntities.InvoiceSearchCriteria { SadadNumber = sadadNumber, IncludeRequest = true }).Items.FirstOrDefault();

                    if (invoice != null)
                    {
                        var _mortgageService = Container.Resolve<IMortgageService>();
                        if (invoice.Request.Type == Core.Enum.RequestType.RegisterMortgage)
                        {
                            _mortgageService.PayInvoice(invoice.RequestId, Core.Entities.InvoiceStatus.Paid);
                            Console.WriteLine("Paid successfully");
                            Console.ReadLine();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: ", ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
