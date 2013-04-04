namespace UnitTests
{
    using System.Collections.Generic;
    using NavigationRoutes;

    public class NavigationNode : INavigationNode
    {
        public NavigationNode()
        {
            this.Options = new NavigationNodeOptions();
            this.ChildNavigationNodes = new List<INavigationNode>();
        }

        public List<INavigationNode> ChildNavigationNodes { get; set; }

        public NavigationNodeOptions Options { get; set; }

        public INavigationNode ParentNode { get; set; }
    }
}