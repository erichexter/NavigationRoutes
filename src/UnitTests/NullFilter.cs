namespace UnitTests
{
    using System.Web.Routing;
    using NavigationRoutes;

    public class NullFilter : INavigationFilter
    {
        public bool FilterWasCalled = false;

        public bool ShouldRemove(Route navigationRoutes)
        {
            this.FilterWasCalled = true;
            return false;
        }
    }
}