using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IngeDolan3._0.Models
{
    using System;
    using System.Collections.Generic;
    public class HistoriasUsuario
    {

        [Display(Name = "ID Historia de Usuario")]
        public string StoryID { get; set; }
        
        [Display(Name = "ID del proyecto")]
        public string ProjectID { get; set; }

        [Display(Name = "Sprint")]
        public string SprintID { get; set; }

        [Display(Name = "Prioridad")]
        public int Priorities { get; set; }

        [Display(Name = "Rol")]
        public string ClientRole { get; set; }

        [Display(Name = "Estimacion")]
        public int Estimation { get; set; }

        [Display(Name = "Razon")]
        public string Reason { get; set; }

        [Display(Name = "Funcionalidad")]
        public string Funtionality { get; set; }

        [Display(Name = "Alias de la Historia de Usuario")]
        public string Alias { get; set; }
        
        public virtual Project Project { get; set; }

        public List<string> Rol { get; set; }
    }
}