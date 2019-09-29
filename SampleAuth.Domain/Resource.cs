using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Domain
{
    public class Resource : AggregateRoot<Guid>, ICascadable<Guid>
    {
        public Resource()
        {
            Id = Infrastructure.SequentialGuidGenerator.NewSequentialGuid(Infrastructure.SequentialGuidType.SequentialAsString);
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ParentId { get; set; }
    }
}
