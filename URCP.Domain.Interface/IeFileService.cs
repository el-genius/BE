using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Enum;

namespace URCP.Domain.Interface
{
    public interface IeFileService
    {
        bool IsAuthenticated(string accessToken);

        string FindUserName(string accessToken);

        User FindCurrentUser(string accessToken);

        User FindUser(string accessToken, string userName);

        void Logout(string accessToken);

        bool IsDelegatedFor(string userName, string crNumber, eFileDelegateRole role);
    }
}
