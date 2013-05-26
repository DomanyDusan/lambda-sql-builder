using System.Collections.Generic;

namespace LambdaSqlBuilder.Tests.Entities
{
    [SqlLamTable(Name = "Categories")]
    public class Category
    {
        [SqlLamColumn(Name = "Category ID")]
        public int CategoryId { get; set; }

        public int GetCategoryId()
        {
            return CategoryId;
        }
        
        [SqlLamColumn(Name = "Category Name")]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; }
    }
}
