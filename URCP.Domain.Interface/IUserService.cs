using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.RepositoryInterface;

namespace URCP.Domain.Interface
{
    public interface IUserService
    {
        void SkipPermission();

        void Commit();

        Task CommitAsync();

        IQueryResult<User> Find(UserSearchCriteria userSearchCriteria, String currentUserName);

        List<User> GetAll();

        IQueryResult<User> GetActiveUsers();

        IQueryResult<User> GetAllUsers();

        Task<User> CreateOrUpdate(User user);

        Task<User> Create(User user, string password);

        User FindByUserName(string userName);

        User FindById(int userId);

        IList<string> GetRoles(User user);

        User SingleByMobile(String mobileNumber);

        User SingleByEmail(String email);

        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        Task<User> FindByIdAsync(int userId);

        Task<User> FindByNameAsync(string userName);

        Task<IdentityResult> AddToRoleAsync(int userId, string role);

        Task<IdentityResult> AddToRolesAsync(int userId, params string[] roles); 
    }
}
