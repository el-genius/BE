using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.SearchEntities;

namespace URCP.Domain.Interface
{
    public interface IInvoiceService
    {
        Invoice Single(int id);

        IQueryResult<Invoice> FindBy(InvoiceSearchCriteria criteria);

        Invoice Create(Invoice entity);

        Invoice Update(Invoice entity);

        //void PayInvoice(int invoiceId, InvoiceStatus status);
    }
}
