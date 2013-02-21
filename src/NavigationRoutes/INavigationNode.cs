using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigationRoutes
{
    public interface INavigationNode
    {
        INavigationNode ParentNode { get; set; }
        List<INavigationNode> ChildNavigationNodes { get; set; }
        NavigationNodeOptions Options { get; set; }
        
        // move to route
        string AreaName { get; set; }
        
    }

    
}
