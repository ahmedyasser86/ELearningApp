using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Service.DB.DataHelper.Assists
{
    public class QueryBuilder<T>(IQueryable<T> query) where T : class
    {
        private IQueryable<T> _query = query;

        public QueryBuilder<T> Include(Expression<Func<T, object>> include)
        {
            _query = _query.Include(include);
            return this;
        }

        public QueryBuilder<T> ThenInclude(Expression<Func<object, object>> thenInclude)
        {
            _query = (_query as IIncludableQueryable<T, object>).ThenInclude(thenInclude);
            return this;
        }

        public IQueryable<T> Build()
        {
            var queryString = _query.ToQueryString();
            return _query;
        }
    }
}
