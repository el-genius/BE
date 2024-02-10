using URCP.Core;
using URCP.Core.SearchEntities;
using URCP.Core.Security;

namespace URCP.Domain.Interface
{
    public interface IUserMorgageMoneyTypeService
    {
        IQueryResult<UserMorgageMoneyType> FindBy(UserMorgageMoneyTypeSearchCriteria criteria);
    }
}
