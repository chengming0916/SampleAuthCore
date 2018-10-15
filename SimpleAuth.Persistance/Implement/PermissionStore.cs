using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Persistance.Implement
{
    public class PermissionStore : GenericStore<Permission, Guid>, IPermissionStore
    {
        public PermissionStore(DataContext context) : base(context)
        {
        }
    }
}
