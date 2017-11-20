using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IngeDolan3._0.Models
{
    public class GenericList
    {
        public string id { get; set; }
        public List<SelectListItem> Nombres { get; set; }
    }
}