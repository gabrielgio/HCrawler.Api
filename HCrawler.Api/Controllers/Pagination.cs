using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HCrawler.Api.Controllers
{
    public static class Pagination
    {
        public static Page<T> Paginate<T>(this IEnumerable<T> source, PageFilter pageFilter)
        {
            var count = source.Count();
            var pages = count / pageFilter.Size;
            var enumerable = source
                .Skip(pageFilter.Number * pageFilter.Size)
                .Take(pageFilter.Size);
            return new Page<T>(count, enumerable, pages);
        }
    }
}
