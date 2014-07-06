namespace QuoteReminder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        QuoteId = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        Text = c.String(),
                        Created = c.DateTime(nullable: false),
                        LastRemind = c.DateTime(nullable: false),
                        NextRemind = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.QuoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Quotes");
        }
    }
}
