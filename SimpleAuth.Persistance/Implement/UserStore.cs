using SimpleAuth.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SimpleAuth.Persistance.Implement
{
    public class UserStore : GenericStore<User, Guid>, IUserStore, IUserGroupStore, IQueryableStore<User>
    {
        private DbSet<UserGroup> _userGroups;
        private EntityStore<Group> _groupStore;

        public UserStore(DataContext context) : base(context)
        {
            _userGroups = context.Set<UserGroup>();
            _groupStore = new EntityStore<Group>(context);
        }

        public IQueryable<User> QueryContext { get { return _entityStore.DbEntitySet; } }

        public Task<User> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
        }

        private async Task<User> GetUserAggregateAsync(Expression<Func<User, bool>> filter)
        {
            User user;
            if (FindByIdFilterParser.TryMatchAndGetId<Guid>(filter, out Guid id))
            {
                user = await _entityStore.GetByIdAsync(id);
            }
            else
            {
                user = await QueryContext.FirstOrDefaultAsync(filter);
            }
            if (user != null)
                await EnsureGroupsLoaded(user);
            return user;
        }

        private async Task EnsureGroupsLoaded(User user)
        {
            if (!Context.Entry(user).Collection(u => u.Groups).IsLoaded)
            {
                Guid userId = user.Id;
                await _userGroups.Where(ug => ug.UserId.Equals(userId)).LoadAsync();
                Context.Entry(user).Collection(u => u.Groups).IsLoaded = true;
            }
        }

        private bool AreGroupsLoaded(User user)
        {
            return Context.Entry(user).Collection(u => u.Groups).IsLoaded;
        }

        public async Task<bool> IsInGroupAsync(User user, string groupName)
        {
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrEmpty(groupName)) throw new ArgumentNullException(nameof(groupName));

            var group = await _groupStore.DbEntitySet.SingleOrDefaultAsync(g => g.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));

            if (group != null)
                return await _userGroups.AnyAsync(ug => ug.GroupId.Equals(group.Id) && ug.UserId.Equals(user.Id));
            return false;
        }

        public async Task AddToGroupAsync(User user, string groupName)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(nameof(groupName)))
                throw new ArgumentNullException(nameof(groupName));
            var group = await _groupStore.DbEntitySet.SingleOrDefaultAsync(g => g.Name.Equals(g.Name, StringComparison.CurrentCultureIgnoreCase));
            if (group == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "", groupName));
            var ug = new UserGroup { UserId = user.Id, GroupId = group.Id };
            _userGroups.Add(ug);
        }

        public async Task RemoveFromGroupAsync(User user, string groupName)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(nameof(groupName));
            var group = await _groupStore.DbEntitySet.SingleOrDefaultAsync(g => g.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            if (group != null)
            {
                var userGroup = await _userGroups.FirstOrDefaultAsync(ug => ug.UserId.Equals(user.Id) && ug.GroupId.Equals(group.Id));
                if (userGroup != null)
                    _userGroups.Remove(userGroup);
            }
        }

        public async Task<IList<string>> GetGroupsAsync(User user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var query = from userGroup in _userGroups
                        where userGroup.UserId.Equals(user.Id)
                        join g in _groupStore.DbEntitySet
                        on userGroup.GroupId equals g.Id
                        select g.Name;
            return await query.ToListAsync();
        }

        private static class FindByIdFilterParser
        {
            // method we need to match: Object.Equals()
            private static readonly MethodInfo EqualsMethodInfo = ((MethodCallExpression)Predicate.Body).Method;

            // expression pattern we need to match
            private static readonly Expression<Func<User, bool>> Predicate = u => u.Id.Equals(0);

            // property access we need to match: User.Id
            private static readonly MemberInfo UserIdMemberInfo = ((MemberExpression)((MethodCallExpression)Predicate.Body).Object).Member;

            internal static bool TryMatchAndGetId<TKey>(Expression<Func<User, bool>> filter, out TKey id) where TKey : IEquatable<TKey>
            {
                // default value in case we can’t obtain it
                id = default(TKey);

                // lambda body should be a call
                if (filter.Body.NodeType != ExpressionType.Call)
                {
                    return false;
                }

                // actually a call to object.Equals(object)
                var callExpression = (MethodCallExpression)filter.Body;
                if (callExpression.Method != EqualsMethodInfo)
                {
                    return false;
                }
                // left side of Equals() should be an access to User.Id
                if (callExpression.Object == null
                    || callExpression.Object.NodeType != ExpressionType.MemberAccess
                    || ((MemberExpression)callExpression.Object).Member != UserIdMemberInfo)
                {
                    return false;
                }

                // There should be only one argument for Equals()
                if (callExpression.Arguments.Count != 1)
                {
                    return false;
                }

                MemberExpression fieldAccess;
                if (callExpression.Arguments[0].NodeType == ExpressionType.Convert)
                {
                    // convert node should have an member access access node
                    // This is for cases when primary key is a value type
                    var convert = (UnaryExpression)callExpression.Arguments[0];
                    if (convert.Operand.NodeType != ExpressionType.MemberAccess)
                    {
                        return false;
                    }
                    fieldAccess = (MemberExpression)convert.Operand;
                }
                else if (callExpression.Arguments[0].NodeType == ExpressionType.MemberAccess)
                {
                    // Get field member for when key is reference type
                    fieldAccess = (MemberExpression)callExpression.Arguments[0];
                }
                else
                {
                    return false;
                }

                // and member access should be a field access to a variable captured in a closure
                if (fieldAccess.Member.MemberType != MemberTypes.Field
                    || fieldAccess.Expression.NodeType != ExpressionType.Constant)
                {
                    return false;
                }

                // expression tree matched so we can now just get the value of the id
                var fieldInfo = (FieldInfo)fieldAccess.Member;
                var closure = ((ConstantExpression)fieldAccess.Expression).Value;

                id = (TKey)fieldInfo.GetValue(closure);
                return true;
            }
        }
    }
}