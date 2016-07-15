namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagenMarcas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Marcas", "Imagen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Marcas", "Imagen");
        }
    }
}
