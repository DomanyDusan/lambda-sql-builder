using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Tests.Entities
{
    [SqlLamTable(Name = "Orders")]
    public class Order
    {
        [SqlLamColumn(Name = "Order ID")]
        public int OrderId { get; set; }

        [SqlLamColumn(Name = "Ship Name")]
        public string ShipName { get; set; }

        [SqlLamColumn(Name = "Ship Region")]
        public string ShipRegion { get; set; }

        [SqlLamColumn(Name = "Required Date")]
        public DateTime RequiredDate { get; set; }

        [SqlLamColumn(Name = "Shipped Date")]
        public DateTime ShippedDate { get; set; }

        public List<Product> Products { get; set; }
    }
}
