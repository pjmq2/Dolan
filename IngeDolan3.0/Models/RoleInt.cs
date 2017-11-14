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

        public AspNetRoles role;
        public List<Permisos> AllPermits;
        public ICollection<Permisos> AssignedPermits;
        public ICollection<Permisos> SelectedPermits;
        public List<int> CodeList;
        public string testSring;
    }
}