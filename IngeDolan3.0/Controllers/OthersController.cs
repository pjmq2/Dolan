using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Controllers
{
    public class OthersController : Controller
    {
        // GET: Others
        public ActionResult Denied()
        {
            return View();
        }

        // GET: Others
        public ActionResult Error()
        {
            return View();
        }
    }
}