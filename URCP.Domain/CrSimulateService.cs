using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Domain.Interface;
using URCP.ServicesRepository.Interface;

namespace URCP.Domain
{
    public class CrSimulateService : ICrService
    {
        private readonly ICrRepository _crRepository;

        public CrSimulateService(ICrRepository crRepository)
        {
            this._crRepository = crRepository;
        }

        public Boolean IsCrManager(long crNumber, string identityNumber)
        {
            if (crNumber == 0)
                throw new ArgumentNullException("crNumber", "crNumber can not be null");

            try
            {
                //return _crRepository.IsCrManager(crNumber, identityNumber);
                return true;
            }
            catch (FaultException e)
            {
                var eai = (e as FaultException<ServicesRepository.CrService.EAICustomError>);
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
                        exceptionMessage = eai.Detail.BusinessMessageAr;
                        throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceBusinessException);
                    case "EAI-401":
                    case "EAI-403":
                    case "EAI-500":
                    default:
                        exceptionMessage = "خطأ في خدمة السجل التجاري ... الرجاء المحالولة لاحقاً";
                        break;
                }

                throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceGenericException);
            }
        }

        public Boolean IsCrOwner(long crNumber, string identityNumber)
        {
            if (crNumber == 0)
                throw new ArgumentNullException("crNumber", "crNumber can not be null");

            try
            {
                //return _crRepository.IsCrOwner(crNumber, identityNumber);
                return true;
            }
            catch (FaultException e)
            {
                var eai = (e as FaultException<ServicesRepository.CrService.EAICustomError>);
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
                        exceptionMessage = eai.Detail.BusinessMessageAr;
                        throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceBusinessException);
                    case "EAI-401":
                    case "EAI-403":
                    case "EAI-500":
                    default:
                        exceptionMessage = "خطأ في خدمة السجل التجاري ... الرجاء المحالولة لاحقاً";
                        break;
                }

                throw new WebServiceException(exceptionMessage, new Exception($"{eai.Detail.BusinessMessageEn} - {eai.Detail.Code}", e), ErrorCode.WebServiceGenericException);
            }
        }
    }
}
