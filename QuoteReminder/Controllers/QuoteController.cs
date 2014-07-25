using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using QuoteReminder.Models;

namespace QuoteReminder.Controllers
{
    public class QuoteController : ApiController
    {
        private QuoteReminderContext db = new QuoteReminderContext();

        // GET api/Quote
        public IEnumerable<Quote> GetQuotes(string q = null, string sort = null, bool desc = false,
                                                        int? limit = null, int offset = 0)
        {
            var list = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<Quote>();

            IQueryable<Quote> items = string.IsNullOrEmpty(sort) ? list.OrderBy(o => o.Created)
                : list.OrderBy(String.Format("it.{0} {1}", sort, desc ? "DESC" : "ASC"));
            if (q == null || q != "All")
            {
                var dateToCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                items = items.Where(x => x.NextRemind <= dateToCompare);
            }
            
            if (offset > 0) items = items.Skip(offset);
            if (limit.HasValue) items = items.Take(limit.Value);
            return items;
        }

        // GET api/Quote/5
        public Quote GetQuote(int id)
        {
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return quote;
        }

        // PUT api/Quote/5
        public HttpResponseMessage PutQuote(int id, Quote quote)
        {
            if (ModelState.IsValid && id == quote.QuoteId)
            {
                if (quote.Repeated)
                {
                    quote = this.UpdateRemindDate(quote.QuoteId);
                }
                db.Entry(quote).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private Quote UpdateRemindDate(int quoteId)
        {
            var quote = this.GetQuote(quoteId);
            var daysBetweenReminders = (quote.NextRemind - quote.LastRemind).Days;

            quote.LastRemind = quote.NextRemind;
            quote.NextRemind = quote.NextRemind.AddDays(daysBetweenReminders * 2);

            return quote;
        }

        // POST api/Quote
        public HttpResponseMessage PostQuote(Quote quote)
        {
            if (ModelState.IsValid)
            {
                if (quote.Created.Year < 2000)
                {
                    quote.Created = DateTime.Now;
                }
                quote.LastRemind = quote.Created;
                quote.NextRemind = quote.Created.AddDays(1);

                db.Quotes.Add(quote);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, quote);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = quote.QuoteId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Quote/5
        public HttpResponseMessage DeleteQuote(int id)
        {
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Quotes.Remove(quote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, quote);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}