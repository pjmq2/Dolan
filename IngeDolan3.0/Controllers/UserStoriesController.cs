using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;

namespace IngeDolan3._0.Controllers
{
    public class UserStoriesController : Controller
    {
        private dolan2Entities db = new dolan2Entities();

        // GET: UserStories
        public ActionResult Index()
        {
            var userStories = db.UserStories.Include(u => u.Project);
            return View(userStories.ToList());
        }

        // GET: UserStories/Details/5
        public ActionResult Details(string storyId, string projectId)
        {
            if (String.IsNullOrEmpty(storyId) || String.IsNullOrEmpty(projectId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userStory = new UserStory();
            userStory.StoryID = storyId;
            userStory.ProjectID = projectId;
            var listaDeHistorias = db.UserStories.Where(m => m.StoryID == userStory.StoryID && m.ProjectID == projectId);
            
            if (listaDeHistorias.Count() > 0)
            {
                var userStories = listaDeHistorias.First();
                return View(userStory);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: UserStories/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions");
            return View();
        }

        // POST: UserStories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StoryID,ProjectID,SprintID,Priorities,ClientRole,Estimation,Reason,Funtionality,Alias")] UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                db.UserStories.Add(userStory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", userStory.ProjectID);
            return View(userStory);
        }

        // GET: UserStories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStory userStory = db.UserStories.Find(id);
            if (userStory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", userStory.ProjectID);
            return View(userStory);
        }

        // POST: UserStories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoryID,ProjectID,SprintID,Priorities,ClientRole,Estimation,Reason,Funtionality,Alias")] UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userStory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", userStory.ProjectID);
            return View(userStory);
        }

        // GET: UserStories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserStory userStory = db.UserStories.Find(id);
            if (userStory == null)
            {
                return HttpNotFound();
            }
            return View(userStory);
        }

        // POST: UserStories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserStory userStory = db.UserStories.Find(id);
            db.UserStories.Remove(userStory);
            db.SaveChanges();
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
