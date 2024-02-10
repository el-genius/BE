using URCP.Core;
using URCP.Core.Enum;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using URCP.Domain.Interface;
using System.Web.Mvc;
using URCP.SqlServerRepository;
using System.Threading.Tasks;
using System.Threading;

namespace URCP.Domain
{
    public class UserService : UserManager<User, int>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IGenericQueryRepository _queryRepository;
        private readonly IGenericRepository _repository;
        private readonly BaseService _baseService;
        private bool _skipPermission;

        public UserService(BaseService baseService, IUnitOfWork unitOfWork, IGenericRepository repository, IGenericQueryRepository queryRepository, IIdentityUserRepository store)
            : base(store)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._queryRepository = queryRepository;
            this._identityUserRepository = store;
            this._baseService = baseService;
            ConfigureManager();
        }

        public void SkipPermission()
        {
            this._skipPermission = true;
        }


        private void ConfigureManager()
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(30);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;
        }

        public void Commit()
        {
            this._baseService.Commit();
        }

        public async Task CommitAsync()
        {
            await this._baseService.CommitAsync();

        }

        public IQueryResult<User> Find(UserSearchCriteria userSearchCriteria, String currentUserName)
        {
            if (userSearchCriteria == null)
                throw new ArgumentNullException("userSearchCriteria", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Page(userSearchCriteria.PageNumber, userSearchCriteria.PageSize)
                .Where(c => c.IsDeleted == false)
                .AndAlso(c => c.UserName != currentUserName);

            if (userSearchCriteria.IsActive.HasValue)
                constraints.AndAlso(c => c.Active == userSearchCriteria.IsActive.Value);

            if (userSearchCriteria.IsActiveDirectory.HasValue)
                constraints.AndAlso(c => c.IsADUser == userSearchCriteria.IsActiveDirectory.Value);

            if (!string.IsNullOrEmpty(userSearchCriteria.FullName))
                constraints.AndAlso(c => c.FullName.Contains(userSearchCriteria.FullName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Mobile))
                constraints.AndAlso(c => c.Mobile.Contains(userSearchCriteria.Mobile));

            if (!string.IsNullOrEmpty(userSearchCriteria.RoleName))
                constraints.AndAlso(c => c.Roles.Any(r => r.Name == userSearchCriteria.RoleName));

            if (!string.IsNullOrEmpty(userSearchCriteria.UserName))
                constraints.AndAlso(c => c.UserName.Contains(userSearchCriteria.UserName));

            if (!string.IsNullOrEmpty(userSearchCriteria.Email))
                constraints.AndAlso(c => c.Email.Contains(userSearchCriteria.Email));

            if (string.IsNullOrEmpty(userSearchCriteria.Sort))
                constraints.SortByDescending(c => c.CreatedAt);
            else if (userSearchCriteria.SortDirection == WebGridSortOrder.Ascending)
                constraints.SortBy(userSearchCriteria.Sort);
            else
                constraints.SortByDescending(userSearchCriteria.Sort);


            return _queryRepository.Find(constraints);
        }

        public List<User> GetAll()
        {
            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Page(1, int.MaxValue)
                .Where(u => u.PasswordHash == null);

            return _queryRepository.Find(constraints).Items.ToList();
        }

        public IQueryResult<User> GetActiveUsers()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => c.Active);

            return _queryRepository.Find(constraints);
        }

        public IQueryResult<User> GetAllUsers()
        {
            var constraints = new QueryConstraints<User>()
                .Page(1, int.MaxValue)
                .Where(c => true);

            return _queryRepository.Find(constraints);
        }

        public async Task<User> CreateOrUpdate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            if (!Thread.CurrentPrincipal.IsInRole(RoleNames.CreateUser) && this._skipPermission == false)
                throw new PermissionException("You have no permission to execute this operation.", RoleNames.CreateUser);

            if (!user.Validate())
                throw new ValidationException("Business Entity has invalid information.", user.ValidationResults, ErrorCode.InvalidData);
             
            if (!user.IsUserNameUnique(user.Id > 0))
                throw new BusinessRuleException("اسم المستخدم موجود مسبقاً", ErrorCode.NotUnique);

            if (!user.IsMobileUnique(user.Id > 0))
                throw new BusinessRuleException("رقم الجوال موجود مسبقاً", ErrorCode.NotUnique);

            if (!user.IsEmailUnique(user.Id > 0))
                throw new BusinessRuleException("البريد الالكتروني موجود مسبقاً", ErrorCode.NotUnique);

            user.FormatUserMobileToFullSaudiMobileNumber();

            IdentityResult result = null;
            if (user.Id > 0)
                result = await UpdateAsync(user);
            else
                result = await CreateAsync(user);

            if (result.Errors.Any())
                throw new BusinessRuleException(string.Join(",", result.Errors), ErrorCode.IdentityUserCreateError);

            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("user", "must not be null.");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", "must not be null.");

            if (!Thread.CurrentPrincipal.IsInRole(RoleNames.CreateUser) && this._skipPermission == false)
                throw new PermissionException("You have no permission to execute this operation.", RoleNames.Admin);

            if (!user.Validate())
                throw new ValidationException("Business Entity has invalid information.", user.ValidationResults, ErrorCode.InvalidData);
             
            if (!user.IsUserNameUnique(false))
            {
                BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.UserNameExisted.ToString());
                throw new BusinessRuleException("اسم المستخدم موجود مسبقاً", exception, ErrorCode.NotUnique);
            }

            user.FormatUserMobileToFullSaudiMobileNumber();

            if (!user.IsMobileUnique(false))
            {
                BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.MobileNumberExisted.ToString());
                throw new BusinessRuleException("رقم الجوال موجود مسبقاً", exception, ErrorCode.NotUnique);
            }

            if (!user.IsEmailUnique(false))
            {
                BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.EmailExisted.ToString());
                throw new BusinessRuleException("البريد الالكتروني موجود مسبقاً", exception, ErrorCode.NotUnique);
            }

            var result = await CreateAsync(user, password);

            if (result.Errors.Any())
                throw new BusinessRuleException(string.Join(",", result.Errors), ErrorCode.IdentityUserCreateError);

            return user;
        }
         
        public User FindByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName", "must not be null or empty.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Where(x => x.UserName == userName);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public User FindById(int userId)
        {
            if (userId == 0)
                throw new ArgumentNullException("userID", "must not be null.");

            var constraints = new QueryConstraints<User>()
                .IncludePath(u => u.Roles)
                .Where(x => x.Id == userId);

            return _queryRepository.SingleOrDefault(constraints);
        }

        //public User Update(User user)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException("entity", "must not be null.");

        //    //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
        //    //    throw new PermissionException("You have no permission to execute current operation.", RoleNames.ManageUsers);

        //    if (!user.Validate())
        //        throw new ValidationException("Some data are not valid", user.ValidationResults, ErrorCode.InvalidData);

        //    if (!user.IsUserNameUnique(true))
        //    {
        //        BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.UserNameExisted.ToString());
        //        throw new BusinessRuleException("اسم المستخدم موجود مسبقاً", exception, ErrorCode.NotUnique);
        //    }

        //    if (!user.IsMobileUnique(true))
        //    {
        //        BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.MobileNumberExisted.ToString());
        //        throw new BusinessRuleException("رقم الجوال موجود مسبقاً", exception, ErrorCode.NotUnique);
        //    } 

        //    if (!user.IsEmailUnique(true))
        //    {
        //        BusinessRuleException exception = new BusinessRuleException(BusinessRuleExceptionType.EmailExisted.ToString());
        //        throw new BusinessRuleException("البريد الالكتروني موجود مسبقاً", exception, ErrorCode.NotUnique);
        //    }

        //    var result = _identityUserRepository.UpdateAsync(user);

        //    if (result.Exception != null)
        //    {
        //        if (result.Exception.InnerException != null)
        //            throw new RepositoryException(result.Exception.InnerException.Message, ErrorCode.DatabaseError);
        //        else
        //            throw new RepositoryException(result.Exception.Message, ErrorCode.DatabaseError);
        //    }
        //    return FindById(user.Id);
        //}

        public IList<string> GetRoles(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return user.Roles.Select(r => r.Name).ToList();
        }

        public User SingleByMobile(String mobileNumber)
        {
            if (String.IsNullOrWhiteSpace(mobileNumber))
                throw new ArgumentNullException("Mobile number can not be empty or null");

            var constraints = new QueryConstraints<User>()
                .Where(u => u.Mobile.Equals(mobileNumber));

            return _queryRepository.Single(constraints);
        }

        public User SingleByEmail(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Email can not be empty or null");

            var constraints = new QueryConstraints<User>()
                .Where(u => u.Email.Equals(email));

            return _queryRepository.Single(constraints);
        }

        public override async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            return await base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override async Task<User> FindByIdAsync(int userId)
        {
            return await base.FindByIdAsync(userId);
        }

        public override async Task<User> FindByNameAsync(string userName)
        {
            return await base.FindByNameAsync(userName);
        }

        public override async Task<IdentityResult> AddToRoleAsync(int userId, string role)
        {
            return await base.AddToRoleAsync(userId, role);
        }

        public override Task<IdentityResult> AddToRolesAsync(int userId, params string[] roles)
        {
            var result = base.AddToRolesAsync(userId, roles);
            if (result.Exception != null)
            {
                if (result.Exception.InnerException != null)
                    throw new RepositoryException(result.Exception.InnerException.Message, ErrorCode.DatabaseError);
                else
                    throw new RepositoryException(result.Exception.Message, ErrorCode.DatabaseError);
            }
            else if (result.Result.Errors.Any())
                throw new BusinessRuleException(string.Join(",", result.Result.Errors), ErrorCode.IdentityUserCreateError);

            return result;
        }
    }

}