using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Tests.Entities
{
    [SqlLamTable(Name = "Order Details")]
    public class OrderDetails
    {
        [SqlLamColumn(Name = "Order ID")]
        public int OrderId { get; set; }

        [SqlLamColumn(Name = "Product ID")]
        public int ProductId { get; set; }
    }
}
