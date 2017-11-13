using IngeDolan3._0.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Data.SqlClient;

[assembly: OwinStartupAttribute(typeof(IngeDolan3._0.Startup))]
namespace IngeDolan3._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers(app);
        }
        private void createRolesandUsers(IAppBuilder app)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            NewDolan2Entities db = new NewDolan2Entities();


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // creating Creating Profesor role
            try
            {
                if (!roleManager.RoleExists("Profesor"))
                {
                    var role = new IdentityRole();
                    role.Name = "Profesor";
                    roleManager.Create(role);
                }
            }
            catch (SqlException e)
            {
                switch (e.Number)
                {
                    case 40:
                        // ¿Como manejar error al conectar con SQL?
                        break;
                    default:
                        throw;
                }
            }

            //Add all 
            AspNetRole roleAux = db.AspNetRoles.Where(x => x.Name == "Profesor").ToList().First();
            //Get all permits given its a professor
            var permisosList = db.Permisos.Where(x => true).ToList();
            foreach (var per in permisosList){
                per.AspNetRoles.Add(roleAux);
                roleAux.Permisos.Add(per);
            }
            db.SaveChanges();
                

            // creating Creating Asistente role    
            if (!roleManager.RoleExists("Asistente"))
            {
                var role = new IdentityRole();
                role.Name = "Asistente";
                roleManager.Create(role);
            }

            // creating Creating Asistente role    
            if (!roleManager.RoleExists("Estudiante"))
            {
                var role = new IdentityRole();
                role.Name = "Estudiante";
                roleManager.Create(role);
            }
        }
    }
}