using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Model;

namespace URCP.Domain.Interface
{
    public interface IAttachmentService
    {
        Guid Upload(AttachmentModel attachmentModel);

        AttachmentModel Download(Guid fileReference);
    }
}
