using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using HCrawler.Core;
using Microsoft.AspNetCore.WebUtilities;

namespace HCrawler.Api.ViewModels
{
    public class Page
    {
        private const string _url = "/";

        public Page(IEnumerable<Proxies.Image> results, DateTime? previous, int? profile, int? source)
        {
            Results = results;
            Previous = previous;
            Profile = profile;
            Source = source;
        }

        public DateTime? Previous { get; }

        public int? Profile { get; }

        public int? Source { get; }

        public IEnumerable<Proxies.Image> Results { get; }

        public string GetProfileLink()
        {
            var param = new Dictionary<string, string>();

            param.Add("profile", Results.FirstOrDefault()?.DetailedProfile.Id.ToString() ?? "");
            
            return QueryHelpers.AddQueryString(_url, param);
        }

        public string GetNextLink()
        {
            var param = new Dictionary<string, string>();
            var next = Results.LastOrDefault()?.CreatedOn;

            param.Add("checkpoint", next.ToString());

            if (Profile is object)
                param.Add("profile", Profile.ToString());

            if (Source is object)
                param.Add("source", Source.ToString());

            return QueryHelpers.AddQueryString(_url, param);
        }
    }
}
