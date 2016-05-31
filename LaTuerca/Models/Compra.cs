using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Compra
    {
        public Compra()
        {
            CompraDetalles = new List<CompraDetalle>();
            ICompraDetalles = new List<CompraDetalle>();
        }

        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Display(Name = "Fecha: ")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Nº: ")]
        public int NumeroFactura { get; set; }

        [Required(ErrorMessage = "Debe ingresar un proveedor")]
        [Display(Name = "Proveedor: ")]
        public int ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio no puede tener valor negativo")]
        [Display(Name = "Total: ")]
        public decimal Total { get; set; }

        [Display(Name = "Estado: ")]
        public bool Pagado { get; set; }

        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; }
        public IEnumerable<CompraDetalle> ICompraDetalles { get; set; }
    }
}