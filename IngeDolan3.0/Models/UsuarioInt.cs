using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace IngeDolan3._0.Models
{

    using System;
    public class UsuarioInt
    {

        [Required(ErrorMessage = "El email es un campo requerido.")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, ErrorMessage = "{0} Debe ser de al menos {2} caracteres, ademas debe tener al menos un numero.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "La contraseña no coinside.")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "El nombre es un campo requerido.")]
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        public string name { get; set; }


        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El primer apellido solo debe contener letras y espacios")]
        [Display(Name = "Primer apellido")]
        public string lastName1 { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El segundo apellido solo debe contener letras y espacios")]
        [Display(Name = "Segundo Apellido")]
        public string lastName2 { get; set; }
        
        [Display(Name = "Cédula")]
        public Nullable<int> person_id { get; set; }

        [Display(Name = "Carnet")]
        public string student_id { get; set; }

        [Display(Name = "Rol")]
        public string role { get; set; }

        public string id { get; set; }

    }
}