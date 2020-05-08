using System;
using HCrawler.Core;

namespace HCrawler.DB.Repositories.DbModel
{
    public class DbDetailedImage
    {
        public int ImageId { get; set; }

        public string ImagePath { get; set; }

        public string ProfileName { get; set; }

        public string ProfileUrl { get; set; }

        public string SourceName { get; set; }

        public string SourceUrl { get; set; }

        public string ImageUrl { get; set; }

        public DateTime ImageCreatedOn { get; set; }

        public Proxies.DetailedImage ToDetailedImage()
        {
            var detailedSource = new Proxies.DetailedSource(SourceName, SourceUrl);
            var detailedProfile = new Proxies.DetailedProfile(ProfileName, ProfileUrl, detailedSource);
            return new Proxies.DetailedImage(ImageId, ImagePath, ImageCreatedOn, ImageUrl,detailedProfile);
        }
    }
}
