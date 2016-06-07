using LaTuerca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaTuerca.ViewModels
{
    public class CompraViewModel
    {
        public Compra Compra { get; set; }
        public List<CompraDetalle> CompraDetalles { get; set; }
        public List<Proveedor> Proveedores { get; set; }
        public List<Repuesto> Repuestos { get; set; }

        public CompraViewModel(Compra _compra, List<CompraDetalle> _compraDetalles, List<Proveedor> _proveedores, List<Repuesto> _repuestos)
        {
            Compra = _compra;
            CompraDetalles = _compraDetalles;
            Proveedores = _proveedores;
            Repuestos = _repuestos;
        }
    }
}