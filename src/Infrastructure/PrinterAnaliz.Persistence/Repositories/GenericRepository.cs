using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Common;
using PrinterAnaliz.Persistence.SqlContext;
using System.Linq.Expressions;

namespace PrinterAnaliz.Persistence.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : EntityBase, new()
    {
        private readonly SqlDbContext dbContext; 
        protected DbSet<T> entity => dbContext.Set<T>();
        public GenericRepository(SqlDbContext _dbContext)
        {
            dbContext = _dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        #region Get Methods
        public virtual IQueryable<T> AsQueryable() => entity.AsQueryable();

        public virtual async Task<T> FindAsync(long id)
        {
            return await entity.FindAsync(id);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }
        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }
        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }
        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query.SingleOrDefault();
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<List<T>> GetAll(bool noTracking = false)
        {
            return await this.entity.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(long id, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            T found = await entity.FindAsync(id);

            if (found == null)
            {
                return null;
            }

            if (noTracking)
            {
                dbContext.Entry(found).State = EntityState.Detached;
            }

            foreach (var include in includes)
            {
                dbContext.Entry(found).Reference(include).Load();
            }

            return found;
        }

        public virtual async Task<List<T>> GetList(Expression<Func<T, bool>> predicate, bool noTracking = false, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = this.AsQueryable();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            query = ApplyIncludes(query, includes);

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            if (noTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }
        public IQueryable<T> Where()
        {
            return entity.AsQueryable();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return query;
        }
        #endregion

        #region Add Methods
        public virtual int Add(T entity)
        {
            this.entity.Add(entity);
            return dbContext.SaveChanges();
        }

        public virtual int Add(IEnumerable<T> entities)
        {
            this.entity.AddRange(entities);
            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            await this.entity.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<T> entities)
        {
            await this.entity.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region Update Methods
        public int Update(T entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            if (this.entity.Entry(entity).State == EntityState.Deleted)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);
            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region AddOrUpdate Methods
        public virtual int AddOrUpdate(T entity)
        {
            if (!this.entity.Local.Any(x => EqualityComparer<long>.Default.Equals(x.Id, entity.Id)))
            {
                dbContext.Update(entity);
            }

            return dbContext.SaveChanges();
        }

        public virtual async Task<int> AddOrUpdateAsync(T entity)
        {
            if (!this.entity.Local.Any(x => EqualityComparer<long>.Default.Equals(x.Id, entity.Id)))
            {
                dbContext.Update(entity);
            }

            return await dbContext.SaveChangesAsync();
        }
        #endregion

        #region Delete Methods
        public int Delete(long id)
        {
            var entity = this.entity.Find(id);
            if (entity == null)
                return 0;
            return Delete(entity);
        }

        public async Task<int> DeleteAsync(long id)
        {
            var entity = await this.entity.FindAsync(id);
            if (entity == null)
                return 0;
            return await DeleteAsync(entity);
        }

        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChanges() > 0;
        }

        public async Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            dbContext.RemoveRange(entity.Where(predicate));
            return await dbContext.SaveChangesAsync() > 0;
        }

        public int Delete(T entity)
        {
            if (this.entity.Entry(entity).State == EntityState.Deleted)
            {
                this.entity.Attach(entity);
            }

            this.entity.Remove(entity);
            return dbContext.SaveChanges();
        }
        #endregion

        #region SaveChanges Methods

        public async Task<int> SaveChangeAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public int SaveChange()
        {
            return dbContext.SaveChanges();
        }

        #endregion

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        #region SoftDelete Methods
        public int SoftDelete(long id)
        {
            var entity = this.entity.Find(id);
            if (entity != null)
            {
                entity.IsDeleted = true;


                dbContext.Entry(entity).State = EntityState.Modified;

                return dbContext.SaveChanges();
            }

            return 0;
        }

        public int SoftDelete(T identity)
        {
            identity.IsDeleted = true;
            dbContext.Entry(identity).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public async Task<int> SoftDeleteAsync(long id)
        {
            var entity = await this.entity.FindAsync(id);
            if (entity != null)
            {

                entity.IsDeleted = true;
                dbContext.Entry(entity).State = EntityState.Modified;

                return await dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> SoftDeleteAsync(T identity)
        {
            identity.IsDeleted = true;
            dbContext.Entry(identity).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }

        public bool SoftDeleteRange(Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = this.entity.Where(predicate);
            foreach (var entity in entitiesToDelete)
            {
                entity.IsDeleted = true;
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            return dbContext.SaveChanges() > 0;
        }

        public async Task<bool> SoftDeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            var entitiesToDelete = await this.entity.Where(predicate).ToListAsync();
            foreach (var entity in entitiesToDelete)
            {
                entity.IsDeleted = true;
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await Task.FromResult(query);
        }
        #endregion


        #region SP

        public IQueryable<T> execSpAsync(string sp)
        {
            return entity.FromSqlRaw(sp).AsNoTracking();
        }

        public IQueryable<T> execSpAsync(string sp,SqlParameter parameter) 
        {
            return entity.FromSqlRaw(sp, parameter).AsNoTracking();
        }
        public IQueryable<T> execSpAsync(string sp, SqlParameter[] parameter)
        {
            
           return entity.FromSqlRaw(sp, parameter).AsNoTracking();
        }
        #endregion

    }
}
