using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using QuoteReminder.DataAccess;
using QuoteReminder.Controllers;

namespace QuoteReminder.DependencyInjection
{
    public class RepositoryInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IQuoteRepository>().ImplementedBy<QuoteRepository>());

            container.Register(Component.For<QuoteController>());
        }
    }
}