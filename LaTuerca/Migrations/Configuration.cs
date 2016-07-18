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
            //Insertar Menu
            context.Menus.AddOrUpdate(
                p => p.Name,
              new Models.Menu
              {
                  //1
                  ParentId = 0,
                  Name = "Administrador",
                  Description = "Modulo Administrador",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //2
                  ParentId = 0,
                  Name = "Compras",
                  Description = "Modulo Compras",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //3
                  ParentId = 0,
                  Name = "Informes",
                  Description = "Modulo Informes",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //4
                  ParentId = 0,
                  Name = "Ventas",
                  Description = "Modulo Ventas",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //5
                  ParentId = 0,
                  Name = "Stock",
                  Description = "Stock",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //6
                  ParentId = 0,
                  Name = "Caja",
                  Description = "Caja",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //7
                  ParentId = 0,
                  Name = "Bancos",
                  Description = "Bancos",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //8
                  ParentId = 3,
                  Name = "Ventas",
                  Description = "Resumen de Ventas",
                  Action = "InformeVentas",
                  Controller = "FacturaClientes",
                  Active = true
              },
              new Models.Menu
              {
                  //9
                  ParentId = 1,
                  Name = "Menús",
                  Description = "Menús",
                  Action = "Index",
                  Controller = "Menus",
                  Active = true
              },
              new Models.Menu
              {
                  //10
                  ParentId = 1,
                  Name = "Empresas",
                  Description = "Empresas",
                  Action = "Index",
                  Controller = "Empresas",
                  Active = true
              },
              new Models.Menu
              {
                  //11
                  ParentId = 2,
                  Name = "Proveedores",
                  Description = "Proveedores",
                  Action = "Index",
                  Controller = "Proveedores",
                  Active = true
              },
              new Models.Menu
              {
                  //12
                  ParentId = 2,
                  Name = "Facturas",
                  Description = "Facturas Proveedor",
                  Action = "Facturados",
                  Controller = "FacturaProveedors",
                  Active = true
              },
              new Models.Menu
              {
                  //13
                  ParentId = 4,
                  Name = "Mantenimiento",
                  Description = "Mantenimiento de Marcas | Categorias | Modelos",
                  Action = "",
                  Controller = "",
                  Active = true
              },
              new Models.Menu
              {
                  //14
                  ParentId = 13,
                  Name = "Marcas",
                  Description = "Mantenimiento de Marcas",
                  Action = "Index",
                  Controller = "Marcas",
                  Active = true
              },
              new Models.Menu
              {
                  //15
                  ParentId = 13,
                  Name = "Modelos",
                  Description = "Mantenimiento de Modelos",
                  Action = "Index",
                  Controller = "Modelos",
                  Active = true
              },
              new Models.Menu
              {
                  //16
                  ParentId = 13,
                  Name = "Categorias",
                  Description = "Mantenimiento de Categorias",
                  Action = "Index",
                  Controller = "Categorias",
                  Active = true
              },
              new Models.Menu
              {
                  //17
                  ParentId = 4,
                  Name = "Repuestos",
                  Description = "Repuestos",
                  Action = "Index",
                  Controller = "Repuestos",
                  Active = true
              },
              new Models.Menu
              {
                  //18
                  ParentId = 4,
                  Name = "Presupuesto Venta",
                  Description = "Presupuestos Cliente",
                  Action = "Presupuestos",
                  Controller = "FacturaClientes",
                  Active = true
              },
              new Models.Menu
              {
                  //19
                  ParentId = 4,
                  Name = "Factura Venta",
                  Description = "Facturas Cliente",
                  Action = "Facturados",
                  Controller = "FacturaClientes",
                  Active = true
              },
              new Models.Menu
              {
                  //20
                  ParentId = 4,
                  Name = "Clientes",
                  Description = "Clientes",
                  Action = "Index",
                  Controller = "Clientes",
                  Active = true
              },
              new Models.Menu
              {
                  //21
                  ParentId = 7,
                  Name = "Bancos",
                  Description = "Bancos",
                  Action = "Index",
                  Controller = "Bancos",
                  Active = true
              },
              new Models.Menu
              {
                  //22
                  ParentId = 7,
                  Name = "Cheques",
                  Description = "Cheques",
                  Action = "Index",
                  Controller = "Cheques",
                  Active = true
              },
              new Models.Menu
              {
                  //23
                  ParentId = 6,
                  Name = "Caja",
                  Description = "Modulo Caja",
                  Action = "Index",
                  Controller = "Cajas",
                  Active = true
              },
              new Models.Menu
              {
                  //24
                  ParentId = 3,
                  Name = "Movimientos",
                  Description = "Informe de Movimientos de Caja",
                  Action = "InformeMovimientos",
                  Controller = "Cajas",
                  Active = true
              }
            );

            context.Marcas.AddOrUpdate(
                p => p.Nombre,
                new Models.Marca
                {
                    Nombre = "GENÉRICO",
                    Imagen = "Default.jpg"
                },
                new Models.Marca
                {
                    Nombre = "TOYOTA",
                    Imagen = "Default.jpg"
                },
                new Models.Marca
                {
                    Nombre = "MERCEDEZ BENZ",
                    Imagen = "Default.jpg"
                },
                new Models.Marca
                {
                    Nombre = "HONDA",
                    Imagen = "Default.jpg"
                },
                new Models.Marca
                {
                    Nombre = "CHEVROLET",
                    Imagen = "Default.jpg"
                },
                new Models.Marca
                {
                    Nombre = "SUZUKI",
                    Imagen = "Default.jpg"
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
                    Telefono = "(021) 582-750",
                    Celular = "(021) 582-750 "
                },
                new Models.Proveedor
                {
                    RazonSocial = "CONDOR SACI",
                    Ruc = "2938475-9",
                    Direccion = "Bernardino Gorostiaga y Guaraníes",
                    Telefono = "(021) 582-750",
                    Celular = "(0981) 382-750"
                },
                new Models.Proveedor
                {
                    RazonSocial = "HONDA MOTOR CO. Ltd",
                    Ruc = "2102832-9",
                    Direccion = "Avda. Eusebio Ayala",
                    Telefono = "(021) 782-750",
                    Celular = "(0973) 882-980"
                },
                new Models.Proveedor
                {
                    RazonSocial = "De La Sobera S.A.",
                    Ruc = "3745673-9",
                    Direccion = "Ruta 6 Km. 48",
                    Telefono = "(021) 582-750",
                    Celular = "(0975) 582-750"
                }
            );



            context.Clientes.AddOrUpdate(
                p => p.RazonSocial,
                new Models.Cliente
                {
                    RazonSocial = "CHACOMER AUTOMOTORES",
                    Documento = "3452943",
                    Direccion = "Avda. Eusebio Ayala Nº 3321 c/ Rca. Argentina",
                    Telefono = "(021) 518-000",
                    Celular = "(0981) 621-230"
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
                },
                new Models.Categoria
                {
                    Nombre = "TRANSMISIONES",
                    Estado = true
                }
            );

            context.Repuestoes.AddOrUpdate(
                p => p.Nombre,
                new Models.Repuesto
                {
                    Nombre = "AMORTIGUADOR",
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
                    Imagen = "Default.jpg"
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
                    Imagen = "Default.jpg"
                },
                new Models.Repuesto
                {
                    Nombre = "EJE DE TRANSMISION",
                    ProveedorId = 3,
                    ModeloId = 2,
                    CategoriaId = 3,
                    Stock = 0,
                    StockMinimo = 10,
                    StockMaximo = 100,
                    PrecioCosto = 450000,
                    PrecioVenta1 = 540000,
                    PrecioVenta2 = 607500,
                    PrecioVenta3 = 630000,
                    Imagen = "Default.jpg"
                }
            );
            context.Users.AddOrUpdate(
                p => p.Email,
                new Models.ApplicationUser
                {
                    Id = "3dfd1c4e-2ba1-47c8-83a8-1ee3019c6b30",
                    Email = "rmacielb3@gmail.com",
                    EmailConfirmed = false,
                    PasswordHash = "AGgI66nvc6RQweHG2a6HE8c0qCamjXOZgcGjV+x5ClqhxH4F5W6jz7Djs5VLJIbmDQ==",
                    SecurityStamp = "5bba54eb-d535-4e3e-b77a-7fca50bf7ed8",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "rmacielb3@gmail.com"
                }
            );
        }
    }
}