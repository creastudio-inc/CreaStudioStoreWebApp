using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreaStudioStoreWebApp.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout 
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            return View();
        }
    }
}