using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity.Configuration;
using Moq;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.SearchEntities;
using System;
using System.Threading;
using System.Security.Principal;
using System.Linq;
using URCP.Core.Lookups;
using Unity;
using URCP.RepositoryInterface;
using URCP.SqlServerRepository;
using URCP.Domain.Interface;
using Unity.Lifetime;
using URCP.Core.Util;
using URCP.Core.Model;

namespace URCP.Domain.UnitTest
{
    public enum MortgageType
    {
        IndividualToIndividual,

        IndividualToIndividualAndMoneyTypeIsFinancialShares,

        IndividualToDelegated,

        IndividualToCompanyManager,

        IndividualToCompanyOwner,

        DelegatedToIndividual,

        DelegatedToDelegated,

        DelegatedToCompanyManager,

        DelegatedToCompanyOwner,

        CompanyOwnerToIndividual,

        CompanyOwnerToDelegated,

        CompanyOwnerToCompanyManager,

        CompanyOwnerToCompanyOwner,

        CompanyManagerToIndividual,

        CompanyManagerToDelegated,

        CompanyManagerToCompanyOwner,

        CompanyManagerToCompanyManager

    }

    [TestClass]
    public class MortgageServiceTests
    {
        IUnityContainer Container = new UnityContainer();
        private static UserService _userService;
        private MortgageService _mortgageService;
        private RoleService _roleService;

        [TestInitialize]
        public void Setup()
        {
            Container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());
            Container.RegisterType<BaseService>();
            Container.LoadConfiguration();
            URCP.Domain.Extensions.Init(Container);

            var mockPrincipal = new Mock<IPrincipal>();


            #region Mock eFile Service
            var mockEfile = new Mock<IeFileService>();
            var eFileuser = new User("2322573508", "mortgaged_unitTest", "asharu", true, "00966565969701", "asharu@email.com", false, UserType.eFileUser)
            {
                IdentityType = IdentityType.Iqama,
                IdentityNumber = "2322573508",
                Birthdate = DateTime.Now.AddYears(-20)
            };

            mockEfile.Setup(m => m.FindUser(It.IsAny<string>(), It.IsAny<string>())).Returns(eFileuser);
            mockEfile.Setup(m => m.IsDelegatedFor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<eFileDelegateRole>())).Returns(true);
            Container.RegisterInstance<IeFileService>(mockEfile.Object);
            #endregion

            #region Mock Cr Service
            var mockCrService = new Mock<ICrService>();
            mockCrService.Setup(m => m.IsCrManager(It.IsAny<long>(), It.IsAny<string>())).Returns(true);
            mockCrService.Setup(m => m.IsCrOwner(It.IsAny<long>(), It.IsAny<string>())).Returns(true);
            Container.RegisterInstance<ICrService>(mockCrService.Object);
            #endregion

            #region Mock Attachment Service
            var mockAttchment = new Mock<IAttachmentService>();
            var fileReferene = new Guid();
            mockAttchment.Setup(m => m.Upload(It.IsAny<AttachmentModel>())).Returns(fileReferene);
            Container.RegisterInstance<IAttachmentService>(mockAttchment.Object);
            #endregion

            #region Mock PaymentGetway
            var mockPaymentGetway = new Mock<IPaymentService>();
            mockPaymentGetway.Setup(m => m.GenerateInvoice(It.IsAny<Request>(), It.IsAny<int>())).Returns(
                new InvoiceSummaryModel(1, 1, 2).Update(1, new Guid(), 222, DateTime.Now, DateTime.Now.AddDays(5)));
            Container.RegisterInstance<IPaymentService>(mockPaymentGetway.Object);
            #endregion

            #region Mock SMS Notification Service
            var mockSmsNotification = new Mock<ISmsNotificationService>();
            mockSmsNotification.Setup(m => m.CreateAsync(It.IsAny<SmsNotification>())).ReturnsAsync(
                new SmsNotification());
            Container.RegisterInstance<ISmsNotificationService>(mockSmsNotification.Object);
            #endregion

            _userService = Container.Resolve<UserService>();
            _roleService = Container.Resolve<RoleService>();
            _mortgageService = Container.Resolve<MortgageService>();

            #region Mock Superadmin User
            var user = _userService.FindByUserName("superadmin");
            mockPrincipal.Setup(ctx => ctx.Identity).Returns(user as IdentityUser);
            mockPrincipal.Setup(p => p.IsInRole(It.IsAny<string>())).Returns(true);
            Thread.CurrentPrincipal = mockPrincipal.Object;
            #endregion
        }

        Mortgage InitMortgage(MortgageType mortgageType)
        {
            var request = new Request(RequestType.RegisterMortgage, RequestStatus.AwaitGenerateInvoice);

            var mortgaged = _userService.FindByUserName("2322576750");
            if (mortgaged == null)
            {
                mortgaged = new User("2322576750", "mortgaged_unitTest", "mortgaged_unitTest", true, "0565888744", "mortgaged_unitTest@email.com", false, UserType.eFileUser)
                {
                    IdentityType = IdentityType.Iqama,
                    IdentityNumber = "2322576750",
                    Birthdate = DateTime.Now.AddYears(-20)
                };

                var eFileUserRoles = _roleService.FindByIds(KeyConfig.eFileService.UserRoles.Split(',').ToList());
                mortgaged.UpdateRoles(eFileUserRoles.Items.ToList());

                _userService.SkipPermission();
                var taskResult = _userService.Create(mortgaged, KeyConfig.eFileService.DefaultPassword);
                _userService.Commit();
                mortgaged = _userService.FindByUserName(taskResult.Result.UserName);
            }


            var mortgageOwner = _userService.FindByUserName("2322576751");
            mortgageOwner = mortgageOwner ?? new User("2322576751", "mortgageOwner900", "mortgageOwner900", true, "0565969703", "mortgageOwner900@email.com", false, UserType.eFileUser)
            {
                IdentityType = IdentityType.Iqama,
                IdentityNumber = "2322576751",
                Birthdate = DateTime.Now.AddYears(-20)
            };


            var SUT = new Mortgage()
            {
                TypeId = KeyConfig.Lookup.MortgageType.General,
                ItemReferenceNumber = "HZX-12345-BA23",
                Status = MortgageStatus.Default,
                MortgageMoneyDescription = "MortgageMoneyDescription",
                MortgageMoneyApproximately = 12134.2M,
                Debt = 4646.2M,
                MoneyStatusId = 14,
                MoneyTypeId = KeyConfig.Lookup.MortgageMoneyType.CommercialEntity,
                ContractExpireDate = DateTime.Now.AddYears(1),
                MortgageImplementationMethodId = 1,
                MortgageImplementationAgent = new MortgageImplementationAgent()
                {
                    IdentityNumber = "2322432345",
                    NationallyId = 1,
                    Mobile = "0565969874",
                    Name = "Ahmed"
                },
                ContractAttachmentModel = new Core.Model.AttachmentModel()
                {
                    FileName = "UnitTest",
                    FileSize = 1,
                    FileType = "pdf",
                    Content = new byte[0]
                }
            };

            SUT.AttachRequest(request);
            SUT.AttachMortgaged(mortgaged);
            SUT.AttachMortgageOwner(mortgageOwner);

            switch (mortgageType)
            {
                case MortgageType.IndividualToIndividual:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    break;
                case MortgageType.IndividualToIndividualAndMoneyTypeIsFinancialShares:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MoneyTypeId = KeyConfig.Lookup.MortgageMoneyType.FinancialShares;

                    MortgageFinancialSharesDetail mortgageFinancialSharesDetail =
                        new MortgageFinancialSharesDetail(SUT, new Lookup() { Id = 30 }, Guid.NewGuid(), 1, 23, "1234567895", "testFileName");
                    SUT.MortgageFinancialSharesDetail = mortgageFinancialSharesDetail;
                    break;
                case MortgageType.IndividualToDelegated:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.IndividualToCompanyManager:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgageOwnerRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.IndividualToCompanyOwner:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgageOwnerRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.DelegatedToIndividual:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    break;
                case MortgageType.DelegatedToDelegated:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.DelegatedToCompanyManager:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.DelegatedToCompanyOwner:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.CompanyOwnerToIndividual:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId }; 
                    break;
                case MortgageType.CompanyOwnerToDelegated:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break; 
                case MortgageType.CompanyOwnerToCompanyManager:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break; 
                case MortgageType.CompanyOwnerToCompanyOwner:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.CompanyManagerToIndividual:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Individual;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId }; 
                    break;
                case MortgageType.CompanyManagerToDelegated:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.Delegated;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.CompanyManagerToCompanyOwner:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyOwner;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                case MortgageType.CompanyManagerToCompanyManager:
                    SUT.MortgagedCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgagedCategory = new Lookup() { Id = SUT.MortgagedCategoryId };
                    SUT.MortgagedRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgagedRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgagedCrNumber = "0123456789";

                    SUT.MortgageOwnerCategoryId = KeyConfig.Lookup.MortgagedOwnerCategory.Company;
                    SUT.MortgageOwnerCategory = new Lookup() { Id = SUT.MortgageOwnerCategoryId };
                    SUT.MortgageOwnerRoleId = KeyConfig.Lookup.MortgagedOwnerRole.CompanyManager;
                    SUT.MortgageOwnerRole = new Lookup() { Id = SUT.MortgagedRoleId.Value };
                    SUT.MortgageOwnerCrNumber = "0123456789";
                    break;
                default:
                    break;
            }

            return SUT;
        }

        [TestMethod]
        public void Create_IndividualToIndividualAndMoneyTypeIsFinancialShares_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.IndividualToIndividualAndMoneyTypeIsFinancialShares), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_IndividualToIndividual_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.IndividualToIndividual), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_IndividualToDelegated_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.IndividualToDelegated), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_IndividualToCompanyOwner_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.IndividualToCompanyOwner), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_IndividualToCompanyManager_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.IndividualToCompanyManager), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_DelegatedToIndividual_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.DelegatedToIndividual), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_DelegatedToDelegated_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.DelegatedToDelegated), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_DelegatedToCompanyOwner_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.DelegatedToCompanyOwner), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_DelegatedToCompanyManager_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.DelegatedToCompanyManager), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyOwnerToIndividual_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyOwnerToIndividual), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyOwnerToDelegated_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyOwnerToDelegated), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyOwnerToCompanyOwner_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyOwnerToCompanyOwner), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyOwnerToCompanyManager_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyOwnerToCompanyManager), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyManagerToIndividual_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyManagerToIndividual), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyManagerToDelegated_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyManagerToDelegated), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyManagerToCompanyOwner_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyManagerToCompanyOwner), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_CompanyManagerToCompanyManager_CreatedSuccessfully()
        {
            var SUT = _mortgageService.Create(InitMortgage(MortgageType.CompanyManagerToCompanyManager), "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        public void Create_SpecificMortgage_PriorityShouldBeEqualOne()
        {
            var SUT = InitMortgage(MortgageType.IndividualToIndividualAndMoneyTypeIsFinancialShares);
            SUT.TypeId = KeyConfig.Lookup.MortgageType.Specific;

            SUT = _mortgageService.Create(SUT, "123");
            var resultPayInvoice = _mortgageService.PayInvoice(SUT.RequestId, InvoiceStatus.Paid);
            resultPayInvoice.Wait();

            var result = _mortgageService.ManageMortgagePriority(SUT);
            result.Wait();

            SUT = _mortgageService.FindBy(new MortgageSearchCriteria()
            {
                Sort = "CreatedAt",
                SortDirection = WebGridSortOrder.Ascending,
                TypeId = KeyConfig.Lookup.MortgageType.Specific
            }).Items.FirstOrDefault();

            Assert.AreNotEqual(0, SUT.Id);
            Assert.IsNotNull(SUT.Priority);
            Assert.AreEqual(1, SUT.Priority.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessRuleException))]
        public void Create_MortgagedIdentityNumberEqualToMortgageOwnerIdentityNumber_ThrowBusinessRuleException()
        {
            var SUT = InitMortgage(MortgageType.IndividualToIndividual);
            SUT.Mortgaged.IdentityNumber = SUT.MortgageOwner.IdentityNumber;
            SUT = _mortgageService.Create(SUT, "123");

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_MortgagedRoleIdIsNull_ThrowValidationException()
        {
            var SUT = InitMortgage(MortgageType.CompanyManagerToCompanyManager);
            SUT.MortgagedRoleId = null;
            SUT.MortgagedRole = null;
            SUT = _mortgageService.Create(SUT, "123"); 
           
            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.MortgagedId);
            Assert.AreNotEqual(0, SUT.MortgageOwnerId);
            Assert.AreNotEqual(0, SUT.MortgageImplementationAgentId);
        }
         
        

    }
}
