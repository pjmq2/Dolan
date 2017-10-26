using IngeDolan3._0.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(IngeDolan3._0.Startup))]
namespace IngeDolan3._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            dolan2Entities baseDatos = new dolan2Entities();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // creating Creating Profesor role    
            if (!roleManager.RoleExists("Profesor"))
            {
                var role = new IdentityRole();
                role.Name = "Profesor";
                roleManager.Create(role);
            }

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