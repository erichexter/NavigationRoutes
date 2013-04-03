namespace NavigationRoutes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public static class NavigationViewExtensions
    {
        public static IEnumerable<NamedRoute> GetRoutesForCurrentRequest(RouteCollection routes, IList<INavigationFilter> routeFilters)
        {
            var navigationRoutes = routes.OfType<NamedRoute>().Where(r => r.IsChild == false).ToList();
            if (routeFilters.Count() > 0)
            {
                foreach (var route in navigationRoutes.ToArray())
                {
                    foreach (INavigationFilter filter in routeFilters)
                    {
                        if (filter.ShouldRemove(route))
                        {
                            navigationRoutes.Remove(route);
                            break;
                        }
                    }
                }
            }

            return navigationRoutes;
        }

        public static IHtmlString Navigation(this HtmlHelper helper)
        {
            // todo: this is a little more complex than it may need to be now that we're moving to tree/node
            return new CompositeMvcHtmlString(
                GetRoutesForCurrentRequest(RouteTable.Routes, GlobalNavigation.Filters)
                .Select(routeGroup => helper.NavigationListItemRouteLink(new List<NamedRoute>() { routeGroup })));
        }

        public static MvcHtmlString NavigationListItemRouteLink(this HtmlHelper html, IEnumerable<NamedRoute> routes)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("nav");

            var namedRoutes = routes as IList<NamedRoute> ?? routes.ToList();

            // TODO: css classes
            var tagBuilders = new List<TagBuilder>();

            foreach (var route in namedRoutes)
            {
                var li = new TagBuilder("li");
                li.InnerHtml = html.RouteLink(route.DisplayName, route.Name).ToString();

                // TODO: css classes
                if (CurrentRouteMatchesName(html, route.Name))
                {
                    li.AddCssClass("active");
                }

                if (route.Children.Any())
                {
                    BuildChildMenu(html, route, li);
                }

                tagBuilders.Add(li);
            }

            var tags = new StringBuilder();
            tagBuilders.ForEach(b => tags.Append(b.ToString(TagRenderMode.Normal)));
            ul.InnerHtml = tags.ToString();

            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

        private static void BuildChildMenu(HtmlHelper html, NamedRoute route, TagBuilder li)
        {
            // convert menu entry to dropdown
            li.AddCssClass("dropdown");
            li.InnerHtml = "<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">" + route.DisplayName +
                           "<b class=\"caret\"></b></a>";

            // build LIs for the children
            var ul = new TagBuilder("ul");
            ul.AddCssClass("dropdown-menu");
            foreach (var child in route.Children)
            {
                var childLi = new TagBuilder("li");
                childLi.InnerHtml = html.RouteLink(child.DisplayName, child.Name).ToString();
                ul.InnerHtml += childLi.ToString();
            }

            // append the UL
            li.InnerHtml = "<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" + route.DisplayName +
                           " <b class='caret'></b></a>" + ul.ToString();
        }

        private static bool CurrentRouteMatchesName(HtmlHelper html, string routeName)
        {
            var namedRoute = html.ViewContext.RouteData.Route as NamedRoute;
            if (namedRoute != null)
            {
                if (string.Equals(routeName, namedRoute.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }

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

    public class CompositeMvcHtmlString : IHtmlString
    {
        private readonly IEnumerable<IHtmlString> baseStrings;

        public CompositeMvcHtmlString(IEnumerable<IHtmlString> strings)
        {
            this.baseStrings = strings;
        }

        public string ToHtmlString()
        {
            return string.Join(string.Empty, this.baseStrings.Select(x => x.ToHtmlString()));
        }
    }
}