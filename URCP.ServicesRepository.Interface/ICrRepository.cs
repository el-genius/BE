using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URCP.ServicesRepository.Interface
{
    public interface ICrRepository
    {
        Boolean IsCrManager(long crNumber, string identityNumber);

        Boolean IsCrOwner(long crNumber, string identityNumber);
    }
}
