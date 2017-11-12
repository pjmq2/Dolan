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

        public AspNetRole role;
        public List<Permiso> AllPermits;
        public ICollection<Permiso> AssignedPermits;
        public ICollection<Permiso> SelectedPermits;
    }
}