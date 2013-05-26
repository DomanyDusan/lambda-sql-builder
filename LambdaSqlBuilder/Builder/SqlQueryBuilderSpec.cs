/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LambdaSqlBuilder.ValueObjects;

namespace LambdaSqlBuilder.Builder
{
    /// <summary>
    /// Implements the SQL building for JOIN, ORDER BY, SELECT, and GROUP BY
    /// </summary>
    partial class SqlQueryBuilder
    {
        public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
        {
            var joinString = string.Format("JOIN {0} ON {1} = {2}",
                                           Adapter.Table(joinTableName), 
                                           Adapter.Field(originalTableName, leftField),
                                           Adapter.Field(joinTableName, rightField));
            _tableNames.Add(joinTableName);
            _joinExpressions.Add(joinString);
            _splitColumns.Add(rightField);
        }

        public void OrderBy(string tableName, string fieldName, bool desc = false)
        {
            var order = Adapter.Field(tableName, fieldName);
            if (desc)
                order += " DESC";

            _sortList.Add(order);            
        }

        public void Select(string tableName)
        {
            var selectionString = string.Format("{0}.*", Adapter.Table(tableName));
            _selectionList.Add(selectionString);
        }

        public void Select(string tableName, string fieldName)
        {
            _selectionList.Add(Adapter.Field(tableName, fieldName));
        }

        public void Select(string tableName, string fieldName, SelectFunction selectFunction)
        {
            var selectionString = string.Format("{0}({1})", selectFunction.ToString(), Adapter.Field(tableName, fieldName));
            _selectionList.Add(selectionString);
        }

        public void GroupBy(string tableName, string fieldName)
        {
            _groupingList.Add(Adapter.Field(tableName, fieldName));
        }
    }
}
