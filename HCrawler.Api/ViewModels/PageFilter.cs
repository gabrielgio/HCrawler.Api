using System;
using HCrawler.Core;

namespace HCrawler.Api.ViewModels
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

        public HCrawler.Core.Payloads.PageFilter ToRecord()
        {
            return new Payloads.PageFilter(Size, Checkpoint, Name);
        }
    }
}
