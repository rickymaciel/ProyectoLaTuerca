using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Categoria
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [StringLength(60, MinimumLength = 3)]
        public string Nombre { get; set; }

        public Boolean Estado { get; set; }
    }
}