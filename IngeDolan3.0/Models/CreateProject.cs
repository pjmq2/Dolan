﻿//------------------------------------------------------------------------------
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
    using System.Web.Mvc;

    public partial class CreateProject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CreateProject(){
            this.UserStories = new HashSet<UserStory>();
        }

        public MultiSelectList group { get; set; }

        [Display(Name = "Identificación")]
        public string ProjectID { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public Nullable<System.DateTime> StartingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de finalización")]
        public Nullable<System.DateTime> FinalDate { get; set; }
        [Display(Name = "Descripción")]
        public string Descriptions { get; set; }
        [Required(ErrorMessage = "El nombre es un campo requerido.")]
        [Display(Name = "Nombre")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Todo proyecto debe tener un lider.")]
        [Display(Name = "Líder")]
        public string LeaderID { get; set; }
        [Display(Name = "Estado")]
        public string Pstate { get; set; }

        public User users { get; set; }
        public List<string> IncludedUsers { get; set; }

        public List<string> PrevEditList { get; set; }

        public string UserID { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserStory> UserStories { get; set; }
    }
}