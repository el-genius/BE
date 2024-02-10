using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Util;
using URCP.ServicesRepository.CrService;
using URCP.ServicesRepository.Interface;

namespace URCP.ServicesRepository
{
    public class CrRepository : ICrRepository
    {

        public Boolean IsCrManager(long crNumber, string identityNumber)
        {
            using (CRServiceClient client = new CRServiceClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.CrService.UserName;
                client.ClientCredentials.UserName.Password = KeyConfig.CrService.Password;

                var crManagersRelationsTypes = KeyConfig.CrService.CrManagerRelationsTypes;
                var crManagersRelationsTypesIds = crManagersRelationsTypes.Split(',').ToList();

                var cRsByPartyIDRequest = new CRsByPartyIDRequest()
                {
                    PartyID = identityNumber,
                    PartyRelationIDs = crManagersRelationsTypesIds,
                    CRStatus = new List<string>() { "ACTIVE" }
                };

                var result = client.GetCrListByPartyID(cRsByPartyIDRequest);

                var isCrManager = result.Where(r =>
                    r.CRNumber.ToString().Trim().Equals(crNumber.ToString().Trim()) &&
                    r.BusinessType.Equals("205") == true &&
                    r.PartyRelTypeID.Equals("10")).Count() != 0;

                if (!isCrManager)
                {
                    isCrManager = result.Where(r =>
                        r.CRNumber.ToString().Trim().Equals(crNumber.ToString().Trim()) &&
                        r.BusinessType.Equals("205") == false &&
                        r.PartyRelTypeID.Equals("20")).Count() != 0;
                }

                return isCrManager;
            }
        }

        public Boolean IsCrOwner(long crNumber, string identityNumber)
        {
            using (CRServiceClient client = new CRServiceClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.CrService.UserName;
                client.ClientCredentials.UserName.Password = KeyConfig.CrService.Password;

                var crOwnerRelationsTypes = KeyConfig.CrService.CrOwnerRelationsTypes;
                var crOwnerRelationsTypesIds = crOwnerRelationsTypes.Split(',').ToList();

                var cRsByPartyIDRequest = new CRsByPartyIDRequest()
                {
                    PartyID = identityNumber,
                    CRBusinessTypesIDs = new List<string>() { "101" },
                    PartyRelationIDs = crOwnerRelationsTypesIds,
                    CRStatus = new List<string>() { "ACTIVE" }
                };

                var result = client.GetCrListByPartyID(cRsByPartyIDRequest);

                return result.Where(r => r.CRNumber.Trim().Equals(crNumber.ToString().Trim())).ToList().Count == 0 ? false : true;
            }
        }
    }
}
