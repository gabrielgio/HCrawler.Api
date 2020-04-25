using System.Collections.Generic;
using System.Linq;

namespace HCrawler.Api.DB.Utils
{
    public static class Pagination
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, PageFilter pageFilter)
        {
            return source
                .Skip(pageFilter.Number * pageFilter.Size)
                .Take(pageFilter.Size);
        }
    }
}
