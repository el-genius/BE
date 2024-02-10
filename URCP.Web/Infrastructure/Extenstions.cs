﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URCP.Core;

namespace URCP.Web
{
    public static class Extenstions
    {
        public static void PopulateIn(this ValidationException valEx, ModelStateDictionary modelState)
        {
            foreach (var item in valEx.ValidationResults)
                modelState.AddModelError("", item.ErrorMessage);
        }
    }
}