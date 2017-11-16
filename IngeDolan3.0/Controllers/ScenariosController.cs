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

namespace IngeDolan3._0.Controllers
{
    public class ScenariosController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Scenarios
        public async Task<ActionResult> Index()
        {
            var scenarios = db.Scenarios.Include(s => s.UserStory);
            return View(await scenarios.ToListAsync());
        }

        // GET: Scenarios/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // GET: Scenarios/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo");
            return View();
        }

        // POST: Scenarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProjectID,SprintID,StoryID,ScenarioNumber,Resultado,Event,AcceptanceCriteria,Context")] Scenario scenario)
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

        // GET: Scenarios/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // POST: Scenarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProjectID,SprintID,StoryID,ScenarioNumber,Resultado,Event,AcceptanceCriteria,Context")] Scenario scenario)
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

        // GET: Scenarios/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // POST: Scenarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Scenario scenario = await db.Scenarios.FindAsync(id);
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
