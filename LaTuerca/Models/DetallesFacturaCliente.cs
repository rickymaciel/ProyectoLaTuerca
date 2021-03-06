﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class DetallesFacturaCliente
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        public int FacturaClienteId { get; set; }
        [ForeignKey("FacturaClienteId")]
        public virtual FacturaCliente FacturaCliente { get; set; }

        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "La cantidad debe ser un número entre 1 y 100")]
        [Required(ErrorMessage = "Debe ingresar una cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Repuesto")]
        [Required(ErrorMessage = "Debe seleccionar un repuesto")]
        public int RepuestoId { get; set; }
        [ForeignKey("RepuestoId")]
        public virtual Repuesto Repuesto { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Debe ingresar el precio")]
        public int Precio { get; set; }
    }
}