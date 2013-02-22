using System.Collections.Generic;
using NavigationRoutes;

namespace UnitTests
{
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
}