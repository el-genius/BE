using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Model;

namespace URCP.ServicesRepository.Interface
{
    public interface IAttachmentRepository
    {
        Guid Upload(AttachmentModel attachmentModel);

        AttachmentModel Download(Guid reference);
    }
}
