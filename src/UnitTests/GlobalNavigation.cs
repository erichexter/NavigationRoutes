using System;
using System.Collections.Generic;
using System.Web.Routing;
using NavigationRoutes;

namespace UnitTests
{
    public static class GlobalNavigation
    {
        static GlobalNavigation()
        {
            Nodes=new List<INavigationNode>();
        }

        public static IList<INavigationNode> Nodes { get; private set; }

        public static IList<INavigationFilter> Filters
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public static void AddAllRoutes(RouteCollection routes)
        {
            AddNodesToRoutes(routes, Nodes);
        }

        public static void AddNodesToRoutes(this RouteCollection routes, IList<INavigationNode> nodes)
        {
            foreach (var node in nodes)
            {
                AddNodeToRoutes(routes, node);
            }
        }

        private static void AddNodeToRoutes(RouteCollection routes, INavigationNode node)
        {
            if (node.Options.Route != null)
            {
                routes.Add(node.Options.Route.Name ,node.Options.Route);
            }
            foreach (var child in node.ChildNavigationNodes)
            {
                AddNodeToRoutes(routes,child);
            }
        }
    }
}