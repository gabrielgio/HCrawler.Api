using System.Collections;
using System.Collections.Generic;

namespace HCrawler.Api.Controllers
{
    public struct Page<T>
    {
        public Page(int count, IEnumerable<T> result, int pageCount)
        {
            Count = count;
            Result = result;
            PageCount = pageCount;
        }

        public int Count { get; }

        public IEnumerable<T> Result { get; }

        public int PageCount { get; }
    }
}
