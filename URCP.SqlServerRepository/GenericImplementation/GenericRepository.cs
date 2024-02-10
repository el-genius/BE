using URCP.GraphDiff;
using URCP.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace URCP.SqlServerRepository
{
    public class GenericRepository : IGenericRepository
    {
        protected MyDbContext _context;


        public GenericRepository(MyDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException("Context argument cannot be null in UnitOfWork.");
        }

        #region IGenericRepository Members

        public virtual TBizEntity Create<TBizEntity>(TBizEntity entity) where TBizEntity : class, new()
        {
            try
            {
                TBizEntity updatedEntity = _context.UpdateGraph<TBizEntity>(entity);
                String dump = _context.DumpTrackedEntities();

                return updatedEntity;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }
         

        public virtual TBizEntity Update<TBizEntity>(TBizEntity entity) where TBizEntity : class, new()
        {
            try
            {
                TBizEntity updatedEntity = _context.UpdateGraph<TBizEntity>(entity);
                string dump = _context.DumpTrackedEntities();

                return updatedEntity;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public virtual T SetDetachedState<T>(T entity) where T : class, new()
        {
            try
            {
                _context.Entry(entity).State = EntityState.Detached;
                return entity;
            }
            catch (Exception e)
            {
                throw ThrowHelper.ReThrow(e);
            }
        }
          
        public virtual void Delete<T>(T entity) where T : class, new()
        { 
            try
            {
                _context.Set<T>().Attach(entity);
                _context.Set<T>().Remove(entity); 
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }
         
        public void Delete<T>(IEnumerable<T> entities) where T : class, new()
        { 
            try
            {
                _context.Set<T>().RemoveRange(entities); 
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }
         

        #endregion
    }
}
