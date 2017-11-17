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
        private NewDolan2Entities db = new NewDolan2Entities();


        // Presenta la lista de todas las historias de usuario que han sido registradas en la página
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

        // Obtiene los usuarios presentes en la base de datos.
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

        // Presenta los detalles de la historia de usuario que tenga el ID de proyecto e historia presentados como parámetros.
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

        // Presenta la pantalla donde se crea la historia de usuario.
        public ActionResult Create(string projectId)
        {
            ViewBag.Sprints = new SelectList(db.Sprints.Where(x => x.ProjectID == projectId), "SprintID", "Sprint");
            return View();
        }

        // Confirma la creación de la historia de usuario.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserStory userStory)
        {
            if (ModelState.IsValid)
            {
                db.UserStories.Add(userStory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectID", userStory.ProjectID);
            return View(userStory);
        }

        // Prepara la vista donde se editará la historia usuario que tenga el ID presentado como parámetro.
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
                var userStories = listaDeHistorias.First();
                return View(userStories);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // Guarda los cambios solicitados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserStory userStory)
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

        // Presenta la vista que le pregunta al usuario si está seguro de que quiere borrar la historia de usuario.
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
                var userStories = listaDeHistorias.First();
                return View(userStories);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // Este método borra la historia de usuario que tenga el id presentado de la base de datos.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserStory userStory = db.UserStories.Find(id);
            db.UserStories.Remove(userStory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Hace que este control sea inutilizable
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
