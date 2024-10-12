using ELearningApp.Core.Assists;
using ELearningApp.Service.DB.DataHelper.Assists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Service.DB.DataHelper
{
    public interface IDataHelper<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllNoTrackingAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T?> GetWithIncludesAsync(string id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetWithIncludesAsync(string id, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task AddRangeAsync(IEnumerable<T> entities);
        void AddWithoutSave(T entity);
        void UpdateWithoutSave(T entity);
        Task SaveChangesAsync();
        Task<IEnumerable<T>> GetWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWithIncludesAsync(Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder);
        Task LoadRelatedDataAsync<TProperty>(T entity, Expression<Func<T, TProperty>> navigationPropertyPath) where TProperty : class;
        Task LoadRelatedDataAsync<TProperty>(T entity, bool isList, Expression<Func<T, IEnumerable<TProperty>>> navigationPropertyPath) where TProperty : class;
        Task<PaginatedList<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PaginatedList<T>> GetPagedWithIncludesAsync(int pageNumber, int pageSize, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        Task<PaginatedList<T>> SearchPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate);
        Task<PaginatedList<T>> SearchPagedWithIncludesAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder);
        Task<PaginatedList<T>> SearchPagedWithIncludesInOrderAsync<TKey>(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate, Func<QueryBuilder<T>, QueryBuilder<T>> queryBuilder, bool IsDESC, Func<T, TKey> order);
        IEnumerable<dynamic> GetDistinctColumnValues(Func<T, dynamic> coulmns);
        Task<int> GetSequenceValue(string sequenceName);
    }
}
