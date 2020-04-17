using System;

namespace HCrawler.Api.DB
{
    public class Image
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }

        public Profile Profile { get; set; }
    }
}