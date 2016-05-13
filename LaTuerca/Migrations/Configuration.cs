namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LaTuerca.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LaTuerca.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            //Insertar Menu
            context.Menus.AddOrUpdate(
                p => p.Name,
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Admin",
                  Description = "Modulo Administrador",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Compras",
                  Description = "Modulo Compras",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Informes",
                  Description = "Modulo Informes",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Ventas",
                  Description = "Modulo Ventas",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 1,
                  Name = "Menús",
                  Description = "Menús",
                  Action = "Index",
                  Controller = "Menus"
              },
              new Models.Menu
              {
                  ParentId = 1,
                  Name = "Empresas",
                  Description = "Empresas",
                  Action = "Index",
                  Controller = "Empresas"
              },
              new Models.Menu
              {
                  ParentId = 2,
                  Name = "Proveedores",
                  Description = "Proveedores",
                  Action = "Index",
                  Controller = "Proveedores"
              },
              new Models.Menu
              {
                  ParentId = 2,
                  Name = "FacturaCompras",
                  Description = "Facturacion Proveedor",
                  Action = "Index",
                  Controller = "FacturaCompras"
              },
              new Models.Menu
              {
                  ParentId = 4,
                  Name = "Mantenimiento",
                  Description = "Mantenimiento de Marcas | Categorias | Modelos",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 9,
                  Name = "Marcas",
                  Description = "Mantenimiento de Marcas",
                  Action = "Index",
                  Controller = "Marcas"
              },
              new Models.Menu
              {
                  ParentId = 9,
                  Name = "Modelos",
                  Description = "Mantenimiento de Modelos",
                  Action = "Index",
                  Controller = "Modelos"
              },
              new Models.Menu
              {
                  ParentId = 9,
                  Name = "Categorias",
                  Description = "Mantenimiento de Categorias",
                  Action = "Index",
                  Controller = "Categorias"
              },
              new Models.Menu
              {
                  ParentId = 4,
                  Name = "Repuestos",
                  Description = "Repuestos",
                  Action = "Index",
                  Controller = "Repuestos"
              },
              new Models.Menu
              {
                  ParentId = 4,
                  Name = "FacturaVentas",
                  Description = "Facturacion Cliente",
                  Action = "Index",
                  Controller = "FacturaVentas"
              },
              new Models.Menu
              {
                  ParentId = 4,
                  Name = "Clientes",
                  Description = "Clientes",
                  Action = "Index",
                  Controller = "Clientes"
              }
            );
        }
    }
}
