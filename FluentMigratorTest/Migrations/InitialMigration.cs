using FluentMigrator;

namespace FluentMigratorTest.Migrations
{
    [Migration(20190418205440, "v1.0.0")] 
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            #region Tables

            Create.Table("Players")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Username").AsString(50).NotNullable();

            Create.Table("Cards")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Number").AsString(100).NotNullable()
                .WithColumn("PlayerId").AsInt32().Nullable();

            Create.Table("Purchases")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Amount").AsDecimal(9, 2).NotNullable()
                .WithColumn("CardId").AsInt32().NotNullable()
                .WithColumn("PlayerId").AsInt32().NotNullable();
            #endregion

            #region Foreign Keys
            Create.ForeignKey("FK_Cards_PlayerId")
                .FromTable("Cards").ForeignColumn("PlayerId")
                .ToTable("Players").PrimaryColumn("Id");

            Create.ForeignKey("FK_Purchases_CardId")
                .FromTable("Purchases").ForeignColumn("CardId")
                .ToTable("Cards").PrimaryColumn("Id");

            Create.ForeignKey("FK_Purchases_PlayerId")
                .FromTable("Purchases").ForeignColumn("PlayerId")
                .ToTable("Players").PrimaryColumn("Id");
            #endregion

            #region Initial Data

            //Insert.IntoTable("Players")
            //    .Row(new { Id = 1, Username = "Player1" })
            //    .Row(new { Id = 2, Username = "Playe2" });
            #endregion

        }
        public override void Down()
        {
            #region Foreign Keys

            Delete.ForeignKey("FK_Cards_PlayerId").OnTable("Cards");

            Delete.ForeignKey("FK_Purchases_CardId").OnTable("Purchases");
            Delete.ForeignKey("FK_Purchases_PlayerId").OnTable("Purchases");

            #endregion

            #region Tables
            Delete.Table("Players");
            Delete.Table("Cards");
            Delete.Table("Purchases");
            #endregion

        }
    }
}
