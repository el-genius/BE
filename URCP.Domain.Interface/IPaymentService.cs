using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Entities;
using URCP.Core.Model;

namespace URCP.Domain.Interface
{
    public interface IPaymentService
    {
        InvoiceSummaryModel GetInvoiceCost(Request request);

        InvoiceSummaryModel GenerateInvoice(Request entity, int invoiceId);
    }
}
