using System;

namespace HCrawler.Core
{
    public class PageFilter
    {
        public PageFilter()
        {
            Size = 30;
        }

        public int Size { get; set; }

        public DateTime? Checkpoint { get; set; }

        public string Name { get; set; }
    }
}
