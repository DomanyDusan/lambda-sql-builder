/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using LambdaSqlBuilder.ValueObjects;

namespace LambdaSqlBuilder.Builder
{
    /// <summary>
    /// Implements the expression buiding for the WHERE statement
    /// </summary>
    partial class SqlQueryBuilder
    {
        public void BeginExpression()
        {
            _conditions.Add("(");
        }

        public void EndExpression()
        {
            _conditions.Add(")");
        }

        public void And()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" AND ");
        }

        public void Or()
        {
            if (_conditions.Count > 0)
                _conditions.Add(" OR ");
        }

        public void Not()
        {
            _conditions.Add(" NOT ");
        }

        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} {1} {2}",
                Adapter.Field(tableName, fieldName),
                op,
                Adapter.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} LIKE {1}",
                Adapter.Field(tableName, fieldName),
                Adapter.Parameter(paramId));

            _conditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldNull(string tableName, string fieldName)
        {
            _conditions.Add(string.Format("{0} IS NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            _conditions.Add(string.Format("{0} IS NOT NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            string newCondition = string.Format("{0} {1} {2}",
            Adapter.Field(leftTableName, leftFieldName),
            op,
            Adapter.Field(rightTableName, rightFieldName));

            _conditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, SqlLamBase sqlQuery)
        {
            var innerQuery = sqlQuery.QueryString;            
            foreach (var param in sqlQuery.QueryParameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), innerQuery);

            _conditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
                                             {
                                                 var paramId = NextParamId();
                                                 AddParameter(paramId, x);
                                                 return Adapter.Parameter(paramId);
                                             });

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), string.Join(",", paramIds));
            _conditions.Add(newCondition);
        }
    }
}
