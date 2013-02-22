using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NavigationRoutes;
using Should;
namespace UnitTests
{
    [TestFixture]
    public class NavigationRegistrationTests:RouteTesterBase
    {
        [Test]
        public void Should_be_an_awesome_api()
        {
            GlobalNavigation.Nodes.Clear();
            var nav = GlobalNavigation.Nodes;

            CreateNavigationNodes(nav);             

            GlobalNavigation.Nodes.Count().ShouldEqual(4);
            GlobalNavigation.Nodes.Clear();
        }
        [Test]
        public void Shoud_add_and_apply_filters()
        {
  
            GlobalNavigation.Filters.Add(new RemoveAuthorizeActions());

 
            GlobalNavigation.Nodes.AddNode("Home").WithRoute<HomeController>(c => c.Index())
               .AddNode("Login").WithRoute<HomeController>(c => c.Logout());

            var routes = new System.Web.Routing.RouteCollection();
            routes.AddNodesToRoutes(GlobalNavigation.Nodes);
            routes.Count.ShouldNotEqual(0);

            var currentRoutes = NavigationRoutes.NavigationViewExtensions.GetRoutesForCurrentRequest(routes, GlobalNavigation.Filters);
            currentRoutes.Count().ShouldEqual(1);

        }
        [Test]
        public void Should_add_routes_to_the_route_collection()
        {
            GlobalNavigation.Nodes.Clear();

            CreateNavigationNodes(GlobalNavigation.Nodes);

            var routes = new System.Web.Routing.RouteCollection();

            GlobalNavigation.AddAllRoutes(routes);
            routes.Count.ShouldEqual(3);
            GlobalNavigation.Nodes.Clear();
        }

    }
}
