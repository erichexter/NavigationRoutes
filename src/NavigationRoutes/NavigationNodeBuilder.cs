using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NavigationRoutes;

namespace UnitTests
{
    public class NavigationNodeBuilder
    {
        public INavigationNode CurrentNode { get; set; }
        public INavigationNode CurrentParentNode { get; set; }

        public IList<INavigationNode> Nodes { get; set; }

        public NavigationNodeBuilder WithRoute<T>(Expression<Func<T, ActionResult>> action) where T : IController
        {
            var newRoute = new NamedRoute("", "", new MvcRouteHandler());
            newRoute.DisplayName = CurrentNode.Options.DisplayName;
            newRoute.ToDefaultAction(action);
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

        public NavigationNodeBuilder WithAreaRoute<T>(Expression<Func<T, ActionResult>> action, string areaName) where T : IController
        {
            var newRoute = new NamedRoute("", "", new MvcRouteHandler());
            newRoute.DisplayName = CurrentNode.Options.DisplayName;
            newRoute.Area = areaName;
            // todo: area <= review this method implementation
            //newRoute.Options.AreaName = areaName;

            newRoute.ToDefaultAction(action);
            CurrentNode.Options.Route = newRoute;
            CurrentNode.Options.NavigationNodeType = NavigationNodeType.Link;
            return this;
        }
    }
}