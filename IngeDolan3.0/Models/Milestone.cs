//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IngeDolan3._0.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Milestone
    {
        public string ProjectID { get; set; }
        public int SprintID { get; set; }
        public string StoryID { get; set; }
        public string TaskID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Date { get; set; }
        public string MilestoneID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Progreso { get; set; }
    
        public virtual ProjectTask ProjectTask { get; set; }
    }
}
