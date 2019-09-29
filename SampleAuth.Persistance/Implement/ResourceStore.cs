using Microsoft.EntityFrameworkCore;
using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance.Implement
{
    public class ResourceStore : GenericStore<Resource, Guid>, IResourceStore
    {
        public ResourceStore(DataContext context) : base(context)
        {
        }

        public Task<Resource> FindByName(string resourceName)
        {
            ThrowIfDisposed();
            return _entityStore.EntitySet.FirstOrDefaultAsync(r => r.Name.Equals(resourceName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
