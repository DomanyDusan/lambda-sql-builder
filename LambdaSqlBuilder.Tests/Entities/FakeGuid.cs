using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LambdaSqlBuilder.Tests.Entities
{
    class FakeGuid
    {
        public FakeGuid()
        {
            Id = Guid.NewGuid();
        }

        public Guid? Id { get; set; }
    }
}
