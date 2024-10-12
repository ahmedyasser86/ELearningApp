using ELearningApp.Core.Assists;
using ELearningApp.Service.DB.DataHelper.Assists;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Service.DB.DataHelper
{
    public class DataHelper<T>(ApplicationDbContext context) : IDataHelper<T> where T : class
    {
        private readonly ApplicationDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllNoTrackingAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            // الحصول على معلومات المفتاح الأساسي
            var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
            var keyType = keyProperty.ClrType;

            if (keyType == typeof(int))
            {
                return await _dbSet.FindAsync(Convert.ToInt32(id));
            }
            else if (keyType == typeof(string))
            {
                return await _dbSet.FindAsync(id);
            }
            else
            {
                throw new Exception("Invalid Key Type");
            }

        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            // الحصول على معلومات المفتاح الأساسي
            var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
            var keyType = keyProperty.ClrType;

            T? entity;
            if (keyType == typeof(int))
            {
                entity = await _dbSet.FindAsync(Convert.ToInt32(id));
            }
            else if (keyType == typeof(string))
            {
                entity = await _dbSet.FindAsync(id);
            }
            else
            {
                throw new Exception("Invalid Key Type");
            }

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No Element Found with this Id");
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void AddWithoutSave(T entity) => _dbSet.Add(entity);

        public void UpdateWithoutSave(T entity) => _dbSet.Update(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IEnumerable<T>> GetWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {

                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetWithIncludesAsync(string id, params Expression<Func<T, object>>[] includes)
        {
            // الحصول على معلومات المفتاح الأساسي
            var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
            var keyName = keyProperty.Name;
            var keyType = keyProperty.ClrType;

            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (keyType == typeof(int))
            {
                return await query.SingleOrDefaultAsync(m => EF.Property<int>(m, keyName) == Convert.ToInt32(id));
            }
            else if (keyType == typeof(string))
            {
                return await query.SingleOrDefaultAsync(m => EF.Property<string>(m, keyName) == id.ToString());
            }
            else
            {
                throw new Exception("Invalid Key Type");
            }
        }
        public async Task<IEnumerable<T>> GetWithIncludesAsync(Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder)
        {
            var query = queryBuilder(new QueryBuilder<T>(_dbSet)).Build();
            return await query.ToListAsync();
        }

        public async Task<T?> GetWithIncludesAsync(string id, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder)
        {
            // الحصول على معلومات المفتاح الأساسي
            var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
            var keyName = keyProperty.Name;
            var keyType = keyProperty.ClrType;

            IQueryable<T> query;

            if (keyType == typeof(int))
            {
                query = queryBuilder(new QueryBuilder<T>(_dbSet.Where(m => EF.Property<int>(m, keyName) == Convert.ToInt32(id)))).Build();
            }
            else if (keyType == typeof(string))
            {
                query = queryBuilder(new QueryBuilder<T>(_dbSet.Where(m => EF.Property<string>(m, keyName) == id.ToString()))).Build();
            }
            else
            {
                throw new Exception("Invalid Key Type");
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task LoadRelatedDataAsync<TProperty>(T entity, Expression<Func<T, TProperty>> navigationPropertyPath) where TProperty : class
        {
            await _context.Entry(entity).Reference(navigationPropertyPath).LoadAsync();
        }

        public async Task LoadRelatedDataAsync<TProperty>(T entity, bool isList, Expression<Func<T, IEnumerable<TProperty>>> navigationPropertyPath) where TProperty : class
        {
            await _context.Entry(entity).Collection(navigationPropertyPath).LoadAsync();
        }

        public async Task<PaginatedList<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, totalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedList<T>> GetPagedWithIncludesAsync(int pageNumber, int pageSize, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder)
        {
            var totalRecords = await _dbSet.CountAsync();

            var query = queryBuilder(new QueryBuilder<T>(_dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize))).Build();
            var items = await query.ToListAsync();

            return new PaginatedList<T>(items, totalRecords, pageNumber, pageSize);
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<PaginatedList<T>> SearchPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
        {
            var totalRecords = await _dbSet.Where(predicate).CountAsync();
            var items = await _dbSet.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, totalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedList<T>> SearchPagedWithIncludesAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder)
        {
            var totalRecords = await _dbSet.Where(predicate).CountAsync();

            var query = queryBuilder(new QueryBuilder<T>(_dbSet.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize))).Build();
            var items = await query.ToListAsync();

            return new PaginatedList<T>(items, totalRecords, pageNumber, pageSize);
        }

        public IEnumerable<dynamic> GetDistinctColumnValues(Func<T, dynamic> coulmns)
        {
            return _dbSet.Distinct().Select(coulmns).ToList();
        }

        public async Task<int> GetSequenceValue(string sequenceName)
        {
            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
            p.Direction = System.Data.ParameterDirection.Output;
            context.Database.ExecuteSqlRaw($"set @result = next value for {sequenceName}", p);
            return (int)p.Value;
        }

        public async Task<PaginatedList<T>> SearchPagedWithIncludesInOrderAsync<TKey>(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder, bool IsDESC, Func<T, TKey> order)
        {
            var totalRecords = await _dbSet.Where(predicate).CountAsync();

            IQueryable<T> query = queryBuilder(new QueryBuilder<T>(_dbSet.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize))).Build();
            List<T> items;
            if (IsDESC)
            {
                items = query.OrderByDescending(order).ToList();
            }
            else
            {
                items = query.OrderBy(order).ToList();
            }

            return new PaginatedList<T>(items, totalRecords, pageNumber, pageSize);
        }
    }
}
