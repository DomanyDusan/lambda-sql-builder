/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Adapter
{
    /// <summary>
    /// Provides functionality common to all supported databases
    /// </summary>
    class SqlAdapterBase
    {
        public string QueryString(string selection, string source, string conditions, string order, string grouping, string having)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} {4} {5}",
                                 selection, source, conditions, order, grouping, having);
        }
    }
}
