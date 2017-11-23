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
    public class ProjectTasksController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();

        // Displays a list of all of the project tasks for an specific project
        public async Task<ActionResult> Index(String projectId, string storyId)
        {
            var projectTasks = db.ProjectTasks.Include(p => p.UserStory);
            return View(await projectTasks.ToListAsync());
        }

        // Displays all of the details of a particular project task
        public async Task<ActionResult> Details(String projectId, string storyId, string taskId){
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

        // Get method to display the project task create screen
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo");
            return View();
        }

        // Post method to store all of the new project task data to the database
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

        // Display a screen that allows the user to edit project tasks
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

       //Saves the edited project task to the database
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

        // Method to display a screen for deleting an specific project task
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

        // Deletes the specified project task from the database
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
