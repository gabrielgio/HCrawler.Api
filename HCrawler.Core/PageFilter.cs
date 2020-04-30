namespace HCrawler.Api.DB.Utils
{
    public class PageFilter
    {
        public PageFilter()
        {
            Size = 20;
            Number = 0;
        }

        public int Size { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }
    }
}
