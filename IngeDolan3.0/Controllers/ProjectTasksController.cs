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
    public class ProjectTasksController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: ProjectTasks
        /*public async Task<ActionResult> Index(String projectId, string storyId)
        {
            var projectTasks = db.ProjectTasks.Include(p => p.UserStory);
            return View(await projectTasks.ToListAsync());
        }*/
        
        public ActionResult Index(string projectId, string storyId)
        {
            int page = 1;
            string sort = "TaskID";
            string sortdir = "asc";
            string search = "";
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetTasks(search, sort, sortdir, skip, pageSize, out totalRecord, projectId, storyId);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.ProyectId = projectId;
            ViewBag.StoryId = storyId;
            var v = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.ProyectoNombre = v.First().ProjectName;
            return View("Index", data);
        }

        // Obtiene los proyectos presentes en la base de datos para llenar el índice.
        public List<ProjectTask> GetTasks(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string idProyecto, string idHistoria)
        {
            var v = (from a in db.ProjectTasks
                     where
                        a.ProjectID.Equals(idProyecto) && a.StoryID.Equals(idHistoria) && (
                            a.StoryID.Contains(search) ||
                            a.Descripcion.Contains(search) ||
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

        // GET: ProjectTasks/Details/5
        public async Task<ActionResult> Details(string projectId, string storyId, string taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Where(x => x.TaskID == taskId).ToList().FirstOrDefault();
            ViewBag.Username = projectTask.User.name;
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create(string projectId, string storyId)
        {
            int sprintId = db.UserStories.Where(x => x.StoryID == storyId).ToList().FirstOrDefault().SprintID;
            var pl = new ProjectTaskInt();
            string ID = DateTime.Now.ToString("MMddyyyy-hhmm-ssff-ffff-MMddyyyyhhmm");
            pl.ProjectID = projectId;
            pl.StoryID = storyId;
            pl.SprintID = sprintId;
            pl.TaskID = ID;
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.UserID = new SelectList((db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == projectId).FirstOrDefault().ProjectID == projectId) && x.AspNetRole.Id == EstID)), "userID", "name");
            return View(pl);
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectTaskInt projectTask)
        {
            if (ModelState.IsValid)
            {
                ProjectTask newTask = new ProjectTask();
                newTask.ProjectID = projectTask.ProjectID;
                newTask.StoryID = projectTask.StoryID;
                newTask.SprintID = projectTask.SprintID;
                newTask.TaskID = projectTask.TaskID;
                newTask.Descripcion = projectTask.Descripcion;
                newTask.Estado = projectTask.Estado;
                newTask.EstimateTime = projectTask.EstimateTime;
                newTask.Priority = projectTask.Priority;
                newTask.UserStory = db.UserStories.Where(x => x.StoryID == projectTask.StoryID).ToList().FirstOrDefault();
                newTask.User = db.Users.Where(x => x.userID == projectTask.UserID).ToList().FirstOrDefault();
                db.ProjectTasks.Add(newTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = projectTask.ProjectID, storyId = projectTask.StoryID });
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        public ActionResult Edit(string projectId, string storyId, string taskId)
        {
            if (taskId == null || projectId == null || storyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Where(x => x.TaskID == taskId).ToList().FirstOrDefault();
            ProjectTaskInt old = new ProjectTaskInt();
            old.ProjectID = projectTask.ProjectID;
            old.StoryID = projectTask.StoryID;
            old.SprintID = projectTask.SprintID;
            old.TaskID = projectTask.TaskID;
            old.Descripcion = projectTask.Descripcion;
            old.Estado = projectTask.Estado;
            old.EstimateTime = projectTask.EstimateTime;
            old.Priority = projectTask.Priority;
            old.UserStory = db.UserStories.Where(x => x.StoryID == projectTask.StoryID).ToList().FirstOrDefault();
            old.User = db.Users.Where(x => x.userID == projectTask.User.userID).ToList().FirstOrDefault();
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.UserID = new SelectList((db.Users.Where(x => (x.Projects.Where(y => y.ProjectID == projectId).FirstOrDefault().ProjectID == projectId) && x.AspNetRole.Id == EstID)), "userID", "name");
            return View(old);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProjectTaskInt projectTask)
        {
            if (ModelState.IsValid)
            {
                ProjectTask old = db.ProjectTasks.Where(x => x.TaskID == projectTask.TaskID).ToList().FirstOrDefault();
                old.ProjectID = projectTask.ProjectID;
                old.StoryID = projectTask.StoryID;
                old.SprintID = projectTask.SprintID;
                old.TaskID = projectTask.TaskID;
                old.Descripcion = projectTask.Descripcion;
                old.Estado = projectTask.Estado;
                old.EstimateTime = projectTask.EstimateTime;
                old.Priority = projectTask.Priority;
                old.UserStory = db.UserStories.Where(x => x.StoryID == projectTask.StoryID).ToList().FirstOrDefault();
                old.User = db.Users.Where(x => x.userID == projectTask.UserID).ToList().FirstOrDefault();
                db.SaveChanges();
                return RedirectToAction("Index", new { projectId = projectTask.ProjectID, storyId = projectTask.StoryID });
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", projectTask.ProjectID);
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public async Task<ActionResult> Delete(string projectId, string storyId, string taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.ProjectTasks.Where(x => x.TaskID == taskId).ToList().FirstOrDefault();
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string projectId, string storyId, string taskId)
        {
            ProjectTask projectTask = db.ProjectTasks.Where(x => x.TaskID == taskId).ToList().FirstOrDefault();
            db.ProjectTasks.Remove(projectTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = projectTask.ProjectID, storyId = projectTask.StoryID });
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
