using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Umbraco.Core;
using Umbraco.Core.Composing;
using WebShop.LightInjector;

namespace WebShop
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class HangfireJobsComposer : IComposer
    {
        public void Compose(Composition composition)
        {
            // Configure hangfire
            var options = new SqlServerStorageOptions { PrepareSchemaIfNecessary = true };
            const string umbracoConnectionName = Constants.System.UmbracoConnectionName;
            var connectionString = System.Configuration
                .ConfigurationManager
                .ConnectionStrings[umbracoConnectionName]
                .ConnectionString;

            var container = composition.Concrete as LightInject.ServiceContainer;

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(connectionString, options)
                .UseConsole()
                .UseActivator(new LightInjectJobActivator(container));
        }
    }
}