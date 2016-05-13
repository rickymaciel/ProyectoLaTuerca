using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Marca
    {
        public Marca()
        {
            Repuestos = new List<Repuesto>();
        }
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [StringLength(60, MinimumLength = 3)]
        public string Nombre { get; set; }

        public virtual ICollection<Repuesto> Repuestos { get; set; }

    }
}