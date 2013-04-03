namespace UnitTests
{
    using System;
    using System.Web.Mvc;

    public class HomeController : System.Web.Mvc.Controller
    {
        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            return this.RedirectToAction("~/");
        }

        internal ActionResult ChangePassword()
        {
            throw new NotImplementedException();
        }
    }
}