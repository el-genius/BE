using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}