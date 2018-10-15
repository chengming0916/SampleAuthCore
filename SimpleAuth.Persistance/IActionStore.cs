using Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Persistance
{
    public interface IActionStore : IStore<Domain.Action>
    {
    }
}
