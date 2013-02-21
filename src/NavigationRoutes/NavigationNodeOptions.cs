using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigationRoutes
{
    public class NavigationNodeOptions
    {
        public NavigationNodeOptions()
        {
            this.InnerHtmlFormat = "{0}";
        }

        string DisplayName { get; set; }
        string ElementId { get; set; }
        string FilterToken { get; set; }
        string InnerHtmlFormat { get; set; }

        bool IsHidden { get; set; }

        NavigationNodeType NavigationNodeType { get; set; }

        List<string> CssClasses { get; set; }

    }
}
