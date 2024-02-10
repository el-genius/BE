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
    public interface ICountryService
    {
        Country Single(int id);
        IQueryResult<Country> FindBy(CountrySearchCriteria criteria);

    }
}
