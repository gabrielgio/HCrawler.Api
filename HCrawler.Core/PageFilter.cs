namespace HCrawler.Api.DB.Utils
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