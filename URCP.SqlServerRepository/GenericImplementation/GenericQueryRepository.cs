using URCP.Core;
using URCP.GraphDiff;
using URCP.RepositoryInterface;
using URCP.RepositoryInterface.Queries;
using URCP.SqlServerRepository.Queries;
using System;
using System.Linq;

namespace URCP.SqlServerRepository
{
    public class GenericQueryRepository : IGenericQueryRepository
    {
        protected MyDbContext _context;

        public GenericQueryRepository() { }

        public GenericQueryRepository(MyDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException("Context argument cannot be null in UnitOfWork.");
        }

        #region IGenericQueryRepository Members

        public TEntity Single<TEntity>(int id)
            where TEntity : class
        {
            try
            { 
                TEntity result = _context.Set<TEntity>().Find(id);
                 
                return result;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        /// <summary>
        /// Returns single BusinessObject of table that satisfies a specified condition or a null if no such element is found.
        /// </summary>
        /// <typeparam name="TEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The BusinessObject the satisfy the predicate.</returns>
        public TEntity Single<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class
        {
            try
            { 
                TEntity result = _context.LoadAggregate(constraints.Predicate); 
                return result;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public TEntity SingleOrDefault<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class
        {

            try
            { 
                TEntity result = _context.Set<TEntity>()
                    .ToSearchResult<TEntity>(constraints)
                    .Items
                    .FirstOrDefault();
                 
                return result;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        /// <summary>
        /// Returns the number of elements in a BusinessObject's table.
        /// </summary>
        /// <typeparam name="T">The type of BusinessObject of source table.</typeparam>
        /// <returns>The number of elements in the input sequence.</returns>
        public int GetCount<T>()
            where T : class
        {
            try
            { 
                int result = _context.Set<T>().Count();
                 
                return result;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        /// <summary>
        /// Returns the number of elements of the specified T BusinessEntity that satisfies a condition.
        /// </summary>
        /// <typeparam name="TEntity">The type of BusinessObject of source table.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of elements in the sequence that satisfies the condition in the predicate function.</returns>
        public int GetCount<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class
        {
            try
            { 
                int result = _context.Set<TEntity>().Count(constraints.Predicate);
                 
                return result;
            }
            catch (Exception ex)
            {
                throw ThrowHelper.ReThrow(ex);
            }
        }

        public IQueryResult<TEntity> Find<TEntity>(IQueryConstraints<TEntity> constraints)
            where TEntity : class
        { 
            var result = _context.Set<TEntity>().ToSearchResult<TEntity>(constraints);
             
            return result;
        }

        #endregion

    }
}
