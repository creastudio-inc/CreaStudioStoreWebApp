using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CreaStudioStoreWebApp.Controllers
{
    public class VendorsController : Controller
    {
        // GET: Vendors
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Show(string Name)
        {
            return View();
        }
    }
}