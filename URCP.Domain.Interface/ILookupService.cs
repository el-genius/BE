using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Lookups;
using URCP.Core.SearchEntities;

namespace URCP.Domain.Interface
{
    public interface ILookupService
    {
        Lookup Single(int id);
        IQueryResult<Lookup> FindBy(int lookupTypeId);
        IQueryResult<Lookup> FindBy(LookupSearchCriteria criteria);
    }
}
