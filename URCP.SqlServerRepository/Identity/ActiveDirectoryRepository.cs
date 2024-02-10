using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.RepositoryInterface;

namespace URCP.SqlServerRepository
{
    public class ActiveDirectoryRepository : IActiveDirectoryRepository
    {
        public bool AuthenticateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public ActiveDirectoryUserInfo GetUserInfo(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
