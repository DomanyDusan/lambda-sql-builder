/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Adapter
{
    /// <summary>
    /// Provides functionality specific to SQL Server 2008
    /// </summary>
    class SqlServer2008Adapter : SqlServerAdapterBase, ISqlAdapter
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize, int pageNumber)
        {
            var innerQuery = string.Format("SELECT {0},ROW_NUMBER() OVER ({1}) AS RN FROM {2} {3}",
                                           selection, order, source, conditions);

            return string.Format("SELECT TOP {0} * FROM ({1}) InnerQuery WHERE RN > {2} ORDER BY RN",
                                 pageSize, innerQuery, pageSize*(pageNumber - 1));
        }
    }
}
