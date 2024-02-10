using URCP.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URCP.SqlServerRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        protected MyDbContext _context;

        public UnitOfWork(MyDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException("Context argument cannot be null in UnitOfWork.");
        }
        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _context = null;
        }
    }
}
