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
          public List<CompraDetalle> CompraDetalle { get; set; }

          public CompraViewModel(Compra _compra, List<CompraDetalle> _compraDetalles)
          {
            Compra = _compra;
            CompraDetalle = _compraDetalles;
          }
    }
}