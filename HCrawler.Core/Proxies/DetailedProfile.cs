namespace HCrawler.Core.Proxies
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
