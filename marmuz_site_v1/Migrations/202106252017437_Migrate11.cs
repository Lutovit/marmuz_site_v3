namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Ban", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Ban", c => c.Boolean(nullable: false));
        }
    }
}
