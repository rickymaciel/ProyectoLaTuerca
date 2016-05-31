namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Compras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false, storeType: "date"),
                        NumeroFactura = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pagado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proveedors", t => t.ProveedorId, cascadeDelete: true)
                .Index(t => t.ProveedorId);
            
            CreateTable(
                "dbo.CompraDetalles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompraId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        RepuestoId = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Compras", t => t.CompraId, cascadeDelete: true)
                .ForeignKey("dbo.Repuestoes", t => t.RepuestoId, cascadeDelete: false)
                .Index(t => t.CompraId)
                .Index(t => t.RepuestoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Compras", "ProveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.CompraDetalles", "RepuestoId", "dbo.Repuestoes");
            DropForeignKey("dbo.CompraDetalles", "CompraId", "dbo.Compras");
            DropIndex("dbo.CompraDetalles", new[] { "RepuestoId" });
            DropIndex("dbo.CompraDetalles", new[] { "CompraId" });
            DropIndex("dbo.Compras", new[] { "ProveedorId" });
            DropTable("dbo.CompraDetalles");
            DropTable("dbo.Compras");
        }
    }
}
