namespace marmuz_site_v1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BanEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BanEmails");
        }
    }
}
