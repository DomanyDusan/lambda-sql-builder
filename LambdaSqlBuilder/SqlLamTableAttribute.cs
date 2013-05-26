using System;

namespace LambdaSqlBuilder
{
    /// <summary>
    /// Configures the name of the db table related to this entity. If the attribute is not specified, the class name is used instead.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlLamTableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
