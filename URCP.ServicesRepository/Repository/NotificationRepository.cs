using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Util;
using URCP.ServicesRepository.Helper;
using URCP.ServicesRepository.Interface;
using URCP.ServicesRepository.NotificationGateway;

namespace URCP.ServicesRepository
{
    public class NotificationRepository : INotificationRepository
    {
        public int SendSms(SmsNotification smsNotification)
        {
            using (NotificationGatewayClient client = new NotificationGatewayClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.NotificationService.Username;
                client.ClientCredentials.UserName.Password = KeyConfig.NotificationService.Password;

                SendSMSNotificationRequest request = smsNotification.ToEntity();

                NotificationResponse response = client.SendSMS(request);

                return response.RequestId;
            }
        }

        public async Task<int> SendSmsAsync(SmsNotification smsNotification)
        {
            using (NotificationGatewayClient client = new NotificationGatewayClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.NotificationService.Username;
                client.ClientCredentials.UserName.Password = KeyConfig.NotificationService.Password;

                SendSMSNotificationRequest request = smsNotification.ToEntity();

                NotificationResponse response = await client.SendSMSAsync(request);

                return response.RequestId;
            }
        }

        public int SendEmail(EmailNotification emailNotification)
        {
            using (NotificationGatewayClient client = new NotificationGatewayClient())
            {
                client.ClientCredentials.UserName.UserName = KeyConfig.NotificationService.Username;
                client.ClientCredentials.UserName.Password = KeyConfig.NotificationService.Password;

                SendEmailRequest request = emailNotification.ToEntity();

                NotificationResponse response = client.SendEmail(request);

                return response.RequestId;
            }
        }
    }
}
