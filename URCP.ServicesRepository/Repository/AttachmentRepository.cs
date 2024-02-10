using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Model;
using URCP.Core.Util;
using URCP.ServicesRepository.AttachmentService;
using URCP.ServicesRepository.Helper;
using URCP.ServicesRepository.Interface;

namespace URCP.ServicesRepository
{
    public class AttachmentRepository : IAttachmentRepository
    {
        public AttachmentModel Download(Guid fileReference)
        {
            using (AttachmentServiceClient client = new AttachmentServiceClient())
            {
                var request = new DownloadRequest()
                {
                    ApplicationId = KeyConfig.AttachmentService.ApplicationId,
                    FileId = fileReference
                };

                DownloadResponse downloadResponse = client.Download(request);

                return downloadResponse.ToModel();
            }
        }

        public Guid Upload(AttachmentModel attachmentModel)
        {
            using (AttachmentServiceClient client = new AttachmentServiceClient())
            {
                UploadRequest uploadRequest = attachmentModel.ToEntity();
                var response = client.Upload(uploadRequest);

                return response.FileId;
            }
        }

        
    }
}
