using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Controllers
{
    public class HomeController : Controller{
        // Displays system's title screen
        public ActionResult Index(){
            return View();
        }

        // Displays the systems information screen
        public ActionResult About(){
            ViewBag.Message = "Your application description page.";

            return View();
        }


        // Displays all of the information to contact the system's administrator
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}