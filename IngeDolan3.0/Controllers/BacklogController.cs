using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;

namespace IngeDolan3._0.Controllers
{
    public class BacklogController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // Obtiene los proyectos presentes en la base de datos.
        public List<Project> GetProjects()
        {
            var v = (from a in db.Projects select a);
            return v.ToList();
        }

        // Presenta la lista de todos los backlog que han sido registrados en la página.
        public ActionResult Index()
        {
            List<Project> data = new List<Project>();
            data = GetProjects();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var c in data)
            {
                item.Add(new SelectListItem
                {
                    Text = c.ProjectName.ToString(),
                    Value = c.ProjectID
                });
            }

            var pl = new ProyectoList();
            pl.Nombres = item;

            return View(pl);
            //ViewBag.ProjectName = new SelectList(db.Projects, "ProjectName", "ProjectName");
        }
    }
}
