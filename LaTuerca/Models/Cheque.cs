using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Cheque
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }


        public int BancoId { get; set; }
        [ForeignKey("BancoId")]
        public virtual Banco Banco { get; set; }

        public int ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "Debe ingresar el Numero de Cheque")]
        public int NroCheque { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto")]
        public int Monto { get; set; }

        [Required(ErrorMessage = "El lugar es requerido")]
        [Display(Name = "Lugar ")]
        [StringLength(60, MinimumLength = 3)]
        public string Lugar { get; set; }


        [Required(ErrorMessage = "La fecha es requerido")]
        [Display(Name = "Fecha ")]
        [StringLength(60, MinimumLength = 3)]
        public string Fecha { get; set; }

        [Required(ErrorMessage = "El Numero de Cuenta es requerido")]
        [Display(Name = "NroCuenta ")]
        [StringLength(60, MinimumLength = 3)]
        public string nroCuenta { get; set; }

        [Required(ErrorMessage = "La serie es requerido")]
        [Display(Name = "Serie ")]
        [StringLength(60, MinimumLength = 3)]
        public string Serie { get; set; }

        public bool Estado { get; set; }

    }
}