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
    public class ScenariosController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Displays the scenarios list that belong to a specific project
        public async Task<ActionResult> Index(String projectId, string storyId)
        {
            var scenarios = db.Scenarios.Include(s => s.UserStory);
            return View(await scenarios.ToListAsync());
        }

        // GET: Displays a list of the details for an specific scenario
        public async Task<ActionResult> Details(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // GET: Displays a screen that allows the user to create an scenario
        public ActionResult Create(){
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo");
            return View();
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
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // Displays a screen that lets the user edit an scenario
        public async Task<ActionResult> Edit(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // POST: Stores the edited scenario information on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scenario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // GET: Displays a screen that allows an user to delete an scenario
        public async Task<ActionResult> Delete(String projectId, string storyId, string ScenarioId)
        {
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
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
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            db.Scenarios.Remove(scenario);
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
