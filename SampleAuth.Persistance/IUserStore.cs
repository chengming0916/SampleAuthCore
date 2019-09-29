using Infrastructure.Data.Repositories;
using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance
{
    interface IUserStore : IStore<User>
    {
        Task<User> FindByNameAsync(string userName);
    }
}
