using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;

namespace URCP.Core
{
    public enum NotificationStatus
    {
        Pending
    }

    public class SmsNotification : AuditableEntity
    {
        [Key]
        public long Id { get; set; }

        public string Content { get; set; }

        private IList<string> _to = new List<string>();

        public virtual IList<string> To
        {
            get { return _to; }
            set { _to = value; }
        }

        [NotMapped]
        public string MobileNo { get; set; }

        [NotMapped]
        public bool IsArabic { get; set; }

        public string ToSms
        {
            get { return string.Join("|", _to); }
            set { _to = value.Split('|'); }
        }

        public bool IsEnabled { get; set; }

        public bool IsSent { get; set; }

        public short RetryTimes { get; set; }


        public DateTime? SentDateTime { get; set; }

        public NotificationStatus Status { get; set; }

        public IList<string> TemplateValuesList { get; set; } = new List<string>();

        public string TemplateValues
        {
            get
            {
                return string.Join("|", TemplateValuesList);
            }
            set
            {
                TemplateValuesList = value.Split('|');
            }
        }

        public string TemplateCode { get; set; }

        public string ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; } 

    }
}
