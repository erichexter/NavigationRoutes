using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavigationRoutes
{
    class Scratch
    {
        public Scratch(GlobalNavigationManager nodes)
        {
            //NavigationManager manager = new NavigationManager();


            List<INavigationNode> childNodes = new List<INavigationNode>()
                {
                    NavigationManager.BuildNode<HomeController>(c => "Foo", c.Foo()),
                    NavigationManager.BuildNode<HomeController>(c => "Bar", c.Bar()),
                    new BootstrapDividerNode(BootstrapDividerType.Vertical),
                    NavigationManager.BuildNode<HomeController>(c => "Baz", c.Baz()),
                    NavigationManager.BuildNode<HomeController>(c => "For", c.For())
                };
            
            
            INavigationNode node = manager.AddNode("Admin")
                                          .WithRoute<HomeController>(c => c.Index())
                                          .WithChildren(n => childNodes);

        }

    }
}
