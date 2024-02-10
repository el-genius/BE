using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Basis;
using URCP.GraphDiff.Attributes;

namespace URCP.Core
{ 
    public class EmailAttachment : AuditableEntity
    {
        private MemoryStream stream;

        [Key]
        public long Id { get; set; }
        public Stream Data { get { return stream; } }
        public string Filename { get; private set; }
        public string MediaType { get; private set; }
        public Attachment File { get { return new Attachment(Data, Filename, MediaType); } }

        [ForeignKey("EmailNotification")]
        public long NotificationId { get; set; }

        [Associated]
        public EmailNotification EmailNotification { get; set; }

        public EmailAttachment(byte[] data, string filename)
        {
            this.stream = new MemoryStream(data);
            this.Filename = filename;
            this.MediaType = MediaTypeNames.Application.Octet;
        }
        public EmailAttachment(string data, string filename)
        {
            this.stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(data));
            this.Filename = filename;
            this.MediaType = MediaTypeNames.Text.Html;
        }
    }
}
