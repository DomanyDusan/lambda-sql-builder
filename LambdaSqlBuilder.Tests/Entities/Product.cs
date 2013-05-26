
namespace LambdaSqlBuilder.Tests.Entities
{
    [SqlLamTable(Name = "Products")]
    public class Product
    {
        [SqlLamColumn(Name = "Product ID")]
        public int ProductId { get; set; }

        public int GetProductId()
        {
            return ProductId;
        }
        
        [SqlLamColumn(Name = "Product Name")]
        public string ProductName { get; set; }

        [SqlLamColumn(Name = "English Name")]
        public string EnglishName { get; set; }
        
        [SqlLamColumn(Name = "Category ID")]
        public int CategoryId { get; set; }

        [SqlLamColumn(Name = "Unit Price")]
        public double UnitPrice { get; set; }

        [SqlLamColumn(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }

        [SqlLamColumn(Name = "Reorder Level")]
        public int? NullableReorderLevel { get { return ReorderLevel; } }

        [SqlLamColumn(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        public Category Category { get; set; }
    }
}
