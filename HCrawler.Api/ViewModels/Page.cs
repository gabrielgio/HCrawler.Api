using System;
using System.Collections.Generic;

namespace HCrawler.Api.ViewModels
{
    public class Page<T>
    {
        public Page(IEnumerable<T> results, DateTime? previous, DateTime? next, string name)
        {
            Results = results;
            Previous = previous;
            Next = next;
            Name = name;
        }

        public DateTime? Next { get; set; }

        public DateTime? Previous { get; set; }

        public string Name { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}
