using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;

namespace URCP.WinSvc.Security
{
    public class UserProfileIdentity : User, IIdentity
    {
        private IIdentity originalIdentity;

        public UserProfileIdentity(IIdentity originalIdentity, User userProfile)
        {
            this.originalIdentity = originalIdentity;
            this.CopyFrom(userProfile);
        }

        #region IIdentity Members

        string IIdentity.AuthenticationType
        {
            get { return originalIdentity.AuthenticationType; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return originalIdentity.IsAuthenticated; }
        }

        string IIdentity.Name
        {
            get { return originalIdentity.Name; }
        }

        #endregion

        private void CopyFrom(User userProfile)
        {
            this.CreatedAt = userProfile.CreatedAt;
            this.CreatedBy = userProfile.CreatedBy;
            this.CreatedByID = userProfile.CreatedByID;
            this.Active = userProfile.Active;
            this.UpdatedAt = userProfile.UpdatedAt;
            this.UpdatedBy = userProfile.UpdatedBy;
            this.UpdatedByID = userProfile.UpdatedByID;
            this.UserName = userProfile.UserName;
            this.FullName = userProfile.FullName;
            this.EnglishFullName = userProfile.EnglishFullName;
            this.Id = userProfile.Id;
            this.Email = userProfile.Email;
            this.Mobile = userProfile.Mobile;
            this.PhoneNumber = userProfile.PhoneNumber;
            this.UserType = userProfile.UserType;
        }

    }
}
