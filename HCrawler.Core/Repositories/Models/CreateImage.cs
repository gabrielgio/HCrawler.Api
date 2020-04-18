using System;

namespace HCrawler.Core.Repositories.Models
{
    public class CreateImage
    {
        public string Id { get; set; }        
        
        public string PostUrl { get; set; }
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}