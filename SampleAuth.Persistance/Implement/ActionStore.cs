using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Persistance.Implement
{
    public class ActionStore : GenericStore<Domain.Action, Guid>, IActionStore
    {
        public ActionStore(DataContext context) : base(context)
        {
        }
    }
}
