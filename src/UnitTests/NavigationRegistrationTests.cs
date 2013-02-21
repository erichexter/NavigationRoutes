using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using NavigationRoutes;

namespace UnitTests
{
    public class NavigationRegistrationTests
    {
        public void Should_be_an_awesome_api()
        {

            var nav = GlobalNavigation.Nodes;

             nav.AddNode("Ädmin").WithRoute<HomeController>(c=>c.Index())
            .AddChild("Latest",{}).WithRoute<>()
            .AddChild("Queue New").WithRoute<>()
            .AddDivider()
            .AddHeader("head this") 
            .AddChildren(IList)
        }
    }

    public class GlobalNavigation
    {
        public static IList<NavigationNode> Nodes { get; set; }
    }
    public static class GlobalNavigationNodesExtensions
    {
        public static NavigationNodeBuilder AddNode(this IList<NavigationNode> nodes, string DisplayText)
        {

            
            var node = new NavigationNode() {DisplayText = DisplayText};
            nodes.Add(node);
            return new NavigationNodeBuilder(){CurrentNode=node,Nodes=nodes};
        }
    }

    public class NavigationNodeBuilder
    {
        public NavigationNode CurrentNode { get; set; }

        public IList<NavigationNode> Nodes { get; set; }

        public NavigationNodeBuilder WithRoute<T>(Expression<Func<T, ActionResult>> action)
        {
            var newRoute = new NamedRoute("", "", new MvcRouteHandler());
            newRoute.DisplayName = CurrentNode.DisplayText;
            //newRoute.NavigationGroup = navigationGroup;
            newRoute.Options = new NavigationRouteOptions();
            newRoute.ToDefaultAction(action, newRoute.Options);
            CurrentNode.Route = newRoute;
            CurrentNode.NodeType = NodeType.Link;
        }
    }

    public enum NodeType
    {
        Link
    }

    public class NavigationNode
    {
        public string DisplayText { get; set; }

        public NodeType NodeType { get; set; }

        public Route Route { get; set; }
    }
}
