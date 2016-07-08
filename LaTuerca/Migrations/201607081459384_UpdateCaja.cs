namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCaja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cajas", "Entrada", c => c.Int());
            AddColumn("dbo.Cajas", "Salida", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cajas", "Salida");
            DropColumn("dbo.Cajas", "Entrada");
        }
    }
}
