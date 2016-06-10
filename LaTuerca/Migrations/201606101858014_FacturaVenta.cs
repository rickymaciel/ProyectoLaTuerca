namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacturaVenta : DbMigration
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
                        FacturaCliente_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.FacturaClientes", t => t.FacturaCliente_Id)
                .Index(t => t.ClienteId)
                .Index(t => t.FacturaCliente_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallesFacturaClientes", "RepuestoId", "dbo.Repuestoes");
            DropForeignKey("dbo.DetallesFacturaClientes", "FacturaClienteId", "dbo.FacturaClientes");
            DropForeignKey("dbo.FacturaClientes", "FacturaCliente_Id", "dbo.FacturaClientes");
            DropForeignKey("dbo.FacturaClientes", "ClienteId", "dbo.Clientes");
            DropIndex("dbo.FacturaClientes", new[] { "FacturaCliente_Id" });
            DropIndex("dbo.FacturaClientes", new[] { "ClienteId" });
            DropIndex("dbo.DetallesFacturaClientes", new[] { "RepuestoId" });
            DropIndex("dbo.DetallesFacturaClientes", new[] { "FacturaClienteId" });
            DropTable("dbo.FacturaClientes");
            DropTable("dbo.DetallesFacturaClientes");
        }
    }
}
