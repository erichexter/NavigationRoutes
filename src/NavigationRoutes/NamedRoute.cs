namespace NavigationRoutes
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class NamedRoute : Route
    {
        private List<NamedRoute> routeChildRoutes = new List<NamedRoute>();
        private string routeDisplayName = string.Empty;
        private string routeName = string.Empty;
        private string routeNavigationRoute = string.Empty;
        private NavigationRouteOptions routeOptions = new NavigationRouteOptions();

        public NamedRoute(string name, string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            this.routeName = name;
        }

        public NamedRoute(string name, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
            this.routeName = name;
        }

        public NamedRoute(string name, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            this.routeName = name;
        }

        public NamedRoute(string name, string displayName, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            this.routeName = name;
            this.routeDisplayName = displayName;
        }

        public NamedRoute(string name, string displayName, string url, MvcRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            this.routeName = name;
            this.routeDisplayName = displayName;
        }

        public List<NamedRoute> Children
        {
            get { return this.routeChildRoutes; }
        }

        public string DisplayName
        {
            get { return this.routeDisplayName ?? this.routeName; }
            set { this.routeDisplayName = value; }
        }

        public bool IsChild { get; set; }

        public string Name
        {
            get { return this.routeName; }
            set { this.routeName = value; }
        }

        public NavigationRouteOptions Options
        {
            get { return this.routeOptions; }
            set { this.routeOptions = value; }
        }
    }
}