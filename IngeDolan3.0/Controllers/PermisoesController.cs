using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;

namespace IngeDolan3._0.Controllers
{
    public class PermisoesController : Controller
    {
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Permisoes
        public async Task<ActionResult> Index()
        {
            ViewBag.AsistenteID = db.AspNetRoles.Where(x => x.Name == "Asistente").ToList().FirstOrDefault().Id;

            ViewBag.EstudianteID = db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault().Id;

            return View(await db.Permisos.ToListAsync());
        }

        // GET: Permisoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permisos permiso = await db.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        // GET: Permisoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Permisoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "codigo,nombre")] Permisos permiso)
        {
            if (ModelState.IsValid)
            {
                db.Permisos.Add(permiso);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(permiso);
        }

        // GET: Permisoes/Edit/5
        public async Task<ActionResult> Edit(String roleId){
            String test = roleId;
            Console.WriteLine(roleId);
            if (roleId == null){
                return RedirectToAction("Index", "Home");
            }


            //Get all permit objects
            ViewBag.AllPermits = db.Permisos.Where(x => true).ToList();

             

            RoleInt roleInt = new RoleInt();
            //Get real role
            roleInt.role = db.AspNetRoles.Where(x => x.Id == roleId).ToList().FirstOrDefault();
            //Get all permits
            roleInt.AllPermits = db.Permisos.Where(x => true).ToList();
            //Get all of that roles assigned permissions
            roleInt.AssignedPermits = roleInt.role.Permisos;
            ViewBag.AssignedPermits = roleInt.role.Permisos;


            return View(roleInt);
        }

        // POST: Permisoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "codigo,nombre")] Permiso permiso)
       

        public async Task<ActionResult> Edit(RoleInt input)
        {

            //var algo = input.testSring;
            //Console.WriteLine(algo);
            
            AspNetRoles role = input.role;
            var allPermits = input.AllPermits;
            var assignedPermits = input.AssignedPermits;
            var selectedPermits = input.SelectedPermits;
            var codeList = input.CodeList;

            foreach (var item in codeList)
            {
                Console.WriteLine(item);
            }


            Console.WriteLine(role.Name);
            foreach (var item in allPermits){
                Console.WriteLine(item.nombre);
            }

            foreach (var item in assignedPermits)
            {
                Console.WriteLine(item.nombre);
            }

            foreach (var item in selectedPermits)
            {
                Console.WriteLine(item.nombre);
            }


            if (ModelState.IsValid){

                if (input.role != null && input.AllPermits != null && input.AssignedPermits != null &&
                    input.SelectedPermits != null){
                }
               // db.Entry(permiso).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
    
            return View(input);
        }

        // GET: Permisoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permisos permiso = await db.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        // POST: Permisoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Permisos permiso = await db.Permisos.FindAsync(id);
            db.Permisos.Remove(permiso);
            await db.SaveChangesAsync();
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
