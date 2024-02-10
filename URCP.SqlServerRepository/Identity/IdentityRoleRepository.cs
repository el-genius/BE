using URCP.Core;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using URCP.SqlServerRepository;
using System.Threading.Tasks;
using System;

namespace URCP.SqlServerRepository
{
    public class IdentityRoleRepository : IIdentityRoleRepository
    {
        private MyDbContext _context;
        private IGenericRepository _repository;
        private IGenericQueryRepository _queryRepository;

        public IdentityRoleRepository(MyDbContext context, IGenericRepository repository, IGenericQueryRepository queryRepository)
        {
            this._context = context ?? throw new ArgumentNullException("DbContext cannot be null.");
            this._repository = repository;
            this._queryRepository = queryRepository;
        }

        #region IRoleStore
        public async Task CreateAsync(IdentityRole role)
        {
            _repository.Create(role); 
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            _repository.Update(role); 

        }
        public async Task DeleteAsync(IdentityRole role)
        {
            _repository.Delete(role);
            await this._context.SaveChangesAsync();
        }
         

        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            return Task.FromResult(FindById(roleId));
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return Task.FromResult(FindByName(roleName));
        }

        public void Dispose()
        {
        }


        #endregion

        #region IRoleStore Core
        public IdentityRole Create(IdentityRole role)
        {
            return _repository.Create<IdentityRole>(role);
        }

        public void Delete(IdentityRole role)
        {
            _repository.Delete<IdentityRole>(role);
        }

        public IdentityRole FindById(int roleId)
        {
            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Id == roleId);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public IdentityRole FindByName(string roleName)
        {
            var constraints = new QueryConstraints<IdentityRole>()
                .Where(x => x.Name == roleName);

            return _queryRepository.SingleOrDefault(constraints);
        }

        public IdentityRole Update(IdentityRole role)
        {
            return _repository.Update<IdentityRole>(role);
        }
        #endregion

    }
}
