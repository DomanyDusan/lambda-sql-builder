using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LambdaSqlBuilder.Tests.Entities;
using LambdaSqlBuilder.Tests.Infrastructure;
using NUnit.Framework;
using Dapper;

namespace LambdaSqlBuilder.Tests
{
    public class ExpressionTests : TestBase
    {
        /// <summary>
        /// Find the products which name starts with 'To'
        /// </summary>
        [Test]
        public void FindByStringPrefix()
        {
            const string prefix = "To";

            var query = new SqlLam<Product>(p => p.ProductName.StartsWith(prefix));

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(2, result.Count);

            foreach (var product in result)
            {
                Assert.IsTrue(product.ProductName.StartsWith(prefix));
            }
        }

        /// <summary>
        /// Find the products which name ends with 'ld'
        /// </summary>
        [Test]
        public void FindByStringSuffix()
        {
            const string suffix = "ld";

            var query = new SqlLam<Product>(p => p.ProductName.EndsWith(suffix));

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(2, result.Count);

            foreach (var product in result)
            {
                Assert.IsTrue(product.ProductName.EndsWith(suffix));
            }
        }

        /// <summary>
        /// Find the products which name contains substring 'ge'
        /// </summary>
        [Test]
        public void FindByStringPart()
        {
            const string part = "ge";

            var query = new SqlLam<Product>(p => p.ProductName.Contains(part));

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(9, result.Count);

            foreach (var product in result)
            {
                Assert.IsTrue(product.ProductName.ToLower().Contains(part));
            }
        }

        /// <summary>
        /// Find the products which name is equal to string 'Tofu'
        /// </summary>
        [Test]
        public void FindByStringEquality()
        {
            const string name = "Tofu";

            var query = new SqlLam<Product>(p => p.ProductName.Equals(name));

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).Single();

            Assert.AreEqual(name, result.ProductName);
        }

        /// <summary>
        /// Use an unsupported function to trigger the exception
        /// </summary>
        [Test]
        public void FindByInvalidFunction()
        {            
            Assert.Throws<ArgumentException>(() => new SqlLam<Product>(p => p.ProductName.IsNormalized()));
        }

        /// <summary>
        /// Find the orders which ship region is null
        /// </summary>
        [Test]
        public void FindByNull()
        {
            var query = new SqlLam<Order>(o => o.ShipRegion == null);

            var result = Connection.Query<Order>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(671, result.Count);

            foreach (var order in result)
            {
                Assert.IsTrue(order.ShipRegion == null);
            }
        }

        /// <summary>
        /// Find the orders which ship region is not null
        /// </summary>
        [Test]
        public void FindByNotNull()
        {
            var query = new SqlLam<Order>(o => o.ShipRegion != null);

            var result = Connection.Query<Order>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(407, result.Count);

            foreach (var order in result)
            {
                Assert.IsTrue(order.ShipRegion != null);
            }
        }

        /// <summary>
        /// Find products with unit price greater than 10.5
        /// </summary>
        [Test]
        public void FindByDouble()
        {
            const double minUnitPrice = 10.5d;

            var query = new SqlLam<Product>(p => p.UnitPrice >= minUnitPrice);

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(63, result.Count);

            foreach (var product in result)
            {
                Assert.GreaterOrEqual(product.UnitPrice, minUnitPrice);
            }
        }

        /// <summary>
        /// Find products by nullable reorder level
        /// </summary>
        [Test]
        public void FindByNullableField()
        {
            var product = new Product() {ReorderLevel = 5};

            var query = new SqlLam<Product>(p => p.NullableReorderLevel == product.NullableReorderLevel.Value);

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(8, result.Count);
        }

        /// <summary>
        /// Find discontinued products 
        /// </summary>
        [Test]
        public void FindByBoolean()
        {
            const int expectedNumberOfResults = 8;

            var query = new SqlLam<Product>(p => p.Discontinued);

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedNumberOfResults, result.Count);

            foreach (var product in result)
            {
                Assert.AreEqual(true, product.Discontinued);
            }
        }

        /// <summary>
        /// Find employees born before 1.1.1960
        /// </summary>
        [Test]
        public void FindByDateTime()
        {
            var minBirthDate = new DateTime(1960, 1, 1);
            const int expectedNumberOfResults = 5;

            var query = new SqlLam<Employee>(p => p.BirthDate < minBirthDate);

            var result = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedNumberOfResults, result.Count);

            foreach (var employee in result)
            {
                Assert.Less(employee.BirthDate, minBirthDate);
            }
        }

        /// <summary>
        /// Test guid access and nullable guid conversion
        /// </summary>
        [Test]
        public void FindByGuidFake()
        {
            var fakeGuid = new FakeGuid();

            var query = new SqlLam<FakeGuid>(p => p.Id == fakeGuid.Id.Value);
            Assert.AreEqual("[FakeGuid].[Id] = @Param1", query.SqlBuilder.WhereConditions.First());
            Assert.AreEqual(fakeGuid.Id.Value, query.QueryParameters.First().Value);
        }

        /// <summary>
        /// Find orders where the shipped date is after the required date
        /// </summary>
        [Test]
        public void FindByMemberComparison()
        {
            const int expectedNumberOfResults = 55;

            var query = new SqlLam<Order>(o => o.RequiredDate < o.ShippedDate);

            var result = Connection.Query<Order>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedNumberOfResults, result.Count);

            foreach (var order in result)
            {
                Assert.Less(order.RequiredDate, order.ShippedDate);
            }
        }

        /// <summary>
        /// Find product by an Id retrieved using a member access
        /// </summary>
        [Test]
        public void FindByMemberAccess()
        {
            var product = new Product()
                              {
                                  ProductId = 17
                              };

            var query = new SqlLam<Product>(p => p.ProductId == product.ProductId);

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).Single();

            Assert.AreEqual(product.ProductId, result.ProductId);
        }

        /// <summary>
        /// Find category by an Id retrieved using a member access
        /// </summary>
        [Test]
        public void FindByDoubleMemberAccess()
        {
            var category = new Category()
            {
                CategoryId = 8
            };

            var product = new Product()
            {
                Category = category
            };

            var query = new SqlLam<Category>(c => c.CategoryId == product.Category.CategoryId);

            var result = Connection.Query<Category>(query.QueryString, query.QueryParameters).Single();

            Assert.AreEqual(category.CategoryId, result.CategoryId);
        }

        /// <summary>
        /// Find product by an Id retrieved using a method call
        /// </summary>
        [Test]
        public void FindByMethodCall()
        {
            var product = new Product()
            {
                ProductId = 17
            };

            var query = new SqlLam<Product>(p => p.ProductId == product.GetProductId());

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).Single();

            Assert.AreEqual(product.ProductId, result.ProductId);
        }

        /// <summary>
        /// Find product by an Id retrieved using a method call
        /// </summary>
        [Test]
        public void FindByMemberAccessAndMethodCall()
        {
            var category = new Category()
            {
                CategoryId = 8
            };

            var product = new Product()
            {
                Category = category
            };

            var query = new SqlLam<Category>(c => c.CategoryId == product.Category.GetCategoryId());

            var result = Connection.Query<Category>(query.QueryString, query.QueryParameters).Single();

            Assert.AreEqual(category.CategoryId, result.CategoryId);
        }

        [Test]
        public void FindByComplexQuery1()
        {
            const int expectedResultCount = 3;
            var expectedNames = new [] {"Nancy", "Margaret", "Laura"};

            var query = new SqlLam<Employee>(p => p.City == "Seattle" || p.City == "Redmond" && p.Title == "Sales Representative")
                    .OrderByDescending(p => p.FirstName);

            var results = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedResultCount, results.Count);
            for (int i=0; i < expectedResultCount; ++i)
            {
                Assert.AreEqual(expectedNames[i], results[i].FirstName);
            }
        }

        [Test]
        public void FindByComplexQuery2()
        {
            const int expectedResultCount = 2;
            var expectedNames = new[] { "Nancy", "Margaret" };

            var query = new SqlLam<Employee>(p => (p.City == "Seattle" || p.City == "Redmond") && p.Title == "Sales Representative")
                    .OrderByDescending(p => p.FirstName);

            var results = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedResultCount, results.Count);
            for (int i = 0; i < expectedResultCount; ++i)
            {
                Assert.AreEqual(expectedNames[i], results[i].FirstName);
            }
        }

        [Test]
        public void FindByComplexQuery2Flipped()
        {
            const int expectedResultCount = 2;
            var expectedNames = new[] { "Nancy", "Margaret" };

            var query = new SqlLam<Employee>(p => "Sales Representative" == p.Title && ("Seattle" == p.City || "Redmond" == p.City) )
                    .OrderByDescending(p => p.FirstName);

            var results = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedResultCount, results.Count);
            for (int i = 0; i < expectedResultCount; ++i)
            {
                Assert.AreEqual(expectedNames[i], results[i].FirstName);
            }
        }

        [Test]
        public void FindByComplexQuery2Negated()
        {
            const int expectedResultCount = 13;

            var query = new SqlLam<Employee>(p => !(p.City == "Seattle" || p.City == "Redmond") || p.Title != "Sales Representative")
                    .OrderByDescending(p => p.FirstName);

            var results = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedResultCount, results.Count);
        }

        [Test]
        public void FindByComplexQuery3()
        {
            const int expectedNumberOfResults = 16;
            const int reorderLevel = 0;

            var query = new SqlLam<Product>(p => !p.Discontinued && p.ReorderLevel == reorderLevel);

            var result = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedNumberOfResults, result.Count);

            foreach (var product in result)
            {
                Assert.AreEqual(false, product.Discontinued);
                Assert.AreEqual(reorderLevel, product.ReorderLevel);
            }
        }

        [Test]
        public void FindByComplexQuery4()
        {
            var dateTime1 = new DateTime(1900, 1, 1);
            var dateTime2 = new DateTime(1950, 1, 1);
            var dateTime3 = new DateTime(1970, 1, 1);
            var dateTime4 = new DateTime(2000, 1, 1);

            const int expectedNumberOfResults = 5;

            var query =
                new SqlLam<Employee>(e =>
                    (e.BirthDate > dateTime1 && e.BirthDate < dateTime2) 
                    ||
                    (e.BirthDate > dateTime3 && e.BirthDate < dateTime4));

            var result = Connection.Query<Employee>(query.QueryString, query.QueryParameters).ToList();

            Assert.AreEqual(expectedNumberOfResults, result.Count);

            foreach (var employee in result)
            {
                Assert.IsTrue((employee.BirthDate > dateTime1 && employee.BirthDate < dateTime2) 
                    || (employee.BirthDate > dateTime3 && employee.BirthDate < dateTime4));
            }
        }
    }
}
