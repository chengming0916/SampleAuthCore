using Infrastructure.Data.Repositories;
using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Persistance
{
    public interface IPermissionStore : IStore<Permission>
    {
    }
}
