namespace UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using NavigationRoutes;

    public class NavigationNodeBuilder
    {
        public INavigationNode CurrentNode { get; set; }

        public INavigationNode CurrentParentNode { get; set; }

        public IList<INavigationNode> Nodes { get; set; }

        public NavigationNodeBuilder AddChild(string displayText)
        {
            var childNode = new NavigationNode() { };
            childNode.Options.DisplayName = displayText;
            this.CurrentNode.ChildNavigationNodes.Add(childNode);
            this.CurrentParentNode = this.CurrentNode;
            this.CurrentNode = childNode;
            return this;
        }

        public NavigationNodeBuilder AddChildren(IList<NavigationNode> childNodes)
        {
            this.CurrentNode.ChildNavigationNodes.AddRange(childNodes);
            return this;
        }

        public NavigationNodeBuilder AddDivider()
        {
            var node = new NavigationNode();
            node.Options.NavigationNodeType = NavigationNodeType.Divider;
            this.Nodes.Add(node);
            this.CurrentNode = node;
            return this;
        }

        public NavigationNodeBuilder AddHeader(string displayText)
        {
            var node = new NavigationNode();
            node.Options.NavigationNodeType = NavigationNodeType.Header;
            node.Options.DisplayName = displayText;
            this.Nodes.Add(node);
            this.CurrentNode = node;
            return this;
        }

        public NavigationNodeBuilder AddNode(string displayText)
        {
            var node = new NavigationNode() { Options = new NavigationNodeOptions() { DisplayName = displayText } };
            this.Nodes.Add(node);
            this.CurrentNode = node;
            return this;
        }

        public NavigationNodeBuilder WithAreaRoute<T>(Expression<Func<T, ActionResult>> action, NavigationRouteOptions options = null) where T : IController
        {
            var newRoute = new NamedRoute(string.Empty, string.Empty, new MvcRouteHandler());
            newRoute.DisplayName = this.CurrentNode.Options.DisplayName;

            if (options != null)
            {
                newRoute.Options.Area = options.Area;
                newRoute.Options.FilterToken = options.FilterToken;
            }

            newRoute.ToDefaultAction(action);
            this.CurrentNode.Options.Route = newRoute;
            this.CurrentNode.Options.NavigationNodeType = NavigationNodeType.Link;
            return this;
        }

        public NavigationNodeBuilder WithRoute<T>(Expression<Func<T, ActionResult>> action) where T : IController
        {
            var newRoute = new NamedRoute(string.Empty, string.Empty, new MvcRouteHandler());
            newRoute.DisplayName = this.CurrentNode.Options.DisplayName;
            newRoute.ToDefaultAction(action);
            this.CurrentNode.Options.Route = newRoute;
            this.CurrentNode.Options.NavigationNodeType = NavigationNodeType.Link;
            return this;
        }
    }
}