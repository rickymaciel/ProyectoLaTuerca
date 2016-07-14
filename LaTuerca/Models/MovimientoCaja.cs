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

        [Required(ErrorMessage = "La Fecha es requerida")]
        [Display(Name = "Fecha: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime")]
        public DateTime Fecha { get; set; }


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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "El ingreso es requerido")]
        [Display(Name = "Ingreso: ")]
        public int Ingreso { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "El egreso es requerido")]
        [Display(Name = "Egreso: ")]
        public int Egreso { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "El saldo es requerido")]
        [Display(Name = "Saldo: ")]
        public int Saldo { get; set; }

    }
}