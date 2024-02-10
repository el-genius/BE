using URCP.GraphDiff.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace URCP.GraphDiff.Attributes
{
    /// <summary>
    /// Marks this property as owned by the parent type or by the chosen AggregateType
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OwnedAttribute : AggregateDefinitionAttribute
    {
    }
}
