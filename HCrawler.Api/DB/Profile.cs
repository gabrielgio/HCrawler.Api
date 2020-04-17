using System.Collections.Generic;

namespace HCrawler.Api.DB
{
    public class Profile
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public Source Source { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}