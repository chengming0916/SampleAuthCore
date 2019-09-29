using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Domain
{
    public class UserGroup : EntityBase<int>
    {
        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }
    }
}
