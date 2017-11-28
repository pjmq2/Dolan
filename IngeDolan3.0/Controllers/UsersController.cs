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
using IngeDolan3._0.Generator;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IngeDolan3._0.Controllers
{
    public class UsersController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();
       

        private ApplicationUserManager _userManager;

        // Displays a list of all of the systems registered users
        public ActionResult Index(int page = 1, string sort = "name", string sortdir = "asc", string search = "")
        {
            if (CanDo("Consultar Lista de Usuarios")){
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

        // Checks if the currently logged in user has permissions to perform a particular task
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

        // Gets a list of all of the users for them to be displayed in the index function
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

        // Displays all of the details for an specific user
        public ActionResult Details(string id)
        {
            if (IDGenerator.CanDo("Consultar Detalles de Usuarios"))
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
            else
            {
                return RedirectToAction("Denied", "Others");
            }
        }

        // Displays a screen that allows the user to create a new user
        public ActionResult Create(){
            return View();
        }

        // Stores the new user information on the database
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

        // Displays a screen that allows a user to modify another user
        public ActionResult Edit(string id){
            if (IDGenerator.CanDo("Modificar Usuarios"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Where(x => x.AspNetUser.Id == id).ToList().FirstOrDefault();
                if (user == null)
                {
                    return HttpNotFound();
                }
                UsuarioInt usuarioInt = new UsuarioInt();
                usuarioInt.email = user.AspNetUser.Email;
                usuarioInt.role = user.AspNetRole.Name;
                usuarioInt.name = user.name;
                usuarioInt.lastName1 = user.firstLastName;
                usuarioInt.lastName2 = user.secondLastName;
                usuarioInt.student_id = user.student_id;
                usuarioInt.person_id = user.person_id;
                usuarioInt.id = user.AspNetUser.Id;
                usuarioInt.userID = user.userID;

                ViewBag.Id = id;

                return View(usuarioInt);
            }
            else
            {
                return RedirectToAction("Denied", "Others");
            }
        }

        // Stores the users edited data on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioInt usuarioInt){
            

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


                    modelUser.person_id = usuarioInt.person_id;
                    modelUser.name = usuarioInt.name;
                    modelUser.firstLastName = usuarioInt.lastName1;
                    modelUser.secondLastName = usuarioInt.lastName2;
                    db.Users.AddOrUpdate(modelUser);
                    db.SaveChanges();
                }
            }            
            return RedirectToAction("Index");
        }
        
        // Displays a confirmation screen for the user deletion
        public ActionResult Delete(string id)
        {
            if (IDGenerator.CanDo("Eliminar Usuarios"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //User user = db.Users.Find(id);
                User user = db.Users.Where(x => x.AspNetUser.Id == id).ToList().FirstOrDefault();

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Denied", "Others");
            }
        }

        // Deletes that specific user from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(User uzer)
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string id = uzer.AspNetUser.Id;
            if (userId != id)
            {
                AspNetUser leader = (db.AspNetUsers.Where(x => x.Id == id).ToList().FirstOrDefault());
                User leaderUser = leader.Users.First();
                Project potentialProyect = leaderUser.Projects.ToList().FirstOrDefault();
                if (potentialProyect == null)
                {
                    leader.Users.Clear();
                    db.Users.Remove(leaderUser);
                    db.AspNetUsers.Remove(leader);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", potentialProyect);
                }
            }
            else
            {
                return View("Suicide");
            }
        }

        // Unused
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

