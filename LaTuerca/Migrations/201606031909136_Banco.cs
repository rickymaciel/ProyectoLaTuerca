namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Banco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bancoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        Direccion = c.String(nullable: false, maxLength: 60),
                        Telefono = c.String(nullable: false, maxLength: 60),
                        Ruc = c.String(nullable: false, maxLength: 60),
                        CuentaCorriente = c.String(nullable: false, maxLength: 60),
                        Email = c.String(nullable: false, maxLength: 60),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bancoes");
        }
    }
}
