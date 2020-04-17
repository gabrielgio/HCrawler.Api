using System.Collections.Generic;

namespace HCrawler.Api.Repositories.Models
{
    public class Source
    {
        public Source()
        {
        }

        public Source(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}