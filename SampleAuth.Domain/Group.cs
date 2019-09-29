using Infrastructure;
using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Domain
{
    public class Group : AggregateRoot<Guid>, ICascadable<Guid>
    {
        public Group()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid(SequentialGuidType.SequentialAsString);
        }

        public Guid ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
