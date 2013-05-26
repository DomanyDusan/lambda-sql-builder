/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LambdaSqlBuilder.Resolver
{
    partial class LambdaResolver
    {
        public void QueryByIsIn<T>(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            var fieldName = GetColumnName(expression);
            _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        }

        public void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = GetColumnName(expression);
            _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        }

        public void QueryByNotIn<T>(Expression<Func<T, object>> expression, SqlLamBase sqlQuery)
        {
            var fieldName = GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        }

        public void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = GetColumnName(expression);
            _builder.Not();
            _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        }
    }
}
