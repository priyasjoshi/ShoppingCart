using System.Web.Mvc;
using System.Web.Routing;
using ShoppingCart.App_Start;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //OAuthProviders.Configure();
        }
    }
}
