using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the supported string methods for the SQL LIKE statement. The item names should match the related string methods.
    /// </summary>
    public enum LikeMethod
    {
        StartsWith,
        EndsWith,
        Contains,
        Equals
    }
}
