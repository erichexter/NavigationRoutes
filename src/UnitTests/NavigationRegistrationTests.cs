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

            nav.AddNode("Ädmin").WithRoute<HomeController>(c => c.Index())
               .AddChild("Latest").WithRoute<HomeController>(c => c.Index())
                //.AddChild("Queue New").WithRoute<>()
               .AddDivider()
               .AddHeader("head this");
            //.AddChildren(IList)
        }
    }

    public class GlobalNavigation
    {
        public static IList<INavigationNode> Nodes { get; set; }
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
    }

}
