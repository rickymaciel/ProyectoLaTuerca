using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Modelo
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [StringLength(60, MinimumLength = 3)]
        public string NombreModelo { get; set; }

        [Required(ErrorMessage = "Debe ingresar un fabricante")]
        public string Fabricante { get; set; }

        [Required(ErrorMessage = "Debe ingresar un año de fabricación")]
        public int Anho { get; set; }

        [Required(ErrorMessage = "Debe ingresar el numero de cilindrada")]
        public string Cilindros { get; set; }

        [Required(ErrorMessage = "Debe ingresar la potencia del motor")]
        public String Potencia { get; set; }

        [Required(ErrorMessage = "Debe ingresar un tipo de cambio")]
        public String Tipo_Cambio { get; set; }

        public Boolean Estado { get; set; }
    }
}