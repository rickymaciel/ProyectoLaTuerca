using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LATUERCA.Models
{
    public class Empresa
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de la empresa")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre corto de la empresa")]
        public string NombreCorto { get; set; }

        [Required(ErrorMessage = "Debe ingresar el Ruc de la empresa")]
        public int Ruc { get; set; }

        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Web { get; set; }
        public Boolean Estado { get; set; }
    }
}