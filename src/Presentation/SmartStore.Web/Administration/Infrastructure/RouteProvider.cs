using SmartStore.Web.Framework.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartStore.Admin.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        #region Public Properties

        public int Priority
        {
            get { return 1000; }
        }

        #endregion Public Properties



        #region Public Methods

        public void RegisterRoutes(RouteCollection routes)
        {
            var route = routes.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "Admin", id = "" },
                new[] { "SmartStore.Admin.Controllers" }
            );
            route.DataTokens["area"] = "Admin";
        }

        #endregion Public Methods
    }
}