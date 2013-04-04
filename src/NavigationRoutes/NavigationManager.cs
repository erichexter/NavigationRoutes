namespace NavigationRoutes
{
    using System.Collections.Generic;

    public class NavigationManagerCollection : List<INavigationNode>
    {
        // this is never used
        private List<INavigationNode> baseNavigationNodes;

        ////public NavigationRouteFilters Filters { get; set; }

        ////void InsertAfter(INavigationNode target, IEnumerable<INavigationNode> nodes)

        public string ToJson()
        {
            return string.Empty;
        }
    }
}