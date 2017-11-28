using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Linq.Dynamic;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using IngeDolan3._0.Generator;

namespace IngeDolan3._0.Controllers
{
    public class ScenariosController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Displays the scenarios list that belong to a specific project
        /*public async Task<ActionResult> Index(String projectId, string storyId)
        {
            var scenarios = db.Scenarios.Include(s => s.UserStory);
            return View(await scenarios.ToListAsync());
        }*/

        public ActionResult Index(string projectId, string storyId)
        {
            int page = 1;
            string sort = "ScenarioNumber";
            string sortdir = "asc";
            string search = "";
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetScenarios(search, sort, sortdir, skip, pageSize, out totalRecord, projectId, storyId);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.ProyectId = projectId;
            ViewBag.StoryId = storyId;
            return View("Index", data);
        }

        // Obtiene los proyectos presentes en la base de datos para llenar el índice.
        public List<Scenario> GetScenarios(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string idProyecto, string idHistoria)
        {
            var v = (from a in db.Scenarios
                     where
                        a.ProjectID.Equals(idProyecto) && a.StoryID.Equals(idHistoria) 
                     select a
                        );
            totalRecord = v.Count();
            v = v.OrderBy(sort + " " + sortdir);
            if (pageSize > 0)
            {
                v = v.Skip(skip).Take(pageSize);
            }
            return v.ToList();
        }

        // GET: Displays a list of the details for an specific scenario
        public async Task<ActionResult> Details(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = db.Scenarios.Where(x => x.ScenarioNumber.ToString() == ScenarioId).ToList().FirstOrDefault();
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // GET: Displays a screen that allows the user to create an scenario
        public ActionResult Create(string projectId, string storyId)
        {
            int sprintId = db.UserStories.Where(x => x.StoryID == storyId).ToList().FirstOrDefault().SprintID;
            var pl = new Scenario();
            //int ID = db.Scenarios.Where(x => x.StoryID == storyId).ToList().ForEach;
            pl.ProjectID = projectId;
            pl.StoryID = storyId;
            pl.SprintID = sprintId;
            pl.ScenarioNumber = 1;
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;

            ViewBag.UserID = new SelectList(db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == projectId).FirstOrDefault().ProjectID == projectId) && x.AspNetRole.Id == EstID), "userID", "name");

            return View(pl);
        }

        // Stores the new scenario on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                db.Scenarios.Add(scenario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = scenario.ProjectID, storyId = scenario.StoryID });
            }

            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.UserID = new SelectList(db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == scenario.ProjectID).FirstOrDefault().ProjectID == scenario.ProjectID) && x.AspNetRole.Id == EstID), "userID", "name");

            return View(scenario);
        }

        // Displays a screen that lets the user edit an scenario
        public ActionResult Edit(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null || projectId == null || storyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario old = db.Scenarios.Where(x => x.ScenarioNumber.ToString() == ScenarioId).ToList().FirstOrDefault();
            if (old == null)
            {
                return HttpNotFound();
            }
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.UserID = new SelectList(db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == projectId).FirstOrDefault().ProjectID == projectId) && x.AspNetRole.Id == EstID), "userID", "name");

            return View(old);
        }

        // POST: Stores the edited scenario information on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index", new { projectId = scenario.ProjectID, storyId = scenario.StoryID });
            }

            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.UserID = new SelectList(db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == scenario.ProjectID).FirstOrDefault().ProjectID == scenario.ProjectID) && x.AspNetRole.Id == EstID), "userID", "name");

            return View(scenario);
        }

        // GET: Displays a screen that allows an user to delete an scenario
        public async Task<ActionResult> Delete(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = db.Scenarios.Where(x => x.ScenarioNumber.ToString() == ScenarioId).ToList().FirstOrDefault();
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // Deletes the specified scenario from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String projectId, string storyId, string ScenarioId)
        {
            Scenario scenario = db.Scenarios.Where(x => x.ScenarioNumber.ToString() == ScenarioId).ToList().FirstOrDefault();
            db.Scenarios.Remove(scenario);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = scenario.ProjectID, storyId = scenario.StoryID });
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
