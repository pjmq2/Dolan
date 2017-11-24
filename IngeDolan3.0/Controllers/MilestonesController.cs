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
using System.Linq.Dynamic;

namespace IngeDolan3._0.Controllers
{
    public class MilestonesController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Milestones
        public ActionResult Index(string projectId, string storyId, string taskId)
        {
            int page = 1;
            string sort = "TaskID";
            string sortdir = "asc";
            string search = "";
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetMilestones(search, sort, sortdir, skip, pageSize, out totalRecord, taskId);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.ProyectId = projectId;
            ViewBag.StoryId = storyId;
            ViewBag.TaskId = taskId;
            var v = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.ProyectoNombre = v.First().ProjectName;
            return View("Index", data);
        }

        public List<Milestone> GetMilestones(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string idTask)
        {
            var v = (from a in db.Milestones
                     where
                        a.TaskID.Equals(idTask) && (
                            a.StoryID.Contains(search) ||
                            a.ProjectID.Contains(search)
                        )
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

        // GET: Milestones/Details/5
        public async Task<ActionResult> Details(string taskid, DateTime date)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Where(x => x.Date == date && x.TaskID == taskid).ToList().FirstOrDefault();
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // GET: Milestones/Create
        public ActionResult Create(string projectId, string storyId, string taskId)
        {
            int sprintId = db.UserStories.Where(x => x.StoryID == storyId).ToList().FirstOrDefault().SprintID;
            var pl = new Milestone();
            pl.ProjectID = projectId;
            pl.StoryID = storyId;
            pl.SprintID = sprintId;
            pl.TaskID = taskId;
            return View(pl);
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Milestones.Add(milestone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = milestone.ProjectID, storyId = milestone.StoryID, taskId = milestone.TaskID });
            }
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public async Task<ActionResult> Edit(string projectId, string storyId, string taskId, DateTime date)
        {
            if (date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Where(x => x.Date == date && x.TaskID == taskId && x.StoryID == storyId && x.ProjectID == projectId).ToList().FirstOrDefault();
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = milestone.ProjectID, storyId = milestone.StoryID, taskId = milestone.TaskID });
            }
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
            return RedirectToAction("Index", new { projectId = milestone.ProjectID, storyId = milestone.StoryID, taskId = milestone.TaskID });
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