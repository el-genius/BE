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
    public interface IReverseMortgageTrademarkDetailService
    {
        IQueryResult<ReverseMortgageTrademarkDetail> FindBy(ReverseMortgageTrademarkDetailSearchCriteria criteria);
    }
}
