using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace URCP.Core.Basis
{
    public abstract class AuditableEntity : BaseEntity
    {
        public int? CreatedByID { get; set; }

        public int? UpdatedByID { get; set; }

        /// <summary>
        /// System creation date time of this entity, set by the system.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// The user who create the entity, set by the system.
        /// </summary>
        public User CreatedBy { get; protected set; }

        /// <summary>
        /// System update date time of this entity, set by the system.
        /// </summary>
        public DateTime? UpdatedAt { get; protected set; }

        /// <summary>
        /// The user who update the entity, set by the system.
        /// </summary>
        public User UpdatedBy { get; protected set; }

        public AuditableEntity()
        {
            this.SetCreate();
        }
        /// <summary>
        /// Set UpdatedAt and UpdatedBy
        /// </summary>
        /// <returns></returns>
        public AuditableEntity SetUpdate()
        {
            this.UpdatedAt = DateTime.Now;
            //this.UpdatedBy = Thread.CurrentPrincipal.Identity as User;
            this.UpdatedByID = (Thread.CurrentPrincipal.Identity as User)?.Id;

            return this;
        }

        /// <summary>
        /// Set CreatedAt and CreatedBy
        /// </summary>
        /// <returns></returns>
        public AuditableEntity SetCreate()
        {
            this.CreatedAt = DateTime.Now;
            //this.CreatedBy = Thread.CurrentPrincipal.Identity as User;
            this.CreatedByID = (Thread.CurrentPrincipal.Identity as User)?.Id;

            return this;
        }
    }
}
