using SimpleAuth.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance
{
    internal interface IUserGroupStore
    {
        Task AddToGroupAsync(User user, string groupName);

        Task RemoveFromGroupAsync(User user, string groupName);

        Task<IList<string>> GetGroupsAsync(User user);

        Task<bool> IsInGroupAsync(User user, string groupName);
    }
}