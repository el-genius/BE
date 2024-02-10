using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URCP.ServicesRepository
{
    public class ServiceCredentials
    {
        #region Notification Service
        public static string NotificationServiceUsername
        {
            get
            {
                var result = ConfigurationManager.AppSettings["NotificationService-Username"];

                if (String.IsNullOrEmpty(result))
                    throw new InvalidOperationException("[NotificationService-Username] appSettings key is not defined or has no value.");

                return result;
            }
        }

        public static string NotificationServicePassword
        {
            get
            {
                var result = ConfigurationManager.AppSettings["NotificationService-Password"];

                if (String.IsNullOrEmpty(result))
                    throw new InvalidOperationException("[NotificationService-Password] appSettings key is not defined or has no value.");

                return result;
            }
        }
        #endregion
    }
}
