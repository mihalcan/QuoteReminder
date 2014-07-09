namespace QuoteReminder.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using QuoteReminder.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<QuoteReminder.Models.QuoteReminderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuoteReminder.Models.QuoteReminderContext context)
        {            
        }
    }
}
