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
    public class Notification : AuditableEntity
    {
        #region Prop
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "Title must be 100-3", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "EmailTemplateCode must be 100-5", MinimumLength = 5)]
        public string EmailTemplateCode { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "SMSTemplateCode must be 100-5", MinimumLength = 5)]
        public string SMSTemplateCode { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "EmailTemplateValues must be 100-5")]
        public string EmailTemplateValues { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "SMSTemplateValues must be 100-5")]
        public string SMSTemplateValues { get; set; }

        [Required(ErrorMessage = "Body is required")]
        [StringLength(maximumLength: 200, ErrorMessage = "Body must be 200-3", MinimumLength = 3)]
        public string Body { get; set; }

        [Required(ErrorMessage = "EmailSent is required")]
        public bool EmailSent { get; set; }

        [Required(ErrorMessage = "SmsSent is required")]
        public bool SmsSent { get; set; }

        [Required(ErrorMessage = "Read is required")]
        public bool Read { get; set; }

        [Required(ErrorMessage = "Deleted is required")]
        public bool Deleted { get; set; }

        [ForeignKey("User")]
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        //public DateTime? SendAt { get; set; }
        #endregion

        #region Navigation

        public User User { get; set; }

        #endregion

        #region Ctor

        public Notification()
        {
            base.SetCreate();
            this.EmailSent = false;
            this.SmsSent = false;
        }

        #endregion
    }
}
