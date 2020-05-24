using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace HCrawler.Api.ViewModels
{
    public class Page<T>
    {
        public Page(IEnumerable<T> results, DateTime? previous, DateTime? next, string name, string source)
        {
            Results = results;
            Previous = previous;
            Next = next;
            Name = name;
            Source = source;
        }

        public DateTime? Next { get; set; }

        public DateTime? Previous { get; set; }

        public string Name { get; set; }


        public string Source { get; set; }

        public IEnumerable<T> Results { get; set; }

        public string GetNextLink()
        {
           const string url = "/";
           var param = new Dictionary<string, string>();
           
           param.Add("checkpoint", Next.ToString());
           
           if (Name is object)
              param.Add("name", Name); 
           
           if (Source is object)
               param.Add("source", Source);
           
           return QueryHelpers.AddQueryString(url, param);
        }
        

    }
}
