using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Resolver.ExpressionTree
{
    class MemberNode : Node
    {
        public string TableName { get; set; }
        public string FieldName { get; set; }
    }
}
