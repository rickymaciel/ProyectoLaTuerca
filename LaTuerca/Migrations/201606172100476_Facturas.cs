namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DetallesFacturaClientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacturaClienteId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Precio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FacturaClientes", t => t.FacturaClienteId, cascadeDelete: false)
                .ForeignKey("dbo.Repuestoes", t => t.RepuestoId, cascadeDelete: true)
                .Index(t => t.FacturaClienteId)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.FacturaClientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false, storeType: "date"),
                        FechaPago = c.DateTime(nullable: false, storeType: "date"),
                        NumeroFactura = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Metodo = c.String(nullable: false),
                        Pagado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId, cascadeDelete: true)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.DetallesFacturaProveedors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacturaProveedorId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Precio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FacturaProveedors", t => t.FacturaProveedorId, cascadeDelete: false)
                .ForeignKey("dbo.Repuestoes", t => t.RepuestoId, cascadeDelete: true)
                .Index(t => t.FacturaProveedorId)
                .Index(t => t.RepuestoId);
            
            CreateTable(
                "dbo.FacturaProveedors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false, storeType: "date"),
                        FechaPago = c.DateTime(nullable: false, storeType: "date"),
                        NumeroFactura = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        TotalPagado = c.Int(nullable: false),
                        Metodo = c.String(nullable: false),
                        Pagado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proveedors", t => t.ProveedorId, cascadeDelete: true)
                .Index(t => t.ProveedorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallesFacturaProveedors", "RepuestoId", "dbo.Repuestoes");
            DropForeignKey("dbo.FacturaProveedors", "ProveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.DetallesFacturaProveedors", "FacturaProveedorId", "dbo.FacturaProveedors");
            DropForeignKey("dbo.DetallesFacturaClientes", "RepuestoId", "dbo.Repuestoes");
            DropForeignKey("dbo.DetallesFacturaClientes", "FacturaClienteId", "dbo.FacturaClientes");
            DropForeignKey("dbo.FacturaClientes", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.FacturaProveedors", new[] { "ProveedorId" });
            DropIndex("dbo.DetallesFacturaProveedors", new[] { "RepuestoId" });
            DropIndex("dbo.DetallesFacturaProveedors", new[] { "FacturaProveedorId" });
            DropIndex("dbo.FacturaClientes", new[] { "ClienteId" });
            DropIndex("dbo.DetallesFacturaClientes", new[] { "RepuestoId" });
            DropIndex("dbo.DetallesFacturaClientes", new[] { "FacturaClienteId" });
            DropTable("dbo.FacturaProveedors");
            DropTable("dbo.DetallesFacturaProveedors");
            DropTable("dbo.FacturaClientes");
            DropTable("dbo.DetallesFacturaClientes");
        }
    }
}
