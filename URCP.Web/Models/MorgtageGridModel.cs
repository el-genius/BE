using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URCP.Core.Enum;

namespace URCP.Web.Models
{
    public class MorgtageGridModel
    {
        private string _status;

        public int RequestId { get; set; }
        public int Id { get; set; }

        [Display(Name = "IdNumberMortgageOwner", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortageOwnerId { get; set; }

        [Display(Name = "IdNumberMortgaged", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortagedId { get; set; }

        [Display(Name = "StartOfContract", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string StartOfContract { get; set; } // sulaiman

        [Display(Name = "MortgageeName", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgagedName { get; set; } // sulaiman

        [Display(Name = "MortgagorName", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageOwnerName { get; set; } // sulaiman

        [Display(Name = "ContractAction", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string ContractAction { get; set; } // sulaiman

        [Display(Name = "ContractNumber", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string MortgageNumber { get; set; } // sulaiman

        [Display(Name = "EndOfContract", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string EndOfContract { get; set; }

        [Display(Name = "MortgageStatus", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public string Status
        {
            get
            {
                return Util.GetDisplayNameFromResource<Core.Enum.MortgageStatus>(_status, typeof(Resources.SharedStrings_Ar));
            }
            set
            {
                _status = value;
            }
        }

        public MortgageStatus MortgageStatus { get; set; }

        public bool IsMortgaged { get; set; }

        public bool AllowObjection { get; set; }

        public bool AllowCancelation { get; set; }

        [Display(Name = "MoneyType", ResourceType = typeof(Resources.SharedStrings_Ar))]
        public object MortgageMoneyTypeName { get; internal set; }
    }
}