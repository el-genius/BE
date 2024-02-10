using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Domain.Interface;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using URCP.Resources;
using URCP.ServicesRepository.Interface;

namespace URCP.Domain
{
    public class EmailNotificationService : BaseService, IEmailNotificationService
    {
        private readonly IGenericRepository _repository;
        private readonly IGenericQueryRepository _queryRepository;
        private readonly INotificationRepository _notificationRepository;

        public EmailNotificationService(
            IUnitOfWork unitOfWork,
            IGenericRepository repository,
            IGenericQueryRepository queryRepository,
            INotificationRepository notificationRepository)
            : base(unitOfWork)
        {
            this._repository = repository;
            this._queryRepository = queryRepository;
            this._notificationRepository = notificationRepository;
        }

        public EmailNotification Create(EmailNotification entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", String.Format(DomainMessages.Entity_Cannot_Be_Null, "EmailNotification"));

            if (!entity.Validate())
                throw new ValidationException(String.Format(DomainMessages.Validation_Error, "EmailNotification"), entity.ValidationResults);

            entity = _repository.Create<EmailNotification>(entity);
            Commit();

            try
            {
                SendEmail(entity);
            }
            catch (FaultException e)
            {
                var eai = (e as FaultException<ServicesRepository.PaymentService.EAICustomError>);
                var exceptionCode = eai.Detail.Code;
                string exceptionMessage = String.Empty;

                switch (exceptionCode)
                {
                    case "EAI-404":
                    case "EAI-901":
                    case "EAI-902":
                    case "EAI-903":
                    case "EAI-904":
                    case "EAI-905":
                    case "EAI-906":
                    case "EAI-907":
                    case "EAI-908":
                    case "EAI-909":
                        exceptionMessage = eai.Detail.BusinessMessageAr;
                        entity.ExceptionMessage = eai.Detail.BusinessMessageEn;
                        entity.ExceptionCode = exceptionCode;
                        Update(entity);

                        throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceBusinessException);
                    case "EAI-401":
                    case "EAI-403":
                    case "EAI-500":
                    case "EAI-910":
                    default:
                        exceptionMessage = "خطأ في خدمة الاشعارات ... الرجاء المحالولة لاحقاً";
                        break;
                }

                entity.ExceptionMessage = eai.Detail.BusinessMessageEn;
                entity.ExceptionCode = exceptionCode;
                Update(entity);

                throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceGenericException);
            }
            catch (Exception e)
            {
                entity.ExceptionMessage = e.Message;
                Update(entity);

                var exceptionMessage = "خطأ في خدمة الاشعارت ... الرجاء المحالولة لاحقاً";
                throw new WebServiceException(exceptionMessage, e, ErrorCode.WebServiceGenericException);
            }

            entity.IsSent = true;
            Update(entity);

            return entity;
        }


        public EmailNotification Update(EmailNotification entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity", String.Format(DomainMessages.Entity_Cannot_Be_Null, "EmailNotification"));

            if (entity.Id == 0)
                throw new ArgumentNullException("Entity.Id == 0", String.Format(DomainMessages.Entity_Cannot_Be_Null, "EmailNotification"));

            if (!entity.Validate())
                throw new ValidationException(String.Format(DomainMessages.Validation_Error, "EmailNotification"), entity.ValidationResults);

            // TODO: Check entity state
            _repository.SetDetachedState(entity);
            entity.SetUpdate();
            entity = _repository.Update<EmailNotification>(entity);
            Commit();

            return entity; 
        }

        #region Private Methods
        private void SendEmail(EmailNotification emailNotification)
        {
            emailNotification.IsArabic = true;
            var result = _notificationRepository.SendEmail(emailNotification);
        }
        #endregion

    }
}
