namespace HCrawler.Api.Repositories.Models
{
    public class Image
    {
        public Image()
        {
        }

        public Image(int id, string path, Profile profile)
        {
            Id = id;
            Path = path;
            Profile = profile;
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public Profile Profile { get; set; }
    }
}