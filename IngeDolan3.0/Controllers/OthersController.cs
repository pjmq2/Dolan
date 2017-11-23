using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Controllers
{
    public class OthersController : Controller
    {
        // Displays an error message when an user tries to perform an action its not allowed to.
        public ActionResult Denied()
        {
            return View();
        }

        // Displays a message when a controlled error gets triggered on the system.
        public ActionResult Error()
        {
            return View();
        }
    }
}