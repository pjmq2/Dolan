﻿using System;
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

        private Boolean revisarPermisos(string permission)
        {
            String userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                var modelUser = db.Users.Where(x => x.id == userId).ToList().First();
                var userRole = modelUser.AspNetRole;
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
            if (!revisarPermisos("Crear Proyectos"))
            {
                return RedirectToAction("Denied", "Other");
            }

            ViewBag.LeaderID = new SelectList(db.Users.Where(x => x.ProjectID == null), "userID", "name");
            ViewBag.DesarrolladoresDisp = (db.Users.Where(x => x.ProjectID == null)).ToList();

            return View();
        }
        
        // Crea el proyecto que se quiere insertar en la base de datos.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,StartingDate,FinalDate,Descriptions,ProjectName,LeaderID,IncludedUsers")] CreateProject project)
        {
            if (ModelState.IsValid)
            {
                string cuenta = db.Projects.Max(x => x.ProjectID);
                int number;
                string id = DateTime.Now.ToString("MMddyyyy-hhmm-ssff-ffff-MMddyyyyhhmm");
                Project proyecto = new Project();
                proyecto.LeaderID = project.LeaderID;
                proyecto.StartingDate = project.StartingDate;
                proyecto.FinalDate = project.FinalDate;
                proyecto.Descriptions = project.Descriptions;
                proyecto.ProjectName = project.ProjectName;
                proyecto.ProjectID = id;
                db.Projects.Add(proyecto);
                db.SaveChanges();

                if (project.IncludedUsers != null)
                {
                    foreach (var c in project.IncludedUsers)
                    {
                        var f = db.Users.Where(x => x.id == c).ToList().FirstOrDefault();
                        f.ProjectID = id;
                        db.SaveChanges();
                    }
                }
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name", project.LeaderID);
            return View(project);
        }
        
        // Guarda los cambios solicitados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,StartingDate,FinalDate,Descriptions,ProjectName,LeaderID")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name", project.LeaderID);
            return View(project);
        }
        
        // Presenta la vista que le pregunta al usuario si está seguro de que quiere borrar el proyecto
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
        
        // Este método borra al proyecto de la base de datos, junto con sus historias de usuario
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            List<User> modelUser = db.Users.Where(x => x.ProjectID == id).ToList();
            List<UserStory> modelstory = db.UserStories.Where(x => x.ProjectID == id).ToList();
            if (modelUser != null)
            {
                foreach (var c in modelUser)
                {
                    c.ProjectID = null;
                    db.SaveChanges();
                }
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
