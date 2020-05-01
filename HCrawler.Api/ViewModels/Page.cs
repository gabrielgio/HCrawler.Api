using System.Collections.Generic;

namespace HCrawler.Api.ViewModels
{
    public class Page<T>
    {
        public Page(IEnumerable<T> results, int previous, int next, string name)
        {
            Results = results;
            Previous = previous;
            Next = next;
            Name = name;
        }

        public int Next { get; set; }

        public int Previous { get; set; }

        public string Name { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}
