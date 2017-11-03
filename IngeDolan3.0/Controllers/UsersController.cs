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

        //Oh snap!
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
                return RedirectToAction("Index", "Home");
            }

        }

        public Boolean CanDo(string permission){
            String userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var modelUser = db.Users.Where(x => x.id == userId).ToList().First();
            var userRole = modelUser.AspNetRole;
            var permisos = userRole.Permisos;

            //if found return true
            foreach (var per in permisos){
                if (per.nombre == permission){
                    return true;
                }
            }
            //if it hasnt returned by now then it must be the user does not have permission
            return false;
        }

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

        //Oh jeez

        // GET: Users/Details/5
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

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

            ViewBag.id = new SelectList(db.AspNetUsers, "Id", "Email", user.id);
            return View(user);
        }

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

