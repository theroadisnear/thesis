namespace library_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LibraryDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Grade = c.String(maxLength: 450),
                        Section = c.String(maxLength: 450),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Grade, unique: true)
                .Index(t => t.Section, unique: true);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Path = c.String(),
                        Name = c.String(maxLength: 450),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        Email = c.String(maxLength: 450),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        Pincode = c.String(maxLength: 450),
                        Role = c.String(),
                        SecretQuestion = c.String(),
                        SecretAnswer = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Pincode, unique: true);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        FirstName = c.String(),
                        MiddleInitial = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Grade = c.String(),
                        Section = c.String(),
                        Birthday = c.DateTime(),
                        ContactNumber = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserAddresses",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAddresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Images", "UserId", "dbo.Users");
            DropForeignKey("dbo.Students", "UserId", "dbo.Users");
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Pincode" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Images", new[] { "Name" });
            DropIndex("dbo.Images", new[] { "UserId" });
            DropIndex("dbo.Grades", new[] { "Section" });
            DropIndex("dbo.Grades", new[] { "Grade" });
            DropTable("dbo.UserAddresses");
            DropTable("dbo.Students");
            DropTable("dbo.Users");
            DropTable("dbo.Images");
            DropTable("dbo.Grades");
        }
    }
}
