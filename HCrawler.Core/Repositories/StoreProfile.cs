using HCrawler.Core.Repositories.Models;

namespace HCrawler.Core.Repositories
{
    public struct StoreProfile
    {
        public StoreProfile(int sourceId, string name, string url)
        {
            SourceId = sourceId;
            Name = name;
            Url = url;
        }

        public int SourceId { get; }
        public string Name { get; }
        public string Url { get; }

        public static StoreProfile FromCreatImage(int sourceId, CreateImage createImage)
        {
            return new StoreProfile(sourceId, createImage.ProfileName, createImage.ProfileUrl);
        }
    }
}
