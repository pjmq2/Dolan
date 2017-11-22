using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using System.Linq.Dynamic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IngeDolan3._0.Controllers
{
    public class UsersController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();
        ApplicationDbContext context = new ApplicationDbContext();

        

        private ApplicationUserManager _userManager;

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
                return RedirectToAction("Denied", "Others");
            }

        }

        // Reviza los permisos que tiene el usuario para determinar si debe o no denegar el acceso del usuario
        public Boolean CanDo(string permission)
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
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null){
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
        public ActionResult Edit(string id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Where(x => x.AspNetUser.Id == id).ToList().FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            UsuarioInt usuarioInt = new UsuarioInt();
            
            ViewBag.Id = id;
            
            return View(usuarioInt);
        }

        // Guarda los cambios solicitados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioInt usuarioInt){
            //ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", user.AspNetRole.Id);
            //ViewBag.role = new SelectList(db.AspNetRoles, "Name", "Name", user.AspNetRole.Name);

            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (ModelState.IsValid){

                var aspUser = db.AspNetUsers.Where(x => x.Id == usuarioInt.id).ToList().FirstOrDefault();
                var modelUser = db.Users.Where(x => x.AspNetUser.Id == aspUser.Id).ToList().First();

                if (aspUser != null && modelUser != null){
                   aspUser.Email = usuarioInt.email;
                   aspUser.AspNetRoles.Clear();
                   //get new role 
                    var role = db.AspNetRoles.Where(x => x.Name == usuarioInt.role).ToList().FirstOrDefault();
                   aspUser.AspNetRoles.Add(role);
                    if (!role.AspNetUsers.Contains(aspUser)){
                        role.AspNetUsers.Add(aspUser);
                    }
                    db.AspNetUsers.AddOrUpdate(aspUser);
                    db.AspNetRoles.AddOrUpdate(role);


                    modelUser.person_id = Int32.Parse(usuarioInt.personID);
                    modelUser.name = usuarioInt.name;
                    modelUser.firstLastName = usuarioInt.lastName1;
                    modelUser.secondLastName = usuarioInt.lastName2;
                    db.Users.AddOrUpdate(modelUser);
                    db.SaveChanges();
                }



                //var aspUser = db.AspNetUsers.Where(x => x.Id == usuarioInt.id);
                //var modelUser = db.Users.Where(x => x.AspNetUser == aspUser);



                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }            
            return RedirectToAction("Index");
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
            AspNetUser leader = (db.AspNetUsers.Where(x => x.Id == id).ToList().FirstOrDefault());
            User leaderUser = leader.Users.FirstOrDefault();
            Project potentialProyect = leaderUser.Projects.FirstOrDefault();
            if (potentialProyect == null)
            {
                leader.Users.Clear();
                db.AspNetUsers.Remove(leader);
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

