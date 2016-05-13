using LaTuerca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaTuerca.ViewModels
{
    public class RepuestoViewModel
    {
          public Repuesto Repuesto { get; set; }
          public List<Marca> Marcas { get; set; }
          public List<Modelo> Modelos { get; set; }
          public List<Categoria> Categorias { get; set; }
          public List<Proveedor> Proveedores { get; set; }
          public Proveedor Proveedor {get; set;}
          public Marca Marca { get; set; }
          public Modelo Modelo { get; set; }
          public Categoria Categoria { get; set; }

          public RepuestoViewModel(Repuesto _repuesto, List<Marca> _marcas, List<Modelo> _modelos, List<Categoria> _categorias, List<Proveedor> _proveedores)
          {
            Repuesto = _repuesto;
            Marcas = _marcas;
            Modelos = _modelos;
            Categorias = _categorias;
            Proveedores = _proveedores;
            Marca = new Marca();
            Modelo = new Modelo();
            Categoria = new Categoria();
            Proveedor = new Proveedor();
          }
    }
}