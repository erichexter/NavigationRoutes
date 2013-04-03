namespace NavigationRoutes
{
    using System.Collections.Generic;

    public interface INavigationNode
    {
        List<INavigationNode> ChildNavigationNodes { get; set; }

        NavigationNodeOptions Options { get; set; }

        INavigationNode ParentNode { get; set; }
    }
}