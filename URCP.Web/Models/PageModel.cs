using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class PageModel
    {
        public String PageName { get; set; }
        public String Action { get; set; }
        public String Controller { get; set; }
        public String Area { get; set; }
        public String LinkId { get; set; }
    }
}