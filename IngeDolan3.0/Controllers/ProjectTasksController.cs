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

namespace IngeDolan3._0.Controllers
{
    public class ProjectTasksController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: ProjectTasks
        /*public async Task<ActionResult> Index(String projectId, string storyId)
        {
            var projectTasks = db.ProjectTasks.Include(p => p.UserStory);
            return View(await projectTasks.ToListAsync());
        }*/

        public ActionResult Index(int page = 1, string sort = "EstimateTime", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetTasks(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        // Obtiene los proyectos presentes en la base de datos para llenar el índice.
        public List<ProjectTask> GetTasks(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = (from a in db.ProjectTasks
                     where
                        a.Descripcion.Contains(search)
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

        // GET: ProjectTasks/Details/5
        public async Task<ActionResult> Details(String projectId, string storyId, string taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(taskId);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProjectID,SprintID,StoryID,taskId,Descripcion,EstimateTime,Priority,Estado")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTasks.Add(projectTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        public async Task<ActionResult> Edit(String projectId, string storyId, string taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(taskId);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", projectTask.ProjectID);
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProjectID,SprintID,StoryID,taskId,Descripcion,EstimateTime,Priority,Estado")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public async Task<ActionResult> Delete(String projectId, string storyId, string taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(taskId);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String projectId, string storyId, string taskId)
        {
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(taskId);
            db.ProjectTasks.Remove(projectTask);
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
