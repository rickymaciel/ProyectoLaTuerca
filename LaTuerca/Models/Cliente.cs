using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Cliente
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar la razon social")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "Debe ingresar el documento")]
        public string Documento { get; set; }


        [Required(ErrorMessage = "Debe ingresar la direccion")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe ingresar teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe ingresar celular")]
        public string Celular { get; set; }



    }
}