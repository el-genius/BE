using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.SearchEntities;

namespace URCP.Domain.Interface
{
    public interface IMortgageService
    {
        void ManageMortgagePendingStatus();

        Task ManageMortgageAwaitPaymentStatusAsync();

        Task ManageMortgageContractDateAsync();

        void ManageMortgageAwaitGenerateInvoiceStatus();

        Mortgage Create(Mortgage entity, string eFileAccessToken);
        
        Task PayInvoice(int requestId, InvoiceStatus status);

        Mortgage Register(int mortgageId);

        Mortgage AssignToAgent(int mortgageId);

        Mortgage RejecteByObjection(int mortgageId);
        Mortgage RejecteByAgency(int mortgageId);

        IQueryResult<Mortgage> FindBy(MortgageSearchCriteria criteria);

        Mortgage Single(int id);

        Mortgage SingleOrDefault(int id);

        void ValidateCreateMortgageBusiness(Mortgage entity);

        Mortgage Cancel(int mortgageId, string reason);  
    }
}
