namespace NavigationRoutes
{
    using System.Web.Routing;

    public class NavigationRouteBuilder
    {
        public NavigationRouteBuilder(RouteCollection routes, NamedRoute parent)
        {
            this.Routes = routes;
            this.Parent = parent;
        }

        public NamedRoute Parent { get; set; }

        public RouteCollection Routes { get; set; }
    }
}