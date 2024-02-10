using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;

namespace URCP.ServicesRepository.Interface
{
    public interface INotificationRepository
    {
        int SendSms(SmsNotification smsNotification);

        Task<int> SendSmsAsync(SmsNotification smsNotification);

        int SendEmail(EmailNotification emailNotification);
    }
}
