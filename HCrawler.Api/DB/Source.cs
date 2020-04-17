using System.Collections.Generic;
using System.Globalization;
using System.Security.Permissions;

namespace HCrawler.Api.DB
{
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<Profile> Profiles { get; set; }
    }
}