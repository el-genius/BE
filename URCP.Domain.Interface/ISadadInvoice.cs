using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Entities;

namespace URCP.Domain.Interface
{
    public interface ISadadInvoice
    {
        Task PayInvoice(int requestId, InvoiceStatus status);
    }
}
