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

        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "El nombre es un campo requerido.")]
        [Display(Name = "Nombre")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        public string name { get; set; }


        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        [Display(Name = "Primer apellido")]
        public string lastName1 { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "El nombre solo debe contener letras y espacios")]
        [Display(Name = "Segundo Apellido")]
        public string lastName2 { get; set; }

        [Required(ErrorMessage = "El número de cédula es un campo requerido.")]
        [Display(Name = "Cédula")]
        public string personID { get; set; }

        [Display(Name = "Carnet")]
        public string studentID { get; set; }

        [Display(Name = "Sexo")]
        public string sex { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime birthDate { get; set; }

        [Display(Name = "Rol")]
        public String role { get; set; }

    }
}