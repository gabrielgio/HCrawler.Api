using System;
using HCrawler.Core;

namespace HCrawler.DB.Repositories.DbModel
{
    public class DbDetailedImage
    {
        public int ImageId { get; set; }

        public string ImagePath { get; set; }

        public string ImageUrl { get; set; }

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public string ProfileUrl { get; set; }

        public int SourceId { get; set; }

        public string SourceName { get; set; }

        public string SourceUrl { get; set; }

        public DateTime ImageCreatedOn { get; set; }

        public Proxies.Image ToRecord()
        {
            var detailedSource = new Proxies.Source(SourceId, SourceName, SourceUrl);
            var detailedProfile = new Proxies.Profile(ProfileId, ProfileName, ProfileUrl, detailedSource);
            return new Proxies.Image(ImageId, ImagePath, ImageCreatedOn, ImageUrl, detailedProfile);
        }
    }
}
