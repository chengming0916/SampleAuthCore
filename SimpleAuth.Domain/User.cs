using Infrastructure;
using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;

namespace SimpleAuth.Domain
{
    public class User : AggregateRoot<Guid>
    {
        public User()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid(SequentialGuidType.SequentialAsString);
        }

        public string Name { get; set; }

        public string Account { get; set; }

        public string PasswordHash { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
    }
}
