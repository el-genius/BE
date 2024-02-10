using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Model;

namespace URCP.ServicesRepository.Interface
{
    public interface IPaymentRepository
    {
        InvoiceSummaryModel GetInvoiceCost(InvoiceModel invoiceModel);

        InvoiceSummaryModel GenerateInvoice(InvoiceModel invoiceModel);
    }
}
