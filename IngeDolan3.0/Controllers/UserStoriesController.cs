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
        [HttpPost, ActionName("Index")]
        public ActionResult Index(GenericList projectId, int page = 1, string sort = "StoryID", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetUsers(search, sort, sortdir, skip, pageSize, out totalRecord, projectId.id);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.ProyectoId = projectId.id;
            var v = db.Projects.Where(m => m.ProjectID == projectId.id);
            ViewBag.ProyectoNombre = v.First().ProjectName;
            return View("Index", data);
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
            var proyectos = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.nombreProyecto = proyectos.First();
            
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
            List<Sprint> data = new List<Sprint>();
            var v = db.Sprints.Where(m => m.ProjectID == projectId);
            data = v.ToList();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var c in data)
            {
                item.Add(new SelectListItem
                {
                    Text = c.SprintID.ToString(),
                    Value = c.SprintID.ToString()
                });
            }

            var pl = new UserStoryInt();
            pl.ListaSprints = item;
            
            string ID = DateTime.Now.ToString("MMddyyyy-hhmm-ssff-ffff-MMddyyyyhhmm");
            pl.ProjectID = projectId;
            pl.StoryID = ID;
            ViewBag.proyectoId = projectId;
            ViewBag.Sprints = new SelectList(db.Sprints.Where(x => x.ProjectID == projectId), "SprintID", "SprintID");
            return View(pl);
        }

        // Confirma la creación de la historia de usuario.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserStoryInt userStory)
        {
            if (ModelState.IsValid)
            {
                UserStory userStoryX = new UserStory();
                userStoryX.ProjectID = userStory.ProjectID;
                userStoryX.SprintID = userStory.SprintID;
                userStoryX.StoryID = userStory.StoryID;
                userStoryX.Modulo = userStory.Modulo;
                userStoryX.ID = userStory.ID;
                userStoryX.Priorities = userStory.Priorities;
                userStoryX.ClientRole = userStory.ClientRole;
                userStoryX.Estimation = userStory.Estimation;
                userStoryX.Reason = userStory.Reason;
                userStoryX.Funtionality = userStory.Funtionality;
                userStoryX.Alias = userStory.Alias;
                
                
                userStoryX.ProjectTasks = userStory.ProjectTasks;
                userStoryX.Scenarios = userStory.Scenarios;
                userStoryX.Sprint = db.Sprints.Where(m => m.SprintID == userStory.SprintID && m.ProjectID == userStory.ProjectID).ToList().FirstOrDefault();
                
                
                db.UserStories.Add(userStoryX);
                db.SaveChanges();
            }
            
            var pl = new GenericList();
            pl.id = userStory.ProjectID;
            return Index(projectId: pl);
        }

        // Prepara la vista donde se editará la historia usuario que tenga el ID presentado como parámetro.
        public ActionResult Edit(string storyId, string projectId)
        {
            if (String.IsNullOrEmpty(storyId) || String.IsNullOrEmpty(projectId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Sprint> data = new List<Sprint>();
            var v = db.Sprints.Where(m => m.ProjectID == projectId);
            data = v.ToList();
            List<SelectListItem> item = new List<SelectListItem>();
            foreach (var c in data)
            {
                item.Add(new SelectListItem
                {
                    Text = c.SprintID.ToString(),
                    Value = c.SprintID.ToString()
                });
            }
            
            var listaDeHistorias = db.UserStories.Where(m => m.StoryID == storyId && m.ProjectID == projectId);
            var proyectos = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.nombreProyecto = proyectos.First();
            if (listaDeHistorias.Count() > 0)
            {
                var userStories = listaDeHistorias.First();
                UserStoryInt pl = ConvertirInt(userStories,item);
                return View(pl);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public UserStoryInt ConvertirInt(UserStory userStory, List<SelectListItem> lista)
        {
            var usi = new UserStoryInt();
            usi.ListaSprints = lista;
            usi.Alias = userStory.Alias;
            usi.ClientRole = userStory.ClientRole;
            usi.Estimation = userStory.Estimation;
            usi.Funtionality = userStory.Funtionality;
            usi.ID = userStory.ID;
            usi.Modulo = userStory.Modulo;
            usi.Priorities = userStory.Priorities;
            usi.ProjectID = userStory.ProjectID;
            usi.ProjectTasks = userStory.ProjectTasks;
            usi.Reason = userStory.Reason;
            usi.Scenarios = userStory.Scenarios;
            usi.Sprint = userStory.Sprint;
            usi.SprintID = userStory.SprintID;
            usi.StoryID = userStory.StoryID;
            return usi;
        }

        // Guarda los cambios solicitados.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserStoryInt userStory)
        {
            if (ModelState.IsValid)
            {
                var userStoryX = db.UserStories.Where(x => x.ProjectID == userStory.ProjectID && x.SprintID == userStory.SprintID && x.StoryID == userStory.StoryID).ToList().FirstOrDefault();
                userStoryX.ProjectID = userStory.ProjectID;
                userStoryX.SprintID = userStory.SprintID;
                userStoryX.StoryID = userStory.StoryID;
                userStoryX.Modulo = userStory.Modulo;
                userStoryX.ID = userStory.ID;
                userStoryX.Priorities = userStory.Priorities;
                userStoryX.ClientRole = userStory.ClientRole;
                userStoryX.Estimation = userStory.Estimation;
                userStoryX.Reason = userStory.Reason;
                userStoryX.Funtionality = userStory.Funtionality;
                userStoryX.Alias = userStory.Alias;


                userStoryX.ProjectTasks = userStory.ProjectTasks;
                userStoryX.Scenarios = userStory.Scenarios;
                userStoryX.Sprint = db.Sprints.Where(m => m.SprintID == userStory.SprintID && m.ProjectID == userStory.ProjectID).ToList().FirstOrDefault(); ;
                
                db.SaveChanges();
            }
            var pl = new GenericList();
            pl.id = userStory.ProjectID;
            return Index(projectId: pl);
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
            var proyectos = db.Projects.Where(m => m.ProjectID == projectId);
            ViewBag.nombreProyecto = proyectos.First();
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
        public ActionResult DeleteConfirmed(string storyId, string projectId)
        {
            UserStory userStory = db.UserStories.Where(m => m.StoryID == storyId && m.ProjectID == projectId).First();
            db.UserStories.Remove(userStory);
            db.SaveChanges();
            GenericList pl = new GenericList();
            pl.id = projectId;
            return Index(projectId: pl);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(string storyId, string projectId)
        {
            GenericList pl = new GenericList();
            pl.id = projectId;
            return Index(projectId: pl);
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
