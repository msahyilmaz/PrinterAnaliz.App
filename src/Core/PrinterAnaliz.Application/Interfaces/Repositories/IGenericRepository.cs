using Microsoft.Data.SqlClient;
using PrinterAnaliz.Domain.Common;
using System.Linq.Expressions;

namespace PrinterAnaliz.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : EntityBase, new()
    {

        #region Add
        Task<int> AddAsync(T entity);
        Task<int> AddAsync(IEnumerable<T> entities);
        int Add(T entity);
        int Add(IEnumerable<T> entities);
        #endregion
        #region AddOrUpdate
        Task<int> AddOrUpdateAsync(T entity);
        int AddOrUpdate(T entity);
        #endregion
        #region Update
        Task<int> UpdateAsync(T entity);
        int Update(T entity);

        #endregion
        #region Delete

        Task<int> DeleteAsync(long id);
        Task<int> DeleteAsync(T identity);
        int Delete(long id);
        int Delete(T identity);
        bool DeleteRange(Expression<Func<T, bool>> predicate);
        Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region SoftDelete
        Task<int> SoftDeleteAsync(long id);
        Task<int> SoftDeleteAsync(T identity);
        int SoftDelete(long id);
        int SoftDelete(T identity);
        bool SoftDeleteRange(Expression<Func<T, bool>> predicate);
        Task<bool> SoftDeleteRangeAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region Get
        Task<T> FindAsync(long id);
        Task<List<T>> GetAll(bool noTracking = false);
        Task<List<T>> GetList(Expression<Func<T, bool>> predicate, bool noTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(long id, bool noTracking = true, params Expression<Func<T, object>>[] include);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        T SingleOrDefault(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Where();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes);
        #endregion
        int SaveChange();
        Task<int> SaveChangeAsync();

        IQueryable<T> AsQueryable();

        IQueryable<T> execSpAsync(string sp);
        IQueryable<T> execSpAsync(string sp, SqlParameter parameter);
        IQueryable<T> execSpAsync(string sp, SqlParameter[] parameter);

    }
}
