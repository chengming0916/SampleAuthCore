using Infrastructure.Data.Repositories;
using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance
{
    public interface IResourceStore : IStore<Resource>
    {
        Task<Resource> FindByName(string resourceName);
    }
}
