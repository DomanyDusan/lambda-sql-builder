using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LambdaSqlBuilder.Adapter;

namespace LambdaSqlBuilder.ValueObjects
{
    /// <summary>
    /// An enumeration of the available SQL adapters. Can be used to set the backing database for db specific SQL syntax
    /// </summary>
    public enum SqlAdapter
    {
        SqlServer2008,
        SqlServer2012
    }
}
