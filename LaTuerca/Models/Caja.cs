using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Caja
    {
        public Caja()
        {
            detallesCaja = new List<MovimientoCaja>();
        }

        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Fecha de Apertura es requerida")]
        [Display(Name = "Fecha de Apertura: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime")]
        public DateTime Fecha_Apertura { get; set; }

        [Display(Name = "Monto Inicial")]
        [Range(0, 100000000000000000, ErrorMessage = "El monto inicial debe ser un número entre 0 y 100.000.000.000.000.000")]
        [Required(ErrorMessage = "Debe ingresar el monto inicial")]
        public int Inicial { get; set; }

        [Display(Name = "Fecha de Cierre: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime")]
        public DateTime? Fecha_Cierre { get; set; }


        [Display(Name = "Total Entrada")]
        [Range(-100000000000000000, 100000000000000000, ErrorMessage = "El monto inicial debe ser un número entre -100.000.000.000.000.000 y 100.000.000.000.000.000")]
        public int? Entrada { get; set; }


        [Display(Name = "Total Salida")]
        public int? Salida { get; set; }


        [Display(Name = "Monto Cierre")]
        [Range(-100000000000000000, 100000000000000000, ErrorMessage = "El monto inicial debe ser un número entre -100.000.000.000.000.000 y 100.000.000.000.000.000")]
        public int? Cierre { get; set; }

        [Required(ErrorMessage = "La cantidad de operaciones es requerido")]
        [Display(Name = "Cant. Operaciones: ")]
        public int? Operaciones { get; set; }

        [Display(Name = "Usuario: ")]
        [Required(ErrorMessage = "Debe indicar un usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Estado: ")]
        public bool Estado { get; set; }

        public virtual ICollection<MovimientoCaja> detallesCaja { get; set; }
    }
}