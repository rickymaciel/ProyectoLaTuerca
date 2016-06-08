using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Banco
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [StringLength(60, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar una direccion")]
        [StringLength(60, MinimumLength = 3)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar un telefono")]
        [StringLength(60, MinimumLength = 3)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar un Ruc")]
        [Display(Name = "R.U.C.")]
        [StringLength(60, MinimumLength = 3)]
        public string Ruc { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cuenta corriente")]
        [Display(Name = "Cuenta Corriente")]
        [StringLength(60, MinimumLength = 3)]
        public string CuentaCorriente { get; set; }

        [Required(ErrorMessage = "Debe ingresar un email")]
        [StringLength(60, MinimumLength = 3)]
        public string Email { get; set; }

        
        public bool Estado { get; set; }

    }
}