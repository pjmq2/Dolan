using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Controllers
{
    public class OthersController : Controller
    {
        // Presenta la pantalla de negación cuando el usuario no presenta los requisitos para acceder.
        public ActionResult Denied()
        {
            return View();
        }

        // Presenta la pantalla que indica si se dió un error [controlado] en la página.
        public ActionResult Error()
        {
            return View();
        }
    }
}