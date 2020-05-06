using HCrawler.Core.Payloads;

namespace HCrawler.Core.Repositories
{
    public struct StoreSource
    {
        public StoreSource(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; }
        public string Url { get; }

        public static StoreSource FromCreateImage(CreateImage createImage)
        {
            return new StoreSource(createImage.SourceName, createImage.SourceUrl);
        }
    }
}
