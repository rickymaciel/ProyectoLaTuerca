using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaTuerca.Models
{
    public class Menu
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        public virtual int ParentId { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del menú")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripción del menú")]
        public string Description { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public bool Active { get; set; }

        public List<Menu> Children()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Menus.Where(b => b.ParentId == this.Id).ToList();
        }
    }
}