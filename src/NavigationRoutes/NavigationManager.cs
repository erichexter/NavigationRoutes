using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigationRoutes
{
    public class NavigationManager : List<INavigationNode>
    {
        private List<INavigationNode> _navigationNodes;
        NavigationRouteFilters Filters { get; set; }

        // void InsertAfter(INavigationNode target, IEnumerable<INavigationNode> nodes)

        public string ToJson()
        {
            return "";
        }
    }
}
