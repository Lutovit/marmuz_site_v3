namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ban", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ban");
        }
    }
}
