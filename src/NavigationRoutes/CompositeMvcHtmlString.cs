namespace NavigationRoutes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

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