using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace NavigationRoutes
{
    public class NavigationNodeOptions
    {
        public NavigationNodeOptions()
        {
            this.InnerHtmlFormat = "{0}";
            CssClasses=new List<string>();
        }

        public string DisplayName { get; set; }
        public string ElementId { get; set; }
        public string FilterToken { get; set; }
        public string InnerHtmlFormat { get; set; }

        public bool IsHidden { get; set; }

        public NavigationNodeType NavigationNodeType { get; set; }

        public List<string> CssClasses { get; protected set; }

        public Route Route { get; set; }
    }
}
