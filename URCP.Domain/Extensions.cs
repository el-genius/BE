using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.Model;
using URCP.Core.Util;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using URCP.SqlServerRepository;

namespace URCP.Domain
{
    public static class Extensions
    {
        private static IUnityContainer _container;
        private static IGenericQueryRepository _queryRepository;

        public static void Init(IUnityContainer container)
        {
            _container = container;
            _container.RegisterType<IGenericQueryRepository, GenericQueryRepository>();
            _queryRepository = _container.Resolve<GenericQueryRepository>();
        }

        static Extensions()
        {

        }

        public static InvoiceModel ToInvoiceModel(this Request request, int invoiceId)
        {
            InvoiceModel invoiceModel = new InvoiceModel()
            {
                RefId = invoiceId.ToString(),
                RefDecription = "RequestId",
                ClientMobileNo = (Thread.CurrentPrincipal.Identity as User).Mobile,
                invoices = new List<InvoiceMetadataModel>()
                {
                    new InvoiceMetadataModel()
                    {
                        RefId = invoiceId.ToString(),
                        RefDecription = "InvoiceId",
                        Description = "Invoice",
                        CityName = "CityName",
                        ContractType = "ContractType",
                        NationalId = 1,
                        RequestNo = request.Id
                    }
                }
            }.Update(KeyConfig.PaymentService.InvoiceDueDate, "Invoice for Register Mortgage");


            switch (request.Type)
            {
                case RequestType.RegisterMortgage:
                    invoiceModel.invoices.FirstOrDefault().InvoiceType = InvoiceType.RegisterMortgageContract;
                    break;
                case RequestType.MortgageInquiry:
                    invoiceModel.invoices.FirstOrDefault().InvoiceType = InvoiceType.MortgageInquiryByContractNumber;
                    break;
                default:
                    break;
            }

            return invoiceModel;
        }

        #region User
        public static Boolean IsUserNameUnique(this User user, bool isUpdatedCheck)
        {
            var constraints = new QueryConstraints<User>()
                                 .Where(x => x.UserName == user.UserName);


            if (isUpdatedCheck)
                constraints = constraints.AndAlso(x => x.Id != user.Id);

            return _queryRepository.SingleOrDefault(constraints) == null;
        }

        public static Boolean IsMobileUnique(this User user, Boolean isUpdateCheck)
        {
            var constraints = new QueryConstraints<User>()
                                 .Where(x => x.Mobile == user.Mobile);


            if (isUpdateCheck)
                constraints = constraints.AndAlso(x => x.Id != user.Id);

            return _queryRepository.SingleOrDefault(constraints) == null;
        }

        public static Boolean IsEmailUnique(this User user, Boolean isUpdateCheck)
        {
            var constraints = new QueryConstraints<User>()
                                 .Where(x => x.Email == user.Email);


            if (isUpdateCheck)
                constraints = constraints.AndAlso(x => x.Id != user.Id);

            return _queryRepository.SingleOrDefault(constraints) == null;
        }

        public static void FormatUserMobileToFullSaudiMobileNumber(this User user)
        {
            var fullSaudiMobileNumberPattern = @"^(\009665)([0-9]{8})$";

            if (user != null && !string.IsNullOrEmpty(user.Mobile) && !Regex.IsMatch(user.Mobile, fullSaudiMobileNumberPattern))
            {
                string pattern = "00966";
                if (user.Mobile.StartsWith("966"))
                    pattern = "966";
                else if (user.Mobile.StartsWith("+966"))
                    pattern = "+966";
                else if (user.Mobile.StartsWith("05"))
                    pattern = "0";
                else if (user.Mobile.StartsWith("5"))
                    pattern = "";

                Regex myRegex = new Regex(pattern);
                user.Mobile = myRegex.Replace(user.Mobile, "00966", 1);
            }
        }
        #endregion 
    }
}
