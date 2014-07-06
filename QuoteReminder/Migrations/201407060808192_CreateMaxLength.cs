namespace QuoteReminder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Quotes", "Group", c => c.String(maxLength: 100));
            AlterColumn("dbo.Quotes", "Text", c => c.String(maxLength: 400));

            CreateIndex("dbo.Quotes", "NextRemind");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quotes", "Text", c => c.String());
            AlterColumn("dbo.Quotes", "Group", c => c.String());
        }
    }
}
