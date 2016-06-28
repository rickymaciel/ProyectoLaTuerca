namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Caja : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cajas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha_Apertura = c.DateTime(nullable: false),
                        Inicial = c.Int(nullable: false),
                        Fecha_Cierre = c.DateTime(),
                        Cierre = c.Int(),
                        Operaciones = c.Int(nullable: false),
                        Usuario = c.String(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovimientoCajas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CajaId = c.Int(nullable: false),
                        Concepto = c.String(nullable: false),
                        Movimiento = c.String(nullable: false),
                        Monto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cajas", t => t.CajaId, cascadeDelete: true)
                .Index(t => t.CajaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovimientoCajas", "CajaId", "dbo.Cajas");
            DropIndex("dbo.MovimientoCajas", new[] { "CajaId" });
            DropTable("dbo.MovimientoCajas");
            DropTable("dbo.Cajas");
        }
    }
}
