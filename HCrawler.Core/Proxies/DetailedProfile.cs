namespace HCrawler.Core.Repositories.Models
{
    public class DetailedProfile
    {
        public DetailedProfile()
        {
            
        }
        public DetailedProfile(string name, string url, DetailedSource detailedSource)
        {
            Name = name;
            Url = url;
            DetailedSource = detailedSource;
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public DetailedSource DetailedSource { get; set; }
    }
}
