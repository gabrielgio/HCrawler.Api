using System;

namespace HCrawler.Core.Proxies
{
    public class DetailedImage
    {
        public DetailedImage()
        {
        } 
        
        public DetailedImage(int id, string path, DetailedProfile profile, DateTime createdOn)
        {
            Id = id;
            Path = path;
            Profile = profile;
            CreatedOn = createdOn;
        }

        public int Id { get; set; }
        public string Path { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public DetailedProfile Profile { get; set; }
    }
}
