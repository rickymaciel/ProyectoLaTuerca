using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Repuesto
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [StringLength(60, MinimumLength = 3)]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Debe ingresar un proveedor")]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una marca")]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "Debe ingresar un modelo")]
        public int ModeloId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una categoria")]
        public int CategoriaId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }

        [ForeignKey("MarcaId")]
        public virtual Marca Marca { get; set; }

        [ForeignKey("ModeloId")]
        public virtual Modelo Modelo { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }


        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe ingresar el stock minimo")]
        public int StockMinimo { get; set; }


        [Required(ErrorMessage = "Debe ingresar el stock maximo")]
        public int StockMaximo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el precio de costo")]
        public float PrecioCosto { get; set; }
        public float PrecioVenta1 { get; set; }
        public float PrecioVenta2 { get; set; }
        public float PrecioVenta3 { get; set; }

    }
}