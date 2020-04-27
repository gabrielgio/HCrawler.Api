namespace HCrawler.Core.Repositories.Models
{
    public class DetailedImage
    {
        public DetailedImage()
        {
            
        } 
        
        public DetailedImage(int id, string path, DetailedProfile profile)
        {
            Id = id;
            Path = path;
            Profile = profile;
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public DetailedProfile Profile { get; set; }
    }
}
