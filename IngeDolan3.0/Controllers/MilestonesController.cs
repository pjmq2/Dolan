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
    public class MilestonesController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Milestones
        public async Task<ActionResult> Index()
        {
            var milestones = db.Milestones.Include(m => m.ProjectTask);
            return View(await milestones.ToListAsync());
        }

        // GET: Milestones/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // GET: Milestones/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.ProjectTasks, "ProjectID", "Descripcion");
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProjectID,SprintID,StoryID,TaskID,Date,Progress")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Milestones.Add(milestone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.ProjectTasks, "ProjectID", "Descripcion", milestone.ProjectID);
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.ProjectTasks, "ProjectID", "Descripcion", milestone.ProjectID);
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProjectID,SprintID,StoryID,TaskID,Date,Progress")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.ProjectTasks, "ProjectID", "Descripcion", milestone.ProjectID);
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Milestone milestone = await db.Milestones.FindAsync(id);
            db.Milestones.Remove(milestone);
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
