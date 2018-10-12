using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Domain
{
    public class Permission : AggregateRoot<Guid>
    {
        public Permission()
        {
            Id = Infrastructure.SequentialGuidGenerator.NewSequentialGuid(Infrastructure.SequentialGuidType.SequentialAsString);
        }

        public Guid ActionId { get; set; }

        public Guid GroupId { get; set; }

        public Guid ResourceId { get; set; }
    }
}
