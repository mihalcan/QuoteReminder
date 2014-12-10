using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuoteReminder.Models;

namespace QuoteReminder.DataAccess
{
    public interface IQuoteRepository
    {
        IEnumerable<Quote> Get();

        Quote GetById(int id);

        void Add(Quote quote);

        void Update(Quote quote);

        void Delete(int id);
    }
}