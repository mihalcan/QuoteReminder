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
            var r = new Random();

            var items = Enumerable.Range(1, 50).Select(o => 
                {
                    var created = new DateTime(2013, r.Next(1, 12), r.Next(1, 28));
                    return new Quote
                    {
                        Group = "Group " + r.Next(10),
                        Created = created,
                        LastRemind = created,
                        NextRemind = created.AddDays(1),
                        Text = "remind text " + o
                    };
                }).ToArray();
            context.Quotes.AddOrUpdate(item => new { item.Text }, items);
        }
    }
}
