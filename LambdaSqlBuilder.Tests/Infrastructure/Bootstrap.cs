using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace LambdaSqlBuilder.Tests.Infrastructure
{
    static class Bootstrap
    {
        public const string CONNECTION_STRING = @"Data Source=AppData\Northwind.sdf";

        public static void Initialize()
        {
            InitializeDapper();
        }

        private static void InitializeDapper()
        {
            const string @namespace = "LambdaSqlBuilder.Tests.Entities";

            var entityTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where t.IsClass && t.Namespace == @namespace
                              select t;

            foreach (var entityType in entityTypes)
            {
                SqlMapper.SetTypeMap(
                    entityType,
                    new CustomPropertyTypeMap(
                    entityType,
                    (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                            .OfType<SqlLamColumnAttribute>()
                            .Any(attr => attr.Name == columnName))
                        )
                    );
            }
        }
    }
}
