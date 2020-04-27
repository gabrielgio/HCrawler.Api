using FluentMigrator;

namespace HCrawler.DB.Migrations
{
    [Migration(202004261749, "add table image")]
    public class AddImage : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Sources").Exists())
            {
                Create.Table("Sources")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("Name").AsString().Unique()
                    .WithColumn("Url").AsString();
            }

            if (!Schema.Table("Profiles").Exists())
            {
                Create.Table("Profiles")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("SourceId").AsInt64()
                    .WithColumn("Name").AsString().Unique()
                    .WithColumn("Url").AsString().Unique();

                Create.ForeignKey()
                    .FromTable("Profiles").ForeignColumn("SourceId")
                    .ToTable("Sources").PrimaryColumn("Id");
            }

            if (!Schema.Table("Images").Exists())
            {
                Create.Table("Images")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("Path").AsString().Unique()
                    .WithColumn("Url").AsString().Unique()
                    .WithColumn("ProfileId").AsInt64();

                Create.ForeignKey()
                    .FromTable("Images").ForeignColumn("ProfileId")
                    .ToTable("Profiles").PrimaryColumn("Id");
            }

            if (Schema.Table("__EFMigrationsHistory").Exists())
            {
                Delete.Table("__EFMigrationsHistory");
            }
        }
        
        private void CreateableIfExists(string schemaName, string tableName)
        {
            this.Execute.Sql($"DROP TABLE IF EXISTS [{schemaName}].[{tableName}];");
        }

        public override void Down()
        {
            Delete.Table("Images");
            Delete.Table("Sources");
            Delete.Table("Profiles");
        }
    }
}
