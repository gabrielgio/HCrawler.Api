using System;
using HCrawler.Core.Repositories.Models;

namespace HCrawler.Core.Repositories
{
    public struct StoreImage
    {
        public StoreImage(int profileId, string path, string url, DateTime createdOn)
        {
            ProfileId = profileId;
            Path = path;
            Url = url;
            CreatedOn = createdOn;
        }

        public int ProfileId { get; }
        public string Path { get; }
        public string Url { get; }
        public DateTime CreatedOn { get; }

        public static StoreImage FromCreateImage(in int profileId, CreateImage createImage)
        {
            return new StoreImage(profileId,
                createImage.ImagePath, createImage.ImageUrl, createImage.CreatedOn);
        }
    }
}
