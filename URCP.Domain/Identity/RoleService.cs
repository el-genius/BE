using URCP.Core;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using URCP.Domain.Interface;

namespace URCP.Domain
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository _repository;
        private readonly IGenericQueryRepository _queryRepository;
        private readonly IIdentityRoleRepository _roleRepository;

        public RoleService(IGenericRepository repository, IGenericQueryRepository queryRepository,
            IIdentityRoleRepository roleRepository)
        {
            this._repository = repository;
            this._queryRepository = queryRepository;
            this._roleRepository = roleRepository;
        }


        public List<IdentityRole> GetByNames(string[] roleNames)
        {
            if (roleNames == null)
                return new List<IdentityRole>();

            var constraints = new QueryConstraints<IdentityRole>()
                .Page(1, int.MaxValue)
                .SortBy("Name")
                .Where(x => roleNames.Contains(x.Name));

            return _queryRepository.Find(constraints).Items.ToList();
        }

        public IdentityRole Create(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.SuperAdministrator))
            //    throw new PermissionException("You have no permission to execute this operation.", RoleNames.SuperAdministrator, ErrorCode.NotAuthorized);

            return _repository.Create<IdentityRole>(role);
        }

        public IdentityRole FindById(int roleId)
        {
            if (roleId == 0)
                throw new ArgumentNullException("roleId", "must not be null.");

            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Id == roleId);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public IQueryResult<IdentityRole> FindByIds(List<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids", "must not be null.");

            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => ids.Contains(x.Id.ToString()));

            return _queryRepository.Find(constraints);
        }

        public IdentityRole FindByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName", "must not be null or empty.");

            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Name == roleName);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public IdentityRole Update(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role", "must not be null.");

            //if (!Thread.CurrentPrincipal.IsInRole(RoleNames.ManageUsers))
            //    throw new PermissionException("You have no permission to execute current operation.", RoleNames.ManageUsers);

            return _repository.Update<IdentityRole>(role);
        }
    }
}
