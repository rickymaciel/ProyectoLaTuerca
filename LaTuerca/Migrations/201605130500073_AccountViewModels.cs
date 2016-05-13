namespace LaTuerca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountViewModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(nullable: false),
                        Documento = c.String(nullable: false),
                        Direccion = c.String(nullable: false),
                        Telefono = c.String(nullable: false),
                        Celular = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        NombreCorto = c.String(nullable: false),
                        Ruc = c.Int(nullable: false),
                        Departamento = c.String(),
                        Ciudad = c.String(),
                        Pais = c.String(),
                        Email = c.String(),
                        Direccion = c.String(),
                        Telefono = c.String(),
                        Web = c.String(),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Marcas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Repuestoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 60),
                        ProveedorId = c.Int(nullable: false),
                        MarcaId = c.Int(nullable: false),
                        ModeloId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                        StockMinimo = c.Int(nullable: false),
                        StockMaximo = c.Int(nullable: false),
                        PrecioCosto = c.Single(nullable: false),
                        PrecioVenta1 = c.Single(nullable: false),
                        PrecioVenta2 = c.Single(nullable: false),
                        PrecioVenta3 = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Marcas", t => t.MarcaId, cascadeDelete: true)
                .ForeignKey("dbo.Modeloes", t => t.ModeloId, cascadeDelete: true)
                .ForeignKey("dbo.Proveedors", t => t.ProveedorId, cascadeDelete: true)
                .Index(t => t.ProveedorId)
                .Index(t => t.MarcaId)
                .Index(t => t.ModeloId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.Modeloes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreModelo = c.String(nullable: false, maxLength: 60),
                        Fabricante = c.String(nullable: false),
                        Anho = c.Int(nullable: false),
                        Cilindros = c.String(nullable: false),
                        Potencia = c.String(nullable: false),
                        Tipo_Cambio = c.String(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proveedors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(nullable: false, maxLength: 60),
                        Ruc = c.String(nullable: false, maxLength: 10),
                        Direccion = c.String(nullable: false),
                        Telefono = c.String(nullable: false),
                        Celular = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Repuestoes", "ProveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.Repuestoes", "ModeloId", "dbo.Modeloes");
            DropForeignKey("dbo.Repuestoes", "MarcaId", "dbo.Marcas");
            DropForeignKey("dbo.Repuestoes", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Repuestoes", new[] { "CategoriaId" });
            DropIndex("dbo.Repuestoes", new[] { "ModeloId" });
            DropIndex("dbo.Repuestoes", new[] { "MarcaId" });
            DropIndex("dbo.Repuestoes", new[] { "ProveedorId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Menus");
            DropTable("dbo.Proveedors");
            DropTable("dbo.Modeloes");
            DropTable("dbo.Repuestoes");
            DropTable("dbo.Marcas");
            DropTable("dbo.Empresas");
            DropTable("dbo.Clientes");
            DropTable("dbo.Categorias");
        }
    }
}
