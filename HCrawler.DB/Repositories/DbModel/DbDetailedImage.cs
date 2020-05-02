using System;
using HCrawler.Core.Repositories.Models;

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


        public DateTime ImageCreatedOn { get; set; }

        public DetailedImage ToDetailedImage()
        {
            return new DetailedImage
            {
                Id = ImageId,
                Path = ImagePath,
                CreatedOn = ImageCreatedOn,
                Profile = new DetailedProfile
                {
                    Name = ProfileName,
                    Url = ProfileUrl,
                    DetailedSource = new DetailedSource
                    {
                        Name = SourceName,
                        Url = SourceUrl
                    }
                }
            };
        }
    }
}
