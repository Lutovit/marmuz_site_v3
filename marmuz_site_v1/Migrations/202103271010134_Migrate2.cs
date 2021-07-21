namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropColumn("dbo.Comments", "ApplicationUserId");
            RenameColumn(table: "dbo.Comments", name: "User_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Comments", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Comments", "ApplicationUserId", c => c.Int());
            RenameColumn(table: "dbo.Comments", name: "ApplicationUserId", newName: "User_Id");
            AddColumn("dbo.Comments", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.Comments", "User_Id");
        }
    }
}
