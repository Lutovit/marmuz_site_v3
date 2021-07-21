namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Text", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Text", c => c.String());
        }
    }
}
