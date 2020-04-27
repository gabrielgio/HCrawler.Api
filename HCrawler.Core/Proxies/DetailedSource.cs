namespace HCrawler.Core.Repositories.Models
{
    public class DetailedSource
    {
        public DetailedSource()
        {
            
        }
        public DetailedSource(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }
        public string Url { get; set; }
    }
}
