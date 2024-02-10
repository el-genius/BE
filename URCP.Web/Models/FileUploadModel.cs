using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URCP.Web.Models
{
    public class FileUploadModel
    {
        public string Name { get; set; }

        public int FileMaxSize { get; set; }

        public string[] AllowedExtensions { get; set; }
    }
}