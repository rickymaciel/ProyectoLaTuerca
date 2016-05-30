using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Modelo
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Modelo: ")]
        [StringLength(60, MinimumLength = 3)]
        public string NombreModelo { get; set; }


        [Required(ErrorMessage = "La marca es requerida")]
        [Display(Name = "Marca: ")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public virtual Marca Marca { get; set; }

        [Display(Name = "Estado: ")]
        public Boolean Estado { get; set; }
    }
}