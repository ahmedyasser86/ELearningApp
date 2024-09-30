using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Assists
{
    public class PaginatedList<T>(List<T> items, int count, int pageNumber, int pageSize)
    {
        public List<T> Items { get; private set; } = items;
        public int TotalRecords { get; private set; } = count;
        public int PageNumber { get; private set; } = pageNumber;
        public int PageSize { get; private set; } = pageSize;
        public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
