using System.Collections.Generic;
using NavigationRoutes;

namespace UnitTests
{
    public static class GlobalNavigationNodesExtensions
    {
        public static NavigationNodeBuilder AddNode(this IList<INavigationNode> nodes, string DisplayText)
        {            
            var node = new NavigationNode() {Options = new NavigationNodeOptions(){DisplayName  = DisplayText}};            
            nodes.Add(node);            
            return new NavigationNodeBuilder(){CurrentNode=node,Nodes=nodes};
        }
    }
}