using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URCP.Domain.Interface
{
    public interface ICrService
    {
        Boolean IsCrManager(long crNumber, string identityNumber);

        Boolean IsCrOwner(long crNumber, string identityNumber);
    }
}
