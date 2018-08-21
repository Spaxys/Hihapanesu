using Hihapanesu.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Hihapanesu.Web.Areas.PrintArea.Controllers
{
    public class PrintController : Controller
    {
        // GET: PrintArea/Print
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print(string data)
        {
            var model = new PrintVM { Data = data };
            return View(model);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Print([FromBody]PrintVM model)
        {
            return View(model);
        }
    }
}