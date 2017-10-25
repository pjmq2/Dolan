using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IngeDolan3._0.Startup))]
namespace IngeDolan3._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

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
