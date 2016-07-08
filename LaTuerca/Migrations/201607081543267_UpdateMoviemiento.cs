namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMoviemiento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovimientoCajas", "Ingreso", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientoCajas", "Egreso", c => c.Int(nullable: false));
            AddColumn("dbo.MovimientoCajas", "Saldo", c => c.Int(nullable: false));
            DropColumn("dbo.MovimientoCajas", "Monto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovimientoCajas", "Monto", c => c.Int(nullable: false));
            DropColumn("dbo.MovimientoCajas", "Saldo");
            DropColumn("dbo.MovimientoCajas", "Egreso");
            DropColumn("dbo.MovimientoCajas", "Ingreso");
        }
    }
}
