Lambda SQL Builder is a neat .NET library for independent creation of SQL queries in a strongly typed fashion.
--------------------------------------------------------------------------------------------------------------

Support
=======
Apart from standard SQL queries, the currently supported SQL databases are SQL Server 2008 and SQL Server 2012. 
The minimum .NET version required is 4.0.

Motivation
==========
The inspiration for Lambda SQL Builder came from tools like **Linq2SQL**, **Entity Framework**, and **QueryOver** 
in NHibernate. I used to be a big fan of these ORMs, although they gave me some serious headaches from time to time. 
However, nothing lasts forever, and I have recently made a decision to join the new rising trend of using Micro-ORMs 
instead of those huge mighty frameworks. And I definately did not regret that. But I still felt like there was 
something missing, something that I had to sacrifice in order to gain the flexibility and performance back after 
years of using big ORMs. And I soon realized what this thing was:

**Using C# code to write my SQL queries.**

No change tracking, no sessions, it was really just this single feature. I simply hate those SQL strings lying there 
in my application and forcing me to think twice anytime I want to refactor something. After some research, I have 
decided to write something on my own, something independent, something that would provide this single feature and 
nothing more, so I can use it along with **SQLCommand**, or with **Dapper**, or even with Nhibernate if it feels 
right. It is not my intension to cover the whole SQL world, however, I do believe that it will satisfy most 
of the common needs. It certainly does in my case, as I was already able to replace 99% of the queries that I needed 
in my recent projects.

I will be glad if you like the tool. If you don’t, let me know why, so I can make it better.

Usage
=====
This basic example queries the database for products named „Tofu” using my favorite Micro-ORM called Dapper:
```csharp
var query = new SqlLam<Product>(p => p.ProductName == "Tofu");
var results = Connection.Query<Product>(query.QueryString, query.QueryParameters);
```

Of course, you can also do this with SQLCommand and SqlDataReader without using any ORM:
```csharp
var query = new SqlLam<Product>(p => p.ProductName == "Tofu");

var selectCommand = new SqlCommand(query.QueryString, Connection);

foreach (var param in query.QueryParameters)
    selectCommand.Parameters.AddWithValue(param.Key, param.Value);

var result = selectCommand.ExecuteReader();
```

If you prefer using LINQ, you can write something like this:
```csharp
var query = from product in new SqlLam<Product>()
            where product.ProductName == "Tofu"
            select product;

var results = Connection.Query<Product>(query.QueryString, query.QueryParameters)
```

As you can see the QueryString property will return the SQL string itself, while the QueryParameters property refers 
to a dictionary of SQL parameters. 

Most of the time you will create a SqlLam object, call some methods on it, and then request the QueryString and 
the QueryParameters at the end. You can also call the methods in a chain, if you prefer. Here are some more 
complicated queries, so you get an idea of what is the tool capable of:
```csharp
var query = new SqlLam<Employee>(p => !(p.City == "Seattle" || p.City == "Redmond"))
            .Or(p => p.Title != "Sales Representative")
            .OrderByDescending(p => p.FirstName);
```
```csharp
var query = from product in new SqlLam<Product>()
            join category in new SqlLam<Category>()
            on product.CategoryId equals category.CategoryId
            where product.ReorderLevel == 25 && category.CategoryName == "Beverages"
            select product;
```
```csharp
var subQuery = new SqlLam<Category>()
               .WhereIsIn(c => c.CategoryName, new object[] { "Beverages", "Condiments" })
               .Select(p => p.CategoryId);

var query = new SqlLam<Product>()
            .Join<Category>((p, c) => p.CategoryId == c.CategoryId)
            .WhereIsIn(c => c.CategoryId, subQuery);
```

Internally there is a Lambda Resolver that translates the C# code into SQL strings, and a SQL Builder, which collects
the SQL strings and stores them as individual SQL clauses (Selection,  Join, Order By,...). Everytime you call 
a method on the SqlLam object, it is translated into a SQL string, which is stored into the related SQL clause 
in the underlying SQL builder. If the expression contains variables, they are stored into the dictionary 
of SQL parameters. When requesting the QueryString, the SQL clauses are combined and returned as a single 
SQL statement.

There are also additional SQL adapters in development, that should expand the support for database-specific SQL statements. 

Reference
=========
There will be later a deeper description of various use cases. Make sure to check out the unit tests for more examples.
