namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actualizar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FacturaClientes", "Total", c => c.Int(nullable: false));
            AlterColumn("dbo.FacturaClientes", "TotalPagado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FacturaClientes", "TotalPagado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.FacturaClientes", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
