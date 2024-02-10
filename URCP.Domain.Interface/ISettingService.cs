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
    public interface ISettingService
    {
        Setting Single(int id);

        IQueryResult<Setting> FindBy(SettingSearchCriteria criteria);
    }
}
