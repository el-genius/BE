using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Enum;
using URCP.Core.Util;
using URCP.Domain.Interface;
using URCP.ServicesRepository.Interface;

namespace URCP.Domain
{
    public class eFileService : IeFileService
    {
        private readonly IeFileRepository _eFileRepository;
        private readonly IRoleService _roleService;

        public eFileService(IeFileRepository eFileRepository, IRoleService roleService)
        {
            this._eFileRepository = eFileRepository;
            this._roleService = roleService;
        }

        public User FindCurrentUser(string accessToken)
        {
            try
            {
                var currentUserNumber = _eFileRepository.FindUserName(accessToken);
                var user = _eFileRepository.FindPerson(accessToken, currentUserNumber);

                var eFileUserRoles = _roleService.FindByIds(KeyConfig.eFileService.UserRoles.Split(',').ToList());
                user.UpdateRoles(eFileUserRoles.Items.ToList());

                return user;
            }
            catch (FaultException e)
            {
                if (e.Code.Name == "105")
                    throw new WebServiceException("رقم هوية الراهن غير مسجلة في نظام التسجيل الموحد", e, ErrorCode.WebServiceBusinessException);

                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }

        public User FindUser(string accessToken, string userName)
        {
            User user = null;
            try
            {
                user = _eFileRepository.FindPerson(accessToken, userName);
            }
            catch (FaultException e)
            {
                if (e.Code.Name == "105")
                    throw new WebServiceException("رقم هوية الراهن غير مسجلة في نظام التسجيل الموحد", e, ErrorCode.WebServiceBusinessException);
                
                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }

            var eFileUserRoles = _roleService.FindByIds(KeyConfig.eFileService.UserRoles.Split(',').ToList());
            user.UpdateRoles(eFileUserRoles.Items.ToList());

            return user;
        }

        public string FindUserName(string accessToken)
        {
            try
            {
                return _eFileRepository.FindUserName(accessToken);
            }
            catch (FaultException e)
            {
                if (e.Code.Name == "105")
                    throw new WebServiceException("رقم هوية الراهن غير مسجلة في نظام التسجيل الموحد", e, ErrorCode.WebServiceBusinessException);

                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }

        public bool IsDelegatedFor(string userName, string crNumber, eFileDelegateRole role)
        {
            try
            {
                return _eFileRepository.IsDelegatedFor(userName, crNumber, role);
            }
            catch (FaultException e)
            {
                if (e.Code.Name == "105")
                    throw new WebServiceException("رقم هوية الراهن غير مسجلة في نظام التسجيل الموحد", e, ErrorCode.WebServiceBusinessException);

                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }

        public bool IsAuthenticated(string accessToken)
        {
            try
            {
                return _eFileRepository.IsAuthenticated(accessToken);
            }
            catch (FaultException e)
            {
                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }

        public void Logout(string accessToken)
        {
            try
            {
                _eFileRepository.Logout(accessToken);
            }
            catch (FaultException e)
            {
                throw new WebServiceException("خطأ في خدمة النظام الموحد ... الرجاء المحالولة لاحقاً", e, ErrorCode.WebServiceGenericException);
            }
        }
    }
}
