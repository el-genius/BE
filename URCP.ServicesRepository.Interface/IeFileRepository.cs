using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Enum;

namespace URCP.ServicesRepository.Interface
{
    public interface IeFileRepository
    {
        bool IsAuthenticated(string accessToken);

        string FindUserName(string accessToken);

        User FindPerson(string accessToken, string identityNumber);

        void Logout(string accessToken);

        bool IsDelegatedFor(string userName, string crNumber, eFileDelegateRole role);
    }
}
