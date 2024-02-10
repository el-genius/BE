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
    public class EmailNotification : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public string Content { get; set; }
        private IList<string> _to;
        public virtual IList<string> To
        {
            get { return _to; }
            set { _to = value; }
        }
        public string ToEmails
        {
            get { return string.Join("|", _to); }
            set { _to = value.Split('|'); }
        }
        public bool IsEnabled { get; set; }
        public bool IsSent { get; set; }
        public short RetryTimes { get; set; }
        public DateTime? SentDateTime { get; set; }
        public NotificationStatus Status { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }

        [NotMapped]
        public bool IsArabic { get; set; }

        public virtual IList<EmailAttachment> AttachmentFiles { get; set; }

        public IList<string> TemplateValuesList { get; set; }
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

        private IList<string> _cc, _bcc;
        public IList<string> CCsList
        {
            get { return _cc; }
            set { _cc = value; }
        }
        public IList<string> BCCsList
        {
            get { return _bcc; }
            set { _bcc = value; }
        }
        public string CCs
        {
            get { return string.Join("|", _cc); }
            set { _cc = value.Split('|'); }
        }
        public string BCCs
        {

            get { return string.Join("|", _bcc); }
            set { _bcc = value.Split('|'); }
        }

        public string ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }

        public EmailNotification()
        {
            To = new List<string>();
            CCsList = new List<string>();
            BCCsList = new List<string>();
            TemplateValuesList = new List<string>();
            CreatedAt = DateTime.Now;
            IsEnabled = true;
            IsSent = false;
            IsBodyHtml = true;
            Status = NotificationStatus.Pending;

        }
    }
}
