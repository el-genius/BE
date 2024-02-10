using System;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.ServiceModel;
using URCP.Core.Util;

namespace URCP.WebSvc
{
    public class BillingAuthenticationValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            try
            {
                var configUser = KeyConfig.BillingSystemAuthorizedUserName;
                var configPassword = KeyConfig.BillingSystemAuthorizedPassword;
                if (userName.ToLower() != configUser.ToLower() || password != configPassword)
                    throw new FaultException("Login Information was incorrect please check.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Login Information was incorrect please check.");
            }
        }
    }
}