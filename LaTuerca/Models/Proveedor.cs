using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Proveedor
    {
        public Proveedor()
        {
            Repuestos = new List<Repuesto>();
        }
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar la razon social")]
        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "Debe ingresar el RUC")]
        [StringLength(10, MinimumLength = 9)]
        [Display(Name = "R.U.C.")]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "Debe ingresar la dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar celular")]
        public string Celular { get; set; }

        public virtual ICollection<Repuesto> Repuestos { get; set; }
    }
}