namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Ban", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Ban", c => c.Int(nullable: false));
        }
    }
}
