﻿using System.Collections.Generic;

namespace URCP.Web.Models
{
    public class RolesTreeViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string root { get; set; }
        public bool selected { get; set; }
        public List<RolesTreeViewModel> children { get; set; }
        public RolesTreeViewModel()
        {
            children = new List<RolesTreeViewModel>();
        }

    }
}