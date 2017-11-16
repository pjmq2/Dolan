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
using Microsoft.AspNet.Identity;

namespace IngeDolan3._0.Controllers
{
    public class ProjectsController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // Reviza los permisos que tiene el usuario para determinar si debe o no denegar el acceso del usuario
        private Boolean CanDo(string permission)
        {
            String userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                var modelUser = db.AspNetUsers.Where(x => x.Id == userId).ToList().FirstOrDefault();
                var user2 = modelUser.Users.FirstOrDefault();
                var userRole = user2.AspNetRole;
                var permisos = userRole.Permisos;

                //if found return true
                foreach (var per in permisos)
                {
                    if (per.nombre == permission)
                    {
                        return true;
                    }
                }
                //if it hasnt returned by now then it must be the user does not have permission
                return false;
            }
            else
            {
                return false;
            }
        }
        
        // Presenta la lista de todos los proyectos que han sido registrados en la página. 
        public ActionResult Index(int page = 1, string sort = "ProjectName", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetProjects(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        // Obtiene los proyectos presentes en la base de datos para llenar el índice.
        public List<Project> GetProjects(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = (from a in db.Projects
                     where
                        a.Descriptions.Contains(search) ||
                        a.ProjectName.Contains(search)
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
        
        // Presenta los detalles del proyecto que tenga el ID presentado como parámetro.
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project pROJECT = db.Projects.Find(id);
            if (pROJECT == null)
            {
                return HttpNotFound();
            }
            return PartialView(pROJECT);
        }
        
        // Prepara las listas de usuarios necesarias para presentar la pantalla donde se crea el proyecto.
        public ActionResult Create()
        {
            if (!CanDo("Crear Proyectos"))
            {
                return RedirectToAction("Denied", "Others");
            }
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.LeaderID = new SelectList(db.Users.Where(x => x.Project == null && x.AspNetRole.Id == EstID), "userID", "name");
            ViewBag.DesarrolladoresDisp = db.Users.Where(x => x.Project == null && x.AspNetRole.Id == EstID).ToList();

            return View();
        }
        
        // Crea el proyecto que se quiere insertar en la base de datos.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProject project)
        {
            if (ModelState.IsValid)
            {
                string id = DateTime.Now.ToString("MMddyyyy-hhmm-ssff-ffff-MMddyyyyhhmm");
                Project proyecto = new Project();
                Sprint sprint = new Sprint();
                User leaderUser = db.Users.Where(x => x.userID == project.LeaderID).FirstOrDefault();

                proyecto.StartingDate = project.StartingDate;
                proyecto.FinalDate = project.FinalDate;
                proyecto.Descriptions = project.Descriptions;
                proyecto.ProjectName = project.ProjectName;
                proyecto.ProjectID = id;
                leaderUser.Projects.Add(proyecto);

                sprint.Project = proyecto;
                sprint.SprintID = 1;
                sprint.StartingDate = project.StartingDate;
                sprint.FinalDate = project.FinalDate;
                db.Projects.Add(proyecto);
                db.Sprints.Add(sprint);
                
                if (project.IncludedUsers != null)
                {
                    foreach (var c in project.IncludedUsers)
                    {
                        User includedUser = db.Users.Where(x => x.userID == c).ToList().FirstOrDefault();
                        includedUser.Projects.Add(proyecto);
                    }
                }

                if(project.LeaderID != null)
                {
                    var f = db.Users.Where(x => x.userID == project.LeaderID).ToList().FirstOrDefault();
                    f.Project = proyecto;
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name", project.LeaderID);
            return View(project);
        }
        
        // Prepara la vista donde se editará el proyecto que tenga el ID presentado como parámetro.
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Proyecto = db.Projects.Find(id);
            CreateProject project = new CreateProject();
            User leader = db.Users.Where(x => x.Projects.FirstOrDefault().ProjectID == Proyecto.ProjectID).FirstOrDefault();
            project.LeaderID = Proyecto.User.userID;
            project.StartingDate = Proyecto.StartingDate;
            project.FinalDate = Proyecto.FinalDate;
            project.Descriptions = Proyecto.Descriptions;
            project.ProjectName = Proyecto.ProjectName;
            Proyecto.Pstate = project.Pstate;
            project.ProjectID = id;
            List<string> lista = new List<string>();
            List<User> listaU = db.Users.Where(x => x.Project.ProjectID == id).ToList();

            if (listaU != null)
            {
                foreach (var c in listaU)
                {
                    string Nombre = c.userID;
                    lista.Add(Nombre);
                }
            }

            project.IncludedUsers = lista;
            project.PrevEditList = lista;

            if (project == null)
            {
                return HttpNotFound();
            }

            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.LeaderID = new SelectList((db.Users.Where(x => (x.Project == null || x.Projects.Where(y => y.ProjectID == id).FirstOrDefault().ProjectID == id) && x.AspNetRole.Id == EstID)), "userID", "name");
            ViewBag.DesarrolladoresDisp = (db.Users.Where(x => (x.Project == null || x.Project.ProjectID == id) && x.AspNetRole.Id == EstID)).ToList();
            return View(project);
        }
        
        // Guarda los cambios solicitados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateProject project, string id)
        {
            if (ModelState.IsValid)
            {
                var Proyecto = db.Projects.Where(x => x.ProjectID == project.ProjectID).ToList().FirstOrDefault();
                var oldleader = db.Users.Where(x => x.AspNetUser.Id == Proyecto.User.AspNetUser.Id).FirstOrDefault();
                oldleader.Projects.Clear();
                var newleader = db.Users.Where(x => x.userID == project.LeaderID).FirstOrDefault();
                Proyecto.StartingDate = project.StartingDate;
                Proyecto.FinalDate = project.FinalDate;
                Proyecto.Descriptions = project.Descriptions;
                Proyecto.ProjectName = project.ProjectName;
                Proyecto.Pstate = project.Pstate;
                newleader.Projects.Add(Proyecto);

                if (project.IncludedUsers != null && project.PrevEditList != null)
                {
                    List<string> excludedOnes = project.PrevEditList.Except(project.IncludedUsers).ToList();
                    List<string> newOnes = project.IncludedUsers.Except(project.PrevEditList).ToList();
                    
                    foreach (var c in newOnes)
                    {
                        var f = db.Users.Where(x => x.userID == c).ToList().FirstOrDefault();
                        f.Project = Proyecto;
                    }

                    foreach (var c in excludedOnes)
                    {
                        var f = db.Users.Where(x => x.userID == c).ToList().FirstOrDefault();
                        f.Projects.Clear();
                    }
                }
                else if(project.IncludedUsers != null)
                {
                    foreach (var c in project.IncludedUsers)
                    {
                        var f = db.Users.Where(x => x.userID == c).ToList().FirstOrDefault();
                        f.Project = Proyecto;
                    }
                }

                if (project.LeaderID != null)
                {
                    var f = db.Users.Where(x => x.userID == project.LeaderID).ToList().FirstOrDefault();
                    Proyecto.Users.Add(f);

                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            string EstID = (db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault()).Id;
            ViewBag.LeaderID = new SelectList((db.Users.Where(x => (x.Projects == null || x.Projects.FirstOrDefault().ProjectID == id) && x.AspNetRole.Id == EstID)), "userID", "name");
            return View(project);
        }
        
        // Presenta la vista que le pregunta al usuario si está seguro de que quiere borrar el proyecto.
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        
        // Este método borra al proyecto de la base de datos, junto con sus historias de usuario.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Project pro = db.Projects.Where(x => x.ProjectID == id).ToList().FirstOrDefault();
            List<User> modelUser = (pro).Users.ToList();
            List<UserStory> modelstory = db.UserStories.Where(x => x.ProjectID == id).ToList();
            if (modelUser != null)
            {
                pro.Users.Clear();
                db.SaveChanges();
            }
            if (modelstory != null)
            {
                foreach (var c in modelstory)
                {
                    db.UserStories.Remove(c);
                    db.SaveChanges();
                }
            }
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Hace que este control sea inutilizable.
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
