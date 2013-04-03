namespace NavigationRoutes
{
    using System.Collections.Generic;

    public class NavigationNodeOptions
    {
        public NavigationNodeOptions()
        {
            this.InnerHtmlFormat = "{0}";
            this.CssClasses = new List<string>();
        }

        public List<string> CssClasses { get; protected set; }

        public string DisplayName { get; set; }

        public string ElementId { get; set; }

        public string FilterToken { get; set; }

        public string InnerHtmlFormat { get; set; }

        public bool IsHidden { get; set; }

        public NavigationNodeType NavigationNodeType { get; set; }

        public NamedRoute Route { get; set; }
    }
}