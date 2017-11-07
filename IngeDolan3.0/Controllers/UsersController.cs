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
    public class UsersController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // Presenta la lista de todos los usuarios que han sido registrados en la página
        public ActionResult Index(int page = 1, string sort = "name", string sortdir = "asc", string search = "")
        {
            if (CanDo("Consular Lista de Usuarios")){
                int pageSize = 10;
                int totalRecord = 0;
                if (page < 1) page = 1;
                int skip = (page * pageSize) - pageSize;
                var data = GetUsers(search, sort, sortdir, skip, pageSize, out totalRecord);
                ViewBag.TotalRows = totalRecord;
                ViewBag.search = search;
                return View(data);
            }
            else{
                Console.WriteLine("Usuario no puede listar usuarios");
                return PartialView("~/Views/Others/Denied.cshtml");
            }

        }

        // Reviza los permisos que tiene el usuario para determinar si debe o no denegar el acceso del usuario
        public Boolean CanDo(string permission){
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

        // Obtiene los usuarios presentes en la base de datos para llenar el índice.
        public List<User> GetUsers(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            var v = (from a in db.Users
                     where
                        a.name.Contains(search) ||
                        a.firstLastName.Contains(search) ||
                        a.secondLastName.Contains(search)
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
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);
        }

        // Presenta la pantalla donde se crea el usuario.
        public ActionResult Create()
        {
            return View();
        }

        // Confirma la creación del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,firstLastName,secondLastName,userID,id,role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // Prepara la vista donde se editará el usuario que tenga el ID presentado como parámetro.
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", user.id);
            return View(user);
        }

        // Guarda los cambios solicitados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "name,firstLastName,secondLastName,userID,id,role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", user.id);
            return View(user);
        }

        // Presenta la vista que le pregunta al usuario si está seguro de que quiere borrar el usuario.
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Este método borra al usuario de la base de datos.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Project potentialProyect = db.Projects.Where(x => x.LeaderID == id).ToList().FirstOrDefault();
            if (potentialProyect == null)
            {
                User user = db.Users.Find(id);
                string realid = user.id;
                AspNetUser uSER = db.AspNetUsers.Find(realid);
                db.AspNetUsers.Remove(uSER);
                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", potentialProyect);
            }
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

