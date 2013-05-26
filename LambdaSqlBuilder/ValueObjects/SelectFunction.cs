using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the supported aggregate SQL functions. The item names should match the related function names
    /// </summary>
    public enum SelectFunction
    {
        COUNT,
        DISTINCT,
        SUM,
        MIN,
        MAX,
        AVG
    }
}
