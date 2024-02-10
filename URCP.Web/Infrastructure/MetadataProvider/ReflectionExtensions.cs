using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace URCP.Web
{
    public static class ReflectionExtensions
    {
        public static bool PropertyExists(this Type type, string propertyName)
        {
            if (type == null || propertyName == null)
            {
                return false;
            }

            var property = type.GetProperty(propertyName,
                BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.Instance);

            if (property == null)
            {
                return false;
            }

            var getter = property.GetGetMethod(true);

            return getter.IsPublic || getter.IsAssembly || getter.IsFamilyOrAssembly;

        }
    }
}