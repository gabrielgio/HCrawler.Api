using System;

namespace HCrawler.Core.Payloads
{
    public class CreateImage
    {
        public string ImagePath { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
    }
}
