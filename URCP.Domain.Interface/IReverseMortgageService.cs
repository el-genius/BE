using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.SearchEntities;

namespace URCP.Domain.Interface
{
    public interface IReverseMortgageService
    {
        ReverseMortgage Create(ReverseMortgage entity);

        IQueryResult<ReverseMortgage> FindBy(ReverseMortgageSearchCriteria criteria);
    }
}
