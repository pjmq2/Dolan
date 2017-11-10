using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Controllers
{
    public class HomeController : Controller
    {
        // Presenta la pantalla de Título.
        public ActionResult Index()
        {
            return View();
        }

        // Presenta la pantalla que describe de qué se trata la página.
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        // Presenta la pantalla que presenta los datos para contactar a los administradores de la página.
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}