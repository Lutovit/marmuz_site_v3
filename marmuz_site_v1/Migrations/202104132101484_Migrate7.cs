namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "isEdited", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "isEdited");
        }
    }
}
