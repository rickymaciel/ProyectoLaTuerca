﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class FacturaProveedor
    {
        public FacturaProveedor()
        {
            DetallesFacturaProveedores = new List<DetallesFacturaProveedor>();
        }

        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [Display(Name = "Fecha: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Vence: ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El Número de Factura es requerida")]
        [Display(Name = "Nº: ")]
        public int NumeroFactura { get; set; }

        [Required(ErrorMessage = "Debe ingresar un proveedor")]
        [Display(Name = "Proveedor: ")]
        public int ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El monto es requerido")]
        [Display(Name = "Total: ")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El monto es requerido")]
        [Display(Name = "Monto pagado: ")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPagado { get; set; }

        [Display(Name = "Metodo: ")]
        [Required(ErrorMessage = "Debe indicar un método de pago")]
        public string Metodo { get; set; }

        [Display(Name = "Estado: ")]
        public bool Pagado { get; set; }

        public virtual ICollection<DetallesFacturaProveedor> DetallesFacturaProveedores { get; set; }
    }
}