namespace HCrawler.Core.Repositories.Models
{
    public class Profile
    {
        public Profile(string name, string url, Source source)
        {
            Name = name;
            Url = url;
            Source = source;
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public Source Source { get; set; }
    }
}