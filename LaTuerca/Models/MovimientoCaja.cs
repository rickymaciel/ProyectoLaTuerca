using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class MovimientoCaja
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe especificar una caja")]
        [Display(Name = "Caja: ")]
        public int CajaId { get; set; }
        [ForeignKey("CajaId")]
        public virtual Caja Caja { get; set; }

        
        [Required(ErrorMessage = "El concepto es requerido")]
        [Display(Name = "Concepto: ")]
        public string Concepto { get; set; }

        [Required(ErrorMessage = "El tipo de operacion es requerido")]
        [Display(Name = "Movimiento: ")]
        public string Movimiento { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Display(Name = "Monto: ")]
        public int Monto { get; set; }

    }
}