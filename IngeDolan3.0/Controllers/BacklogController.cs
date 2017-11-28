using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using IngeDolan3._0.Generator;

namespace IngeDolan3._0.Controllers
{
    public class BacklogController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();

        // Displays a list of all of the system's product backlogs
        public ActionResult Index(){
            if (IDGenerator.CanDo("Acceder Backlog"))
            {
                List<Project> data = new List<Project>();
                var v = (from a in db.Projects select a);
                data = v.ToList();
                List<SelectListItem> item = new List<SelectListItem>();
                foreach (var c in data)
                {
                    item.Add(new SelectListItem
                    {
                        Text = c.ProjectName.ToString(),
                        Value = c.ProjectID
                    });
                }

                var pl = new GenericList();
                pl.Nombres = item;

                return View(pl);
            }
            else
            {
                return RedirectToAction("Denied", "Others");
            }
        }
    }
}
