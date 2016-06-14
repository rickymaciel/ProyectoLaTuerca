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
                  Name = "Factura Compras",
                  Description = "Facturacion Proveedor",
                  Action = "Factura",
                  Controller = "FacturaProveedors"
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
                  Name = "Factura Ventas",
                  Description = "Facturacion Cliente",
                  Action = "Factura",
                  Controller = "FacturaClientes"
              },
              new Models.Menu
              {
                  ParentId = 4,
                  Name = "Clientes",
                  Description = "Clientes",
                  Action = "Index",
                  Controller = "Clientes"
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Stock",
                  Description = "Stock",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Caja",
                  Description = "Caja",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Bancos",
                  Description = "Bancos",
                  Action = "",
                  Controller = ""
              },
              new Models.Menu
              {
                  ParentId = 18,
                  Name = "Bancos",
                  Description = "Bancos",
                  Action = "Index",
                  Controller = "Bancos"
              },
              new Models.Menu
              {
                  ParentId = 18,
                  Name = "Cheques",
                  Description = "Cheques",
                  Action = "Index",
                  Controller = "Cheques"
              },
              new Models.Menu
              {
                  ParentId = 0,
                  Name = "Configuración",
                  Description = "Configuraciones del sistema",
                  Action = "",
                  Controller = ""
              }
            );

            context.Marcas.AddOrUpdate(
                p => p.Nombre,
                new Models.Marca
                {
                    Nombre = "GENÉRICO"
                },
                new Models.Marca
                {
                    Nombre = "TOYOTA"
                },
                new Models.Marca
                {
                    Nombre = "MERCEDEZ BENZ"
                },
                new Models.Marca
                {
                    Nombre = "HONDA"
                },
                new Models.Marca
                {
                    Nombre = "CHEVROLET"
                },
                new Models.Marca
                {
                    Nombre = "SUZUKI"
                }
            );

            context.Modeloes.AddOrUpdate(
                p => p.NombreModelo,
                new Models.Modelo
                {
                    NombreModelo = "GENÉRICO",
                    MarcaId = 1,
                    Estado = true
                },
                new Models.Modelo
                {
                    NombreModelo = "CAMRY",
                    MarcaId = 2,
                    Estado = true
                },
                new Models.Modelo
                {
                    NombreModelo = "AMG G63",
                    MarcaId = 3,
                    Estado = true
                },
                new Models.Modelo
                {
                    NombreModelo = "CLK 350",
                    MarcaId = 3,
                    Estado = true
                }
            );

            context.Proveedors.AddOrUpdate(
                p => p.RazonSocial,
                new Models.Proveedor
                {
                    RazonSocial = "TOYOTOSHI",
                    Ruc = "9283847-7",
                    Direccion = "Avda. Mariscal López 2801/99 y Reclus",
                    Telefono = "(021) 582 750 ",
                    Celular = "(021) 582 750 "
                },
                new Models.Proveedor
                {
                    RazonSocial = "CONDOR SACI",
                    Ruc = "2938475-9",
                    Direccion = "Bernardino Gorostiaga y Guaraníes",
                    Telefono = "595-61-575057",
                    Celular = "595-61-575057"
                }
            );


            context.Categorias.AddOrUpdate(
                p => p.Nombre,
                new Models.Categoria
                {
                    Nombre = "AMORTIGUADORES",
                    Estado = true
                },
                new Models.Categoria
                {
                    Nombre = "FILTROS",
                    Estado = true
                },
                new Models.Categoria
                {
                    Nombre = "RADIADORES",
                    Estado = true
                }
            );

            context.Repuestoes.AddOrUpdate(
                p => p.Nombre,
                new Models.Repuesto
                {
                    Nombre = "AMORTIGUADOR DELANTERO",
                    ProveedorId = 1,
                    ModeloId = 1,
                    CategoriaId = 1,
                    Stock = 0,
                    StockMinimo = 10,
                    StockMaximo = 100,
                    PrecioCosto = 760000,
                    PrecioVenta1 = 912000,
                    PrecioVenta2 = 1026000,
                    PrecioVenta3 = 1064000,
                },
                new Models.Repuesto
                {
                    Nombre = "BUJIA DE ENCENDIDO",
                    ProveedorId = 2,
                    ModeloId = 3,
                    CategoriaId = 2,
                    Stock = 0,
                    StockMinimo = 10,
                    StockMaximo = 100,
                    PrecioCosto = 370000,
                    PrecioVenta1 = 444000,
                    PrecioVenta2 = 499500,
                    PrecioVenta3 = 518000,
                }
            );
        }
    }
}