﻿using URCP.Core;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URCP.SqlServerRepository
{
    public class IdentityUserRepository : IIdentityUserRepository
    {
        private MyDbContext _context;
        private IGenericRepository _repository;
        private IGenericQueryRepository _queryRepository;

        public IdentityUserRepository(MyDbContext context, IGenericRepository genericRepository, IGenericQueryRepository queryRepository) 
        {
            this._context = context ?? throw new ArgumentNullException("DbContext cannot be null.");
            this._repository = genericRepository;
            this._queryRepository = queryRepository;
        }

        #region IUserStore  

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.FromResult(FindById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var result = FindByName(userName);
            return Task.FromResult(result);
        } 

        public void Dispose()
        {
        }

        #endregion

        #region IUserStore (Core)

        public async Task CreateAsync(User user)
        {
            var updatedUser = _repository.Create(user); 
        }

        public async Task UpdateAsync(User user)
        { 
            var updateUser = _repository.Update<User>(user); 
        }

        public async Task DeleteAsync(User user)
        {
            _repository.Delete(user);
            await this._context.SaveChangesAsync();
        } 

        public User FindById(int userId)
        {
            var constraints = new QueryConstraints<User>()
                .IncludePath(x => x.Roles)
                .Where(x => x.Id == userId);
            return _queryRepository.SingleOrDefault<User>(constraints);
        }

        public User FindByName(string userName)
        {
            var constraints = new QueryConstraints<User>()
                .IncludePath(x => x.Roles)
                .Where(x => x.UserName == userName);

            return _queryRepository.SingleOrDefault<User>(constraints);
        }

        

        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(GetPasswordHash(user));
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(HasPassword(user));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            SetPasswordHash(user, passwordHash);
            return Task.FromResult(0);
        }


        public Task<User> FindByIdAsync(string userId)
        {
            return FindByIdAsync(int.Parse(userId));
        }

        #endregion

        #region IUserPasswordStore (Core)
        public string GetPasswordHash(User user)
        {
            return user.PasswordHash;
        }

        public bool HasPassword(User user)
        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public void SetPasswordHash(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
        }
        #endregion

        #region IUserEmailStore
        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult(FindByEmail(email));
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(GetEmail(user));
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(GetEmailConfirmed(user));
        }

        public Task SetEmailAsync(User user, string email)
        {
            SetEmail(user, email);
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            SetEmailConfirmed(user, confirmed);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserEmailStore Core
        public User FindByEmail(string email)
        {
            var constraints = new QueryConstraints<User>()
                .Where(x => x.Email == email);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public string GetEmail(User user)
        {
            return user.Email;
        }

        public bool GetEmailConfirmed(User user)
        {
            return user.EmailConfirmed;
        }

        public void SetEmail(User user, string email)
        {
            user.Email = email;
        }

        public void SetEmailConfirmed(User user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null or empty.");

            user.EmailConfirmed = confirmed;
        }

        #endregion

        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(GetAccessFailedCount(user));
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(GetLockoutEnabled(user));
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(GetLockoutEndDate(user));
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(IncrementAccessFailedCount(user));
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            ResetAccessFailedCount(user);
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            SetLockoutEnabled(user, enabled);
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            SetLockoutEndDate(user, lockoutEnd);
            return Task.FromResult(0);
        }
        #endregion

        #region IUserLockoutStore Core
        public int GetAccessFailedCount(User user)
        {
            return user.AccessFailedCount;
        }

        public bool GetLockoutEnabled(User user)
        {
            return user.LockoutEnabled;
        }

        public DateTimeOffset GetLockoutEndDate(User user)
        {
            return user.LockoutEnd.Value;
        }

        public int IncrementAccessFailedCount(User user)
        {
            return ++user.AccessFailedCount;
        }

        public void ResetAccessFailedCount(User user)
        {
            user.AccessFailedCount = 0;
        }

        public void SetLockoutEnabled(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
        }

        public void SetLockoutEndDate(User user, DateTimeOffset? lockoutEnd)
        {
            user.LockoutEnd = lockoutEnd;
        }
        #endregion

        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(GetTwoFactorEnabled(user));
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            SetTwoFactorEnabled(user, enabled);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserTwoFactorStore Core
        public bool GetTwoFactorEnabled(User user)
        {
            return user.TwoFactorEnabled;
        }

        public void SetTwoFactorEnabled(User user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(User user, string roleName)
        {
            AddToRole(user, roleName);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.FromResult(GetRoles(user));
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.FromResult(IsInRole(user, roleName));
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            RemoveFromRole(user, roleName);
            return Task.FromResult(0);
        }

        #endregion

        #region IUserRoleStore core

        public void AddToRole(User user, string roleName)
        {
            var roleConstraints = new QueryConstraints<IdentityRole>()
                .Where(r => r.Name == roleName);
            var roleEntity = _queryRepository.SingleOrDefault<IdentityRole>(roleConstraints);
            user.Roles.Add(roleEntity);
        }

        public IList<string> GetRoles(User user)
        {
            return user.Roles.Select(r => r.Name).ToList();
        }

        public bool IsInRole(User user, string roleName)
        {
            return user.Roles.Any(r => r.Name == roleName);
        }

        public void RemoveFromRole(User user, string roleName)
        {
            var role = user.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null) user.Roles.Remove(role);
        } 
        #endregion

    }
}