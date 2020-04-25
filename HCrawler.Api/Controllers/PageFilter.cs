using System.Drawing;
using Microsoft.AspNetCore.Identity;

namespace HCrawler.Api.Controllers
{
    public class PageFilter
    {
        public PageFilter()
        {
            Size = 10;
            Number = 0;
        }

        public int Size { get; set; }

        public int Number { get; set; }
    }
}
