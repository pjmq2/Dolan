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
    public class ProjectsController : Controller
    {
        private dolan2Entities db = new dolan2Entities();
        ApplicationDbContext context = new ApplicationDbContext();

        private bool revisarPermisos(string permiso)
        {
            //
            //String userID = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var rol = context.Users.Find(userID).Roles.First();
            //var permisoID = db.Permisos.Where(m => m.nombre == permiso).First().codigo;
            //var listaRoles = db..Where(m => m.permiso == permisoID).ToList().Select(n => n.rol);
            //bool userRol = listaRoles.Contains(rol.RoleId);
            //
            return true;
        }

        // GET: Projects
        //Oh snap!
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

        public List<Project> GetProjects(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = (from a in db.Project
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

        //Oh jeez

        // GET: PROJECTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project pROJECT = db.Project.Find(id);
            if (pROJECT == null)
            {
                return HttpNotFound();
            }
            return PartialView(pROJECT);
        }

        // GET: Crear
        public ActionResult Create()
        {
            if (!revisarPermisos("Crear Proyectos"))
            {
                return RedirectToAction("Denied", "Other");
            }
            List<Users> listaDesarrolladores = new List<Users>();
            List<Users> listaClientes = new List<Users>();

            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name");
            ViewBag.DesarrolladoresDisp = db.Users.ToList();
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartingDate,FinalDate,Descriptions,ProjectName,LeaderID")] Project project)
        {
            if (ModelState.IsValid)
            {
                int cuenta = db.Project.Count();
                IDGenerator generador = new IDGenerator();
                string id = generador.IntToString(cuenta);
                project.ProjectID = id;
                db.Project.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name", project.LeaderID);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeaderID = new SelectList(db.Users, "userID", "name", project.LeaderID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Projects/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Project project = db.Project.Find(id);
            var vps = db.UserStory.Where(a => a.ProjectID == id).ToList();
            foreach (var vp in vps)
            {
                db.UserStory.Remove(vp);
            }
            db.Project.Remove(project);
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
