using Hangfire;
using Microsoft.Owin;
using Owin;
using Umbraco.Web;
using WebShop;
using WebShop.Services.Interfaces;

[assembly: OwinStartup("UmbracoStandardOwinStartup", typeof(UmbracoStandardOwinStartup))]
namespace WebShop
{
    public class UmbracoStandardOwinStartup : UmbracoDefaultOwinStartup

    {
        public UmbracoStandardOwinStartup()
        {
        }
        
        public override void Configuration(IAppBuilder app)
        {
            //ensure the default options are configured
            base.Configuration(app);

            var dashboardOptions = new DashboardOptions { Authorization = new[] { new UmbracoAuthorizationFilter() } };
            app.UseHangfireDashboard("/hangfire", dashboardOptions);
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate<IHangfire>(x => x.ScheduleRecurringProductUpdate(), "0 23 * * *");

            //BackgroundJob.Enqueue<HangfireService>(x => x.InitDatabase());

        }
    }
}