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
        private dolan2Entities db = new dolan2Entities();

        /*public List<Project> GetProjects()
        {
            var v = (from a in db.Projects select a);
            return v.ToList();
        }*/

        public ActionResult Index()
        {
            /*List<Project> data = new List<Project>();
            data = GetProjects();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var c in data)
            {
                item.Add(new SelectListItem
                {
                    Text = c.ProjectName,
                    Value = c.ProjectName.ToString()
                });
            }

            ViewBag.projs = item;
            return View();*/
            ViewBag.ProjectName = new SelectList(db.Project, "ProjectName", "ProjectName");
            return View();
        }

        [HttpPost]
        public ActionResult Index(Project model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}
