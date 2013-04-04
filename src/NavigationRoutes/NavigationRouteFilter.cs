namespace NavigationRoutes
{
    using System.Web.Routing;

    public class NavigationRouteFilter : INavigationRouteFilter
    {
        public bool ShouldRemove(Route route)
        {
            return true;
        }
    }
}