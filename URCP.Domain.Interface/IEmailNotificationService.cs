using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;

namespace URCP.Domain.Interface
{
    public interface IEmailNotificationService
    {
        EmailNotification Create(EmailNotification entity);

        EmailNotification Update(EmailNotification entity);
    }
}
