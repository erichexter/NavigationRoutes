using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Should;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.IO;
using NavigationRoutes;
namespace UnitTests
{
    [TestFixture]
    public class Navigation_Route_Should:RouteTesterBase
    {
        [Test]
        public void Map_a_route_to_a_url_using_an_area()
        {
            var nodes = new List<INavigationNode>();
            CreateNavigationNodes(nodes);
            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes(nodes);
 
            routes.Count.ShouldNotEqual(0);

            var uh = GetUrlHelper(routes);

            uh.RouteUrl("Navigation-Admin-Home-About").ShouldEqual("/admin/about");
            var r = (Route)routes["Navigation-Admin-Home-About"];
            r.DataTokens["area"].ToString().ShouldEqual("admin");
            
            uh.RouteUrl("Navigation-AdMin-Home-Index").ShouldEqual("/admin");
      

        }
        [Test]
        public void Map_a_route_to_a_url_matching_the_action_name()
        {
            var nav = new List<INavigationNode>();

            nav.AddNode("Home").WithRoute<HomeController>(c => c.Index())
               .AddNode("About").WithRoute<HomeController>(c => c.About());
            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes( nav);

            routes.Count.ShouldNotEqual(0);

            var uh = GetUrlHelper(routes);

            uh.RouteUrl("Navigation-Home-About").ShouldEqual("/about");
            uh.RouteUrl("Navigation-Home-Index").ShouldEqual("/"); 
        }

        [Test]
        public void Map_home_index_to_the_site_root()
        {
            var nav = new List<INavigationNode>();

            nav.AddNode("Home").WithRoute<HomeController>(c => c.Index())
               .AddNode("About").WithRoute<HomeController>(c => c.About())
               .AddNode("Login").WithRoute<HomeController>(c => c.Logout());
            
            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes(nav);
            routes.Count.ShouldNotEqual(0);

            var uh = GetUrlHelper(routes);
            uh.RouteUrl("Navigation-Home-Index").ShouldEqual("/");
        }
        [Test]
        public void Apply_filter_to_the_current_request()
        {
            var routes = RouteTable.Routes;
            var filter = new NullFilter();
            GlobalNavigation.Filters.Add(filter);
            routes.MapNavigationRoute<HomeController>("Home", c => c.Index());

            NavigationRoutes.NavigationViewExtensions.Navigation(null);

            filter._wasCalled.ShouldBeTrue();
        }
        [Test]
        public void should_apply_filters()
        {
            
            IList<INavigationFilter> filters = new List<INavigationFilter>();
            filters.Add(new RemoveAuthorizeActions());

            var nav = new List<INavigationNode>();

            nav.AddNode("Home").WithRoute<HomeController>(c => c.Index())
               .AddNode("Login").WithRoute<HomeController>(c => c.Logout());

            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes(nav);
            routes.Count.ShouldNotEqual(0);

            var currentRoutes = NavigationRoutes.NavigationViewExtensions.GetRoutesForCurrentRequest(routes, filters);
            currentRoutes.Count().ShouldEqual(1);
        }


        [Test]
        public void add_namespaces()
        {
            var nav = new List<INavigationNode>();
            nav.AddNode("Home").WithRoute<HomeController>(c => c.Index());
            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes(nav);
            routes.Count().ShouldEqual(1);

            var namedRoute = ((NamedRoute) routes[0]);
            namedRoute.ShouldNotBeNull();
            var namespaces = (string[]) namedRoute.DataTokens["Namespaces"];
            namespaces.ShouldContain("UnitTests");
        }
    }
}
