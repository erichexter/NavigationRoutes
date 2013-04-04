namespace NavigationRoutes
{
    using System.Web.Routing;

    public static class RouteValueDictionaryExtensions
    {
        public static string FilterToken(this RouteValueDictionary routeValues)
        {
            return (string)routeValues[Constants.FilterTokenKey];
        }

        public static bool HasFilterToken(this RouteValueDictionary routeValues)
        {
            return routeValues.ContainsKey(Constants.FilterTokenKey);
        }
    }
}