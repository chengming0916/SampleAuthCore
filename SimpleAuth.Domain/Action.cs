using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Domain
{
    public class Action : AggregateRoot<Guid>
    {
        public Action()
        {
            Id = Infrastructure.SequentialGuidGenerator.NewSequentialGuid(Infrastructure.SequentialGuidType.SequentialAsString);
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
