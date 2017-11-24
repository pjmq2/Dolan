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
    public class SprintsController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Sprints
        public async Task<ActionResult> Index(string projectId)
        {
            ViewBag.ProjectID = projectId;
            var sprints = db.Sprints.Where(m => m.ProjectID == projectId);
            return View(await sprints.ToListAsync());
        }

        // GET: Sprints/Details/5
        public async Task<ActionResult> Details(string projectId, string SprintId)
        {
            if (SprintId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = await db.Sprints.FindAsync(SprintId);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            return View(sprint);
        }

        // GET: Sprints/Create
        public ActionResult Create(string projectId)
        {
            ViewBag.ProjectID = projectId;
            Sprint sprint = new Sprint();
            sprint.ProjectID = projectId;
            return View(sprint);
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Sprints/Edit/5
        public async Task<ActionResult> Edit(string projectId, string SprintId)
        {
            if (SprintId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = await db.Sprints.FindAsync(SprintId);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", sprint.ProjectID);
            return View(sprint);
        }

        // POST: Sprints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sprint).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", sprint.ProjectID);
            return View(sprint);
        }

        // GET: Sprints/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sprint sprint = await db.Sprints.FindAsync(id);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            return View(sprint);
        }

        // POST: Sprints/Delete/5
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
