using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.SearchEntities;
using URCP.RepositoryInterface;

namespace URCP.Domain.Interface
{
    public interface IRequestService
    {
        Request Create(Request entity);

        Request Update(Request entity);

        IQueryResult<Request> FindBy(RequestSearchCriteria criteria);
    }
}
