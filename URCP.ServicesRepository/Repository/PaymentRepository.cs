using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Model;
using URCP.Core.Util;
using URCP.ServicesRepository.Helper;
using URCP.ServicesRepository.Interface;
using URCP.ServicesRepository.PaymentService;

namespace URCP.ServicesRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        public InvoiceSummaryModel GetInvoiceCost(InvoiceModel invoiceModel)
        {
            using (BillingServiceClient client = new BillingServiceClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.PaymentService.Username;
                client.ClientCredentials.UserName.Password = KeyConfig.PaymentService.Password;

                GenerateBillSadadRequest billCoastObject = invoiceModel.ToEntity();

                CheckBillCostResponse response = client.GetBillCost(billCoastObject);
                var invoiceSummaryModel = new InvoiceSummaryModel(response.Cost, response.VATCost, response.TotalCost);
                response.BillDetails.ForEach(i => invoiceSummaryModel.AddInvoiceDetail(int.Parse(i.RefId), i.Cost));

                return invoiceSummaryModel;
            }
        }

        public InvoiceSummaryModel GenerateInvoice(InvoiceModel invoiceModel)
        {
            using (BillingServiceClient client = new BillingServiceClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.PaymentService.Username;
                client.ClientCredentials.UserName.Password = KeyConfig.PaymentService.Password;

                GenerateBillSadadRequest billCoastObject = invoiceModel.ToEntity();

                GenerateBillSadadResponse response = client.GenerateSadadBillThiqah(billCoastObject);
                var invoiceSummaryModel = new InvoiceSummaryModel(response.Cost, response.VATCost, response.TotalCost);
                invoiceSummaryModel.Update(response.BillId, response.ReferenceNumber, response.SadadNumber, DateTime.Now, billCoastObject.ExpireDate);

                GetBillDetailResponse getResponse = client.GetBillDetail(invoiceModel.RefId, invoiceModel.RefDecription);
                getResponse.PricingDetails.ForEach(i => invoiceSummaryModel.AddInvoiceDetail(int.Parse(i.RefId), i.Cost));

                return invoiceSummaryModel;
            }
        }
    }
}
