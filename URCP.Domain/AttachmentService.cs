using System;
using System.ServiceModel;
using URCP.Core;
using URCP.Core.Model;
using URCP.Domain.Interface;
using URCP.Resources;
using URCP.ServicesRepository.Interface;

namespace URCP.Domain
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            this._attachmentRepository = attachmentRepository;
        }

        public AttachmentModel Download(Guid fileReference)
        {
            try
            {
                return _attachmentRepository.Download(fileReference);
            }
            catch (FaultException e)
            {
                throw new WebServiceException("خطأ في خدمة رفع الملفات ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }

        public Guid Upload(AttachmentModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", String.Format(DomainMessages.Entity_Cannot_Be_Null, nameof(AttachmentModel)));

            if (!entity.Validate())
                throw new ValidationException(String.Format(DomainMessages.Validation_Error, nameof(AttachmentModel)), entity.ValidationResults);

            try
            {
                return _attachmentRepository.Upload(entity);
            }
            catch (FaultException e)
            {
                throw new WebServiceException("خطأ في خدمة رفع الملفات ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }


    }
}
