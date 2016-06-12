namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacturaCliente2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FacturaClientes", "FacturaCliente_Id", "dbo.FacturaClientes");
            DropIndex("dbo.FacturaClientes", new[] { "FacturaCliente_Id" });
            DropColumn("dbo.FacturaClientes", "FacturaCliente_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FacturaClientes", "FacturaCliente_Id", c => c.Int());
            CreateIndex("dbo.FacturaClientes", "FacturaCliente_Id");
            AddForeignKey("dbo.FacturaClientes", "FacturaCliente_Id", "dbo.FacturaClientes", "Id");
        }
    }
}
