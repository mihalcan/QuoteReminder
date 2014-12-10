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
using QuoteReminder.DataAccess;

namespace QuoteReminder.Controllers
{
    public class QuoteController : ApiController
    {
        private QuoteReminderContext db = new QuoteReminderContext();

        private IQuoteRepository repository;

        public QuoteController (IQuoteRepository repository)
	    {
            this.repository = repository;
	    }

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
            Quote quote = this.repository.GetById(id);
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
                quote = this.UpdateDates(id, quote);                
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

        // POST api/Quote
        public HttpResponseMessage PostQuote(Quote quote)
        {
            if (ModelState.IsValid)
            {
                this.repository.Add(quote);

                return this.CreateSuccessResponse(quote);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Quote/5
        public HttpResponseMessage DeleteQuote(int id)
        {
            try
            {
                this.repository.Delete(id);
            }
            catch (ArgumentException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        private Quote UpdateDates(int id, Quote quote)
        {
            switch (quote.EditType)
            {
                case EditType.KnowIt:
                    return this.UpdateRemindDateWhenKnow(quote.QuoteId);
                case EditType.Forget:
                    return this.UpdateRemindDateWhenForget(quote.QuoteId);
                default:
                    return quote;
            }
        }

        private Quote UpdateRemindDateWhenKnow(int quoteId)
        {
            var quote = this.GetQuote(quoteId);
            var daysBetweenReminders = (quote.NextRemind - quote.LastRemind).Days;

            quote.LastRemind = quote.NextRemind;
            quote.NextRemind = quote.NextRemind.AddDays(daysBetweenReminders * 2);

            return quote;
        }

        private Quote UpdateRemindDateWhenForget(int quoteId)
        {
            var quote = this.GetQuote(quoteId);
            quote.LastRemind = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            quote.NextRemind = quote.LastRemind.AddDays(1);

            return quote;
        }

        private HttpResponseMessage CreateSuccessResponse(Quote quote)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, quote);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = quote.QuoteId }));
            return response;
        }
    }
}