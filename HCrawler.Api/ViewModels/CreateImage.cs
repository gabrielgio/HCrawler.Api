using System;
using HCrawler.Core;

namespace HCrawler.Api.ViewModels
{
    public class CreateImage
    {
        public string ImagePath { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }

        public HCrawler.Core.Payloads.CreateImage ToRecord()
        {
            return new Payloads.CreateImage(ImagePath, ImageUrl, CreatedOn, ProfileName, ProfileUrl, SourceName,
                SourceUrl);
        }
    }
}
