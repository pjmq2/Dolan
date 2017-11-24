using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

        // Displays a list of all of the systems roles, along with an edit button to get into its respective edit page
        // GET: Permisoes
        public async Task<ActionResult> Index(){
            ViewBag.AsistenteID = db.AspNetRoles.Where(x => x.Name == "Asistente").ToList().FirstOrDefault().Id;

            ViewBag.EstudianteID = db.AspNetRoles.Where(x => x.Name == "Estudiante").ToList().FirstOrDefault().Id;

            return View(await db.AspNetRoles.ToArrayAsync());
        }

        // Unused
        // GET: Permisoes/Details/5
        public async Task<ActionResult> Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permiso permiso = await db.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        // Unused
        // GET: Permisoes/Create
        public ActionResult Create(){
            return View();
        }

        // Unused
        // POST: Permisoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "codigo,nombre")] Permiso permiso){
            if (ModelState.IsValid){
                db.Permisos.Add(permiso);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(permiso);
        }

        // Displays a screen that includes a "multiselect list" that allows the user to select the permissions the specified role must have.
        // GET: Permisoes/Edit/5
        public async Task<ActionResult> Edit(String roleId){
            /*
            < p >
                @Html.ActionLink("Editar Estudiante", "Edit", new { roleID = ViewBag.EstudianteID });
            @Html.ActionLink("Editar Asistente", "Edit", new { roleID = ViewBag.AsistenteID });
            </ p >
            */
            if (roleId == null){
                return RedirectToAction("Index", "Home");
            }


            //Get all permit objects
            ViewBag.AllPermits = db.Permisos.Where(x => true).ToList();
            ViewBag.Id = roleId;
             

            RoleInt roleInt = new RoleInt();
            //Get real role
            roleInt.role = db.AspNetRoles.Where(x => x.Id == roleId).ToList().FirstOrDefault();
            //Get all permits
            roleInt.AllPermits = db.Permisos.Where(x => true).ToList();
            //Get all of that roles assigned permissions
            roleInt.CodeList = roleInt.role.Permisos.Select(x => x.codigo).ToList();

            String title = "Cambiar permisos de " + roleInt.role.Name;
            ViewBag.TitleLabel = title;

            return View(roleInt);
        }

        // Displays a screen that includes a "multiselect list" that allows the user to select the permissions the specified role must have.
        // POST: Permisoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleInt input){

            
            AspNetRole role = db.AspNetRoles.Where(x => x.Id == input.Id).ToList().FirstOrDefault();
            //Remove all permisions from role
            role.Permisos.Clear();

            if (input.CodeList.Count != 0){
                //For each new permission
                foreach (var code in input.CodeList)
                {
                    //Get it
                    var permit = db.Permisos.Where(x => x.codigo == code).ToList().First();
                    //add that permit to that role
                    role.Permisos.Add(permit);
                    //If the permit didnt had that role associated then add it
                    if (!permit.AspNetRoles.Contains(role))
                    {
                        permit.AspNetRoles.Add(role);
                    }
                    db.Permisos.AddOrUpdate(permit);
                }
            }
            db.AspNetRoles.AddOrUpdate(role);
            db.SaveChanges();
            
            if (ModelState.IsValid){
                foreach (var item in input.CodeList){
                    Console.WriteLine(item);
                }
             
            
            }
    
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
