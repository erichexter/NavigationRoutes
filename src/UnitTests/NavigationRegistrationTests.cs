using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using NavigationRoutes;
using Should;
namespace UnitTests
{
    [TestFixture]
    public class NavigationRegistrationTests
    {
        [Test]
        public void Should_be_an_awesome_api()
        {

            var nav = GlobalNavigation.Nodes;

            CreateNavigationNodes(nav);             

            GlobalNavigation.Nodes.Count().ShouldEqual(4);
            GlobalNavigation.Nodes.Clear();
        }
        [Test]
        public void Should_add_routes_to_the_route_collection()
        {           

            CreateNavigationNodes(GlobalNavigation.Nodes);

            var routes = new System.Web.Routing.RouteCollection();

            GlobalNavigation.AddAllRoutes(routes);
            routes.Count.ShouldEqual(3);
            GlobalNavigation.Nodes.Clear();
        }
        private static void CreateNavigationNodes(IList<INavigationNode> nav)
        {
            nav.AddNode("Ädmin").WithRoute<HomeController>(c => c.Index())
               .AddChild("Latest").WithRoute<HomeController>(c => c.Index())
               .AddChild("Queue New").WithRoute<HomeController>(c => c.Index())
               .AddDivider()
               .AddHeader("head this")
               .AddChildren(new List<NavigationNode>()
                   {
                       new NavigationNode() {Options = new NavigationNodeOptions() {DisplayName = "foo"}}
                   })
               .AddNode("Home");
        }
    }

    public class GlobalNavigation
    {
         static GlobalNavigation()
        {
             Nodes=new List<INavigationNode>();
        }

        public static IList<INavigationNode> Nodes { get; protected set; }

        public static void AddAllRoutes(RouteCollection routes)
        {
            foreach (var node in Nodes)
            {
                AddNodeToRoutes(routes, node);
            }
        }

        private static void AddNodeToRoutes(RouteCollection routes, INavigationNode node)
        {
            if (node.Options.Route != null)
            {
                routes.Add(node.Options.Route);
            }
            foreach (var child in node.ChildNavigationNodes)
            {
                AddNodeToRoutes(routes,child);
            }
        }
    }
    public static class GlobalNavigationNodesExtensions
    {
        public static NavigationNodeBuilder AddNode(this IList<INavigationNode> nodes, string DisplayText)
        {            
            var node = new NavigationNode() {Options = new NavigationNodeOptions(){DisplayName  = DisplayText}};            
            nodes.Add(node);            
            return new NavigationNodeBuilder(){CurrentNode=node,Nodes=nodes};
        }
    }

    public class NavigationNode:INavigationNode
    {
        public NavigationNode()
        {
            Options=new NavigationNodeOptions();
            ChildNavigationNodes=new List<INavigationNode>();
        }

        public INavigationNode ParentNode { get; set; }
        public List<INavigationNode> ChildNavigationNodes { get; set; }
        public NavigationNodeOptions Options { get; set; }
        
    }

    public class NavigationNodeBuilder
    {
        public INavigationNode CurrentNode { get; set; }
        public INavigationNode CurrentParentNode { get; set; }

        public IList<INavigationNode> Nodes { get; set; }

        public NavigationNodeBuilder WithRoute<T>(Expression<Func<T, ActionResult>> action) where T : IController
        {
            var newRoute = new NamedRoute("", "", new MvcRouteHandler());
            newRoute.DisplayName = CurrentNode.Options.DisplayName;
            newRoute.ToDefaultAction(action, newRoute.Options);
            CurrentNode.Options.Route = newRoute;
            CurrentNode.Options.NavigationNodeType = NavigationNodeType.Link;
            return this;
        }

        public NavigationNodeBuilder AddChild(string displayText)
        {
            
            var childNode = new NavigationNode() {};
            childNode.Options.DisplayName = displayText;
            CurrentNode.ChildNavigationNodes.Add(childNode);
            CurrentParentNode = CurrentNode;
            CurrentNode = childNode;
            return this;
        }

        public NavigationNodeBuilder AddDivider()
        {
            var node = new NavigationNode();
            node.Options.NavigationNodeType=NavigationNodeType.Divider;
            Nodes.Add(node);
            CurrentNode = node;
            return this;
        }

        public NavigationNodeBuilder AddHeader(string displayText)
        {
            var node = new NavigationNode();
            node.Options.NavigationNodeType=NavigationNodeType.Header;
            node.Options.DisplayName = displayText;
            Nodes.Add(node);
            CurrentNode = node;
            return this;
        }

        public NavigationNodeBuilder AddChildren(IList<NavigationNode> childNodes)
        {
            CurrentNode.ChildNavigationNodes.AddRange(childNodes);
            return this;
        }

        public NavigationNodeBuilder AddNode(string displayText)
        {
            var node = new NavigationNode() {Options = new NavigationNodeOptions() {DisplayName = displayText}};
            Nodes.Add(node);
            CurrentNode = node;
            return this;
        }
    }

}
