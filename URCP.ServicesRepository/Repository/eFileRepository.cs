using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Enum;
using URCP.Core.Util;
using URCP.ServicesRepository.eFileService;
using URCP.ServicesRepository.Interface;

namespace URCP.ServicesRepository
{
    public class eFileRepository : IeFileRepository
    {
        public bool IsAuthenticated(string accessToken)
        {
            using (EFileServiceClient client = new EFileServiceClient())
            {
                return client.IsAuthenticated(accessToken, KeyConfig.eFileService.ClientId);
            }
        }

        public string FindUserName(string accessToken)
        {
            using (EFileServiceClient client = new EFileServiceClient())
            {
                return client.GetUsernameByAccessToken(accessToken);
            }
        }

        public User FindPerson(string accessToken, string userName)
        {
            using (EFileServiceClient client = new EFileServiceClient())
            { 
                var eServiceFilePerson = client.GetPersonByNID(userName, KeyConfig.eFileService.ClientId, KeyConfig.eFileService.ClientSecret);

                var eFilePersonFullName = $"{ eServiceFilePerson?.FirstName } { eServiceFilePerson?.SecondName} { eServiceFilePerson?.LastName}";
                var eFilePersonBirthday = eServiceFilePerson.BirthDate.HasValue == false ? DateTime.Now.AddYears(-20) : eServiceFilePerson.BirthDate.Value;


                return new User(userName, eFilePersonFullName, eFilePersonFullName, eServiceFilePerson.ContactMobile, eServiceFilePerson.ContactEmail, eFilePersonBirthday, UserType.eFileUser)
                {
                    IdentityType = userName.StartsWith("2") ? IdentityType.Iqama : IdentityType.NatioanlId,
                    IdentityNumber = userName
                };
            }
        }

        public void Logout(string accessToken)
        {
            using (EFileServiceClient client = new EFileServiceClient())
            {
                client.Logout(accessToken);
            }
        }

        public bool IsDelegatedFor(string userName, string crNumber, eFileDelegateRole role)
        {
            List<EFileServiceDelegationDetails> result = null;
            using (EFileServiceClient client = new EFileServiceClient())
            {
                result = client.GetAllBeneficiariesByCrNumber(crNumber, KeyConfig.eFileService.ClientId, KeyConfig.eFileService.ClientSecret);

                if(result != null && result.Count > 0)
                {
                    switch (role)
                    {
                        case eFileDelegateRole.CreateMortgage:
                            return result.Where(r => r.BeneficiaryNID.Trim().Equals(userName.Trim()) &&
                          r.DelegatedReferences.Where(d => d.RoleID == KeyConfig.eFileService.CreateMortgageRole).Count() > 0).Count() > 0;
                        case eFileDelegateRole.CreateInquiry:
                            return result.Where(r => r.BeneficiaryNID.Trim().Equals(userName.Trim()) &&
                       r.DelegatedReferences.Where(d => d.RoleID == KeyConfig.eFileService.CreateMortgageInquiryRole).Count() > 0).Count() > 0;
                        default:
                            return false;
                    }
                      
                }
            }

            return result.Count() == 0 ? false : true;
        }

    }
}
