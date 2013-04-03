namespace NavigationRoutes
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Web.Mvc; // nuget:mvc4futures

    public static class NavigationRouteConfigurationExtensions
    {
        public static NavigationRouteBuilder AddChildRoute<T>(this NavigationRouteBuilder builder, string displayText, Expression<Func<T, ActionResult>> action) where T : IController
        {
            var childRoute = new NamedRoute(string.Empty, string.Empty, new MvcRouteHandler());
            childRoute.DisplayName = displayText;
            childRoute.IsChild = true;
            childRoute.ToDefaultAction<T>(action);
            builder._parent.Children.Add(childRoute);
            builder._routes.Add(childRoute.Name, childRoute);
            return builder;
        }

        public static string CreateUrl(string actionName, string controllerName, string areaName)
        {
            var url = CreateUrl(actionName, controllerName);
            if (areaName == string.Empty)
            {
                return url;
            }

            return areaName.ToLower() + "/" + url;
        }

        public static string CreateUrl(string actionName, string controllerName)
        {
            if (controllerName.Equals("home", StringComparison.CurrentCultureIgnoreCase))
            {
                if (actionName.Equals("index", StringComparison.CurrentCultureIgnoreCase))
                {
                    return string.Empty;
                }
                else
                {
                    return actionName.ToLower();
                }
            }

            return controllerName.ToLower() + "/" + actionName.ToLower();
        }

        public static void MapNavigationRoute(this RouteCollection routes, string name, string url, object defaults, object constraints = null)
        {
            var newRoute = new NamedRoute(name, url, new MvcRouteHandler()) { Defaults = new RouteValueDictionary(defaults), Constraints = new RouteValueDictionary(constraints) };
            routes.Add(name, newRoute);
        }

        public static NavigationRouteBuilder MapNavigationRoute(this RouteCollection routes, string name, string displayName, string url, object defaults, object constraints = null)
        {
            var newRoute = new NamedRoute(name, displayName, url, new MvcRouteHandler()) { Defaults = new RouteValueDictionary(defaults), Constraints = new RouteValueDictionary(constraints) };
            routes.Add(name, newRoute);
            return new NavigationRouteBuilder(routes, newRoute);
        }

        public static void MapNavigationRoute(this RouteCollection routes, string name, string displayName, string url, object defaults, string[] namespaces, object constraints = null)
        {
            var newRoute = new NamedRoute(name, displayName, url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if (namespaces != null && namespaces.Length > 0)
            {
                newRoute.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, newRoute);
        }

        public static NavigationRouteBuilder MapNavigationRoute<T>(this RouteCollection routes, string displayName, Expression<Func<T, ActionResult>> action) where T : IController
        {
            var newRoute = new NamedRoute(string.Empty, string.Empty, new MvcRouteHandler());
            newRoute.DisplayName = displayName;
            newRoute.ToDefaultAction(action);
            routes.Add(newRoute.Name, newRoute);
            return new NavigationRouteBuilder(routes, newRoute);
        }

        public static NamedRoute ToDefaultAction<T>(this NamedRoute route, Expression<Func<T, ActionResult>> action) where T : IController
        {
            var body = action.Body as MethodCallExpression;

            if (body == null)
            {
                throw new ArgumentException("Expression must be a method call");
            }

            if (body.Object != action.Parameters[0])
            {
                throw new ArgumentException("Method call must target lambda argument");
            }

            string actionName = body.Method.Name;

            // check for ActionName attribute
            var attributes = body.Method.GetCustomAttributes(typeof(ActionNameAttribute), false);
            if (attributes.Length > 0)
            {
                var actionNameAttr = (ActionNameAttribute)attributes[0];
                actionName = actionNameAttr.Name;
            }

            string controllerName = typeof(T).Name;

            if (controllerName.EndsWith(Constants.Controller, StringComparison.OrdinalIgnoreCase))
            {
                var length = Constants.Controller.Length;
                controllerName = controllerName.Remove(controllerName.Length - length, length);
            }

            route.Defaults = LinkBuilder.BuildParameterValuesFromExpression(body) ?? new RouteValueDictionary();
            foreach (var pair in route.Defaults.Where(x => x.Value == null).ToList())
            {
                route.Defaults.Remove(pair.Key);
            }

            route.Defaults.Add(Constants.Controller, controllerName);
            route.Defaults.Add(Constants.Action, actionName);

            if (route.Options != null)
            {
                // calculate route
                var areaName = route.Options.Area ?? string.Empty;
                route.Url = CreateUrl(actionName, controllerName, areaName);

                if (areaName == string.Empty)
                {
                    route.Name = "Navigation-" + controllerName + "-" + actionName;
                }
                else
                {
                    route.Name = "Navigation-" + areaName + "-" + controllerName + "-" + actionName;
                }

                // apply tokens
                if (route.DataTokens == null)
                {
                    route.DataTokens = new RouteValueDictionary();
                }

                // namespace
                route.DataTokens.Add(Constants.Namespaces, new string[] { typeof(T).Namespace });

                // area
                route.DataTokens.Add(Constants.AreaTokenKey, areaName.ToLower());

                // filter
                var filterToken = route.Options.FilterToken ?? string.Empty;
                if (!string.IsNullOrEmpty(filterToken))
                {
                    route.DataTokens.Add(Constants.FilterTokenKey, filterToken.ToLower());
                }
            }

            return route;
        }
    }

    public class NavigationRouteBuilder
    {
        public NavigationRouteBuilder(RouteCollection routes, NamedRoute parent)
        {
            this._routes = routes;
            this._parent = parent;
        }

        public NamedRoute _parent { get; set; }

        public RouteCollection _routes { get; set; }
    }
}