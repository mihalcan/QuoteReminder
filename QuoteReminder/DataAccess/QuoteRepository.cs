using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuoteReminder.Models;

namespace QuoteReminder.DataAccess
{
    public class QuoteRepository : IQuoteRepository, IDisposable
    {
        private QuoteReminderContext db = new QuoteReminderContext();

        public IEnumerable<Models.Quote> Get()
        {
            throw new NotImplementedException();
        }

        public Models.Quote GetById(int id)
        {
            return this.db.Quotes.Find(id);
        }

        public void Add(Models.Quote quote)
        {
            if (quote.Created.Year < 2000)
            {
                quote.Created = DateTime.Now;
            }
            quote.LastRemind = quote.Created;
            quote.NextRemind = quote.Created.AddDays(1);

            db.Quotes.Add(quote);
            db.SaveChanges();
        }

        public void Update(Models.Quote quote)
        {
            throw new NotImplementedException();
        }


        public void Delete(int id)
        {
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                throw new ArgumentException(string.Format("Item with id={0} cannot be found", id), "id"); 
            }

            db.Quotes.Remove(quote);
            db.SaveChanges();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}