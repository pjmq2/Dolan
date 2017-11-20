using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;



namespace IngeDolan3._0.Models
{
    public class UserStoryInt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserStoryInt()
        {
            this.ProjectTasks = new HashSet<ProjectTask>();
            this.Scenarios = new HashSet<Scenario>();
        }

        public string ProjectID { get; set; }
        public int SprintID { get; set; }
        public string StoryID { get; set; }
        public string Modulo { get; set; }
        public string ID { get; set; }
        public Nullable<int> Priorities { get; set; }
        public string ClientRole { get; set; }
        public Nullable<int> Estimation { get; set; }
        public string Reason { get; set; }
        [DataType(DataType.MultilineText)]
        public string Funtionality { get; set; }
        public string Alias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; set; }
        public virtual Sprint Sprint { get; set; }


        public List<SelectListItem> ListaSprints { get; set; }

    }
}