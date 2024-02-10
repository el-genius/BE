using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity.Configuration;
using Moq;
using System;
using System.Security.Principal;
using System.Threading;
using Unity;
using Unity.Lifetime;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.Lookups;
using URCP.Core.Util;
using URCP.SqlServerRepository;

namespace URCP.Domain.UnitTest
{
    public enum ReverseMortgageMoneyType
    {
        Airplane,

        Cash,

        Ship,

        Trademark
    }

    [TestClass]
    public class ReverseMortgageServiceTest
    {
        IUnityContainer Container = new UnityContainer();
        private static UserService _userService;
        private ReverseMortgageService _reverseMortgageService;
        private RoleService _roleService;

        [TestInitialize]
        public void Setup()
        {
            Container.RegisterType<MyDbContext>(new PerResolveLifetimeManager());
            Container.RegisterType<BaseService>();
            Container.LoadConfiguration();
            URCP.Domain.Extensions.Init(Container);

            var mockPrincipal = new Mock<IPrincipal>();


            _userService = Container.Resolve<UserService>();
            _roleService = Container.Resolve<RoleService>();
            _reverseMortgageService = Container.Resolve<ReverseMortgageService>();

            #region Mock Superadmin User
            var user = _userService.FindByUserName("superadmin");
            mockPrincipal.Setup(ctx => ctx.Identity).Returns(user as IdentityUser);
            mockPrincipal.Setup(p => p.IsInRole(It.IsAny<string>())).Returns(true);
            Thread.CurrentPrincipal = mockPrincipal.Object;
            #endregion
        }

        ReverseMortgage InitMortgage(ReverseMortgageMoneyType reverseMortgageMoneyType)
        {
            var reverseMortgage = new ReverseMortgage()
            {
                MortgageOwnerName = "Ahmed Al Sharu",
                MortgageOwnerIdentityNumber = "2322575584"
            };

            switch (reverseMortgageMoneyType)
            {
                case ReverseMortgageMoneyType.Airplane:
                    reverseMortgage.ReverseMortgageMoneyType = new Lookup() { Id = KeyConfig.Lookup.ReverseMortgageMoneyType.Airplane };
                    reverseMortgage.ReverseMortgageMoneyTypeId = reverseMortgage.ReverseMortgageMoneyType.Id;
                    reverseMortgage.ReverseMortgageAirplaneDetail = new ReverseMortgageAirplaneDetail()
                    {
                        ModelNumber = "123456",
                        SerialNumber = "654321"
                    };
                    break;
                case ReverseMortgageMoneyType.Ship:
                    reverseMortgage.ReverseMortgageMoneyType = new Lookup() { Id = KeyConfig.Lookup.ReverseMortgageMoneyType.Ships };
                    reverseMortgage.ReverseMortgageMoneyTypeId = reverseMortgage.ReverseMortgageMoneyType.Id;
                    reverseMortgage.ReverseMortgageShipDetail = new ReverseMortgageShipDetail()
                    {
                        ShipNumber = "123456",
                        RegistrationNumber = "654321",
                        RegistrationDate = DateTime.Now.AddYears(-1)
                    };
                    break;
                case ReverseMortgageMoneyType.Trademark:
                    reverseMortgage.ReverseMortgageMoneyType = new Lookup() { Id = KeyConfig.Lookup.ReverseMortgageMoneyType.Trademarks };
                    reverseMortgage.ReverseMortgageMoneyTypeId = reverseMortgage.ReverseMortgageMoneyType.Id;
                    reverseMortgage.ReverseMortgageTrademarkDetail = new ReverseMortgageTrademarkDetail()
                    {
                        TrademarkNumber = "123456",
                        TrademarkType = "car",
                        TrademarkCategory = "brand",
                        TrademarkStatus = "registered",
                        TrademarkDescription = "nothing special"
                    };
                    break;
                case ReverseMortgageMoneyType.Cash:
                    reverseMortgage.ReverseMortgageMoneyType = new Lookup() { Id = KeyConfig.Lookup.ReverseMortgageMoneyType.Cash };
                    reverseMortgage.ReverseMortgageMoneyTypeId = reverseMortgage.ReverseMortgageMoneyType.Id;
                    break;
                default:
                    break;
            }

            return reverseMortgage;
        }

        [TestMethod]
        public void Create_ReverseMortgageMoneyTypeAirplane_CreatedSuccessfully()
        {
            var SUT = _reverseMortgageService.Create(InitMortgage(ReverseMortgageMoneyType.Airplane));

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.ReverseMortgageAirplaneDetail.Id); 
        }

        [TestMethod]
        public void Create_ReverseMortgageMoneyTypeCash_CreatedSuccessfully()
        {
            var SUT = _reverseMortgageService.Create(InitMortgage(ReverseMortgageMoneyType.Cash));

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
        }

        [TestMethod]
        public void Create_ReverseMortgageMoneyTypeShip_CreatedSuccessfully()
        {
            var SUT = _reverseMortgageService.Create(InitMortgage(ReverseMortgageMoneyType.Ship));

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.ReverseMortgageShipDetail.Id);
        }

        [TestMethod]
        public void Create_ReverseMortgageMoneyTypeTrademark_CreatedSuccessfully()
        {
            var SUT = _reverseMortgageService.Create(InitMortgage(ReverseMortgageMoneyType.Trademark));

            Assert.AreNotEqual(0, SUT.Id);
            Assert.AreNotEqual(0, SUT.Request.Id);
            Assert.AreNotEqual(0, SUT.ReverseMortgageTrademarkDetail.Id);
        }
    }
}
