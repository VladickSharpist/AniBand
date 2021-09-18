using System;
using System.Collections.Generic;
using System.Linq;

namespace AniBand.Core.Infrastructure.Helpers.Generic
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        
        public int TotalPages { get; private set; }
        
        public int PageSize { get; private set; }
        
        public int TotalCount { get; private set; }
        
        public bool HasPrevious => CurrentPage > 1;
        
        public bool HasNext => CurrentPage < TotalPages;
        
        public PagedList(
            IEnumerable<T> items, 
            int count, 
            int pageNumber, 
            int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = pageSize != default 
                ? (int)Math.Ceiling(count / (double)pageSize)
                : default;
            AddRange(items);
        }
        
        public static PagedList<T> ToPagedList(
            IEnumerable<T> source,
            int pageNumber = default, 
            int pageSize = default)
        {
            var count = source.Count();
            if (pageNumber != default
                && pageSize != default)
            {
                var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            
            return new PagedList<T>(source, count, pageNumber, pageSize);
        }
    }
}