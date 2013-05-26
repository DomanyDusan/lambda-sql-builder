using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LambdaSqlBuilder.Tests.Infrastructure
{
    public abstract class TestBase
    {
        protected SqlCeConnection Connection;

        [SetUp]
        public void Init()
        {
            Bootstrap.Initialize();

            Connection = new SqlCeConnection(Bootstrap.CONNECTION_STRING);
            Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Connection.Close();
        }
    }
}
