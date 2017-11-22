using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IngeDolan3._0.Models
{
    public class RoleInt
    {

        public MultiSelectList group { get; set; }

        public AspNetRole role { get; set; }
        public List<Permiso> AllPermits { get; set; }
        public ICollection<Permiso> AssignedPermits { get; set; }
        public ICollection<Permiso> SelectedPermits { get; set; }
        public List<int> CodeList { get; set; }
        public string Id { get; set; }
    }
}