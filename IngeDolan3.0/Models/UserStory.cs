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
    
    public partial class UserStory
    {
        public string StoryID { get; set; }
        public string ProjectID { get; set; }
        public string SprintID { get; set; }
        public string Modulo { get; set; }
        public string ID { get; set; }
        public Nullable<int> Priorities { get; set; }
        public string ClientRole { get; set; }
        public Nullable<int> Estimation { get; set; }
        public string Reason { get; set; }
        public string Funtionality { get; set; }
        public string Alias { get; set; }
    
        public virtual Project Project { get; set; }
    }
}
