namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cheque : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.Cheques",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BancoId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        NroCheque = c.Int(nullable: false),
                        Monto = c.Int(nullable: false),
                        Lugar = c.String(nullable: false, maxLength: 60),
                        Fecha = c.String(nullable: false, maxLength: 60),
                        nroCuenta = c.String(nullable: false, maxLength: 60),
                        Serie = c.String(nullable: false, maxLength: 60),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bancoes", t => t.BancoId, cascadeDelete: true)
                .ForeignKey("dbo.Proveedors", t => t.ProveedorId, cascadeDelete: true)
                .Index(t => t.BancoId)
                .Index(t => t.ProveedorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cheques", "ProveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.Cheques", "BancoId", "dbo.Bancoes");
            DropIndex("dbo.Cheques", new[] { "ProveedorId" });
            DropIndex("dbo.Cheques", new[] { "BancoId" });
            DropTable("dbo.Cheques");
        }
    }
}
