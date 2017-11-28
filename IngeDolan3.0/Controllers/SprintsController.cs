using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using IngeDolan3._0.Generator;

namespace IngeDolan3._0.Controllers
{
    public class SprintsController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Displays a list of all of the sprints from a specific project
        public async Task<ActionResult> Index(string projectId){
            var sprints = db.Sprints.Include(s => s.Project);
            ViewBag.ProjectID = projectId;
            return View(await sprints.ToListAsync());
        }

        // GET: Displays all of the details for an specific sprint
        public async Task<ActionResult> Details(string projectId, int sprintId){

            var listaDeSprints = db.Sprints.Where(m => m.SprintID == sprintId && m.ProjectID == projectId);
            var proyectos = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.nombreProyecto = proyectos.First();

            if (listaDeSprints.Count() > 0)
            {
                var sprints = listaDeSprints.First();
                return View(sprints);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Displays a screen that allows the user to create an sprint on an speficic project
        public ActionResult Create(string projectId)
        {
            ViewBag.ProjectID = projectId;
            Sprint sprint = new Sprint();
            sprint.ProjectID = projectId;
            return View(sprint);
        }

        // POST: Stores the new sprints information on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                db.Sprints.Add(sprint);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = sprint.ProjectID;
            return View(sprint);
        }
        
        // GET: Displays a screen that allows the user to delete an sprint
        public async Task<ActionResult> Delete(string projectId, int sprintId)
        {
            var listaDeSprints = db.Sprints.Where(m => m.SprintID == sprintId && m.ProjectID == projectId);
            var proyectos = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.nombreProyecto = proyectos.First();

            if (listaDeSprints.Count() > 0)
            {
                var sprints = listaDeSprints.First();
                return View(sprints);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Deletes the specified sprint from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Sprint sprint = await db.Sprints.FindAsync(id);
            db.Sprints.Remove(sprint);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
