using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.SearchEntities;

namespace URCP.Domain.Interface
{
    public interface IMortgageInquiryService
    {
        MortgageInquiry Single(int id);

        MortgageInquiry Create(MortgageInquiry entity);

        MortgageInquiry Update(MortgageInquiry entity);

        IQueryResult<Mortgage> ValidateMortgageExistence(MortgageInquiry entity);

        IQueryResult<MortgageInquiry> FindBy(MortgageInquirySearchCriteria criteria);

        MortgageInquiry Accept(int mortgageInquiryId);

        MortgageInquiry Reject(int mortgageInquiryId);

        Task PayInvoice(int requestId, InvoiceStatus status);

        Task ManageInquiryAwaitPaymentStatusAsync();
    }
}
