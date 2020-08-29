using FluentMigrator;

namespace HCrawler.DB.Migrations
{
    [Migration(202005080008, "add image CreatedOn index")]
    public class AddImageIndex : Migration

    {
        public override void Up()
        {
            Create.Index("images_createdon_index")
                .OnTable("Images")
                .OnColumn("CreatedOn")
                .Descending();
        }

        public override void Down()
        {
            Delete.Index("images_createdon_index");
        }
    }
}
