using System.Linq;
using System.Web;
using Hangfire.Dashboard;
using Umbraco.Web.Composing;
using Umbraco.Web.Security;

namespace WebShop
{
    public class UmbracoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var http = new HttpContextWrapper(HttpContext.Current);
            var ticket = http.GetUmbracoAuthTicket();
            http.AuthenticateCurrentRequest(ticket, true);

            var user = Current.UmbracoContext.Security.CurrentUser;
            return user != null && user.Groups.Any(g => g.Alias == "admin");
        }
    }
}