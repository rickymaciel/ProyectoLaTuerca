namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagenRepuesto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repuestoes", "Imagen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repuestoes", "Imagen");
        }
    }
}
