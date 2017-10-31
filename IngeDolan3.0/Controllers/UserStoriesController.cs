using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using System.Linq.Dynamic;

namespace IngeDolan3._0.Controllers
{
    public class UserStoriesController : Controller
    {
        private dolan2Entities db = new dolan2Entities();


        //Oh snap!
        public ActionResult Index(ProyectoList projecto, int page = 1, string sort = "StoryID", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetUsers(search, sort, sortdir, skip, pageSize, out totalRecord, projecto.id);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        public List<UserStory> GetUsers(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string idProyecto)
        {
            var v = (from a in db.UserStories
                     where
                        a.ProjectID.Equals(idProyecto) && (
                            a.StoryID.Contains(search) ||
                            a.Alias.Contains(search) ||
                            a.Funtionality.Contains(search) ||
                            a.Reason.Contains(search)
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

        //Oh jeez
        // GET: UserStories
        

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
                return View(userStories);
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
        public ActionResult Edit(string storyId, string projectId)
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
                ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", userStory.ProjectID);
                var userStories = listaDeHistorias.First();
                return View(userStory);
            }
            else
            {
                return HttpNotFound();
            }
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
        public ActionResult Delete(string storyId, string projectId)
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
                ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "Descriptions", userStory.ProjectID);
                var userStories = listaDeHistorias.First();
                return View(userStory);
            }
            else
            {
                return HttpNotFound();
            }
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
