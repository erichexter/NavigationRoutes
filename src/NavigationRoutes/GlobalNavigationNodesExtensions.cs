namespace UnitTests
{
    using System.Collections.Generic;
    using NavigationRoutes;

    public static class GlobalNavigationNodesExtensions
    {
        public static NavigationNodeBuilder AddNode(this IList<INavigationNode> nodes, string displayText)
        {
            var node = new NavigationNode() { Options = new NavigationNodeOptions() { DisplayName = displayText } };
            nodes.Add(node);
            return new NavigationNodeBuilder() { CurrentNode = node, Nodes = nodes };
        }
    }
}