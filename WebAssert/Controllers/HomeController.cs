namespace WebAssert.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index(string imagePath = "")
        {
            WebAssert.Assert(() => !string.IsNullOrEmpty(imagePath), "The param 'imagePath' must not be empty.");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}