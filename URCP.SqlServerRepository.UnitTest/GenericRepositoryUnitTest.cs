using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Unity;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.SearchEntities;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;

namespace URCP.SqlServerRepository.UnitTest
{
    [TestClass]
    public class GenericRepositoryUnitTest
    {
        IUnityContainer Container;

        [TestInitialize]
        public void Setup()
        {
            var request = new Request(RequestType.RegisterMortgage, RequestStatus.Paid);

            var mortgages = new QueryResult<Mortgage>(new List<Mortgage>()
            {
                new Mortgage(MortgageStatus.Pending).AttachRequest(new Request(RequestType.RegisterMortgage, RequestStatus.Paid))
            }, 2, 1, 1);

            var repoMock = new Mock<IGenericQueryRepository>();
            repoMock.Setup(r => r.Find(It.IsAny<QueryConstraints<Mortgage>>())).Returns(mortgages);

            Container = new UnityContainer();
            Container.RegisterInstance<IGenericQueryRepository>(repoMock.Object);
        }

        [TestMethod]
        public void Find()
        {
            ////var repo = Container.Resolve<MortgageService>();
            //var SUT = repo.Find(new QueryConstraints<Mortgage>());

            //Assert.AreNotEqual(0, SUT.TotalCount);
        }
    }
}
