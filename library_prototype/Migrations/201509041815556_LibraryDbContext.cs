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
                        ImageId = c.Guid(),
                        Grade = c.String(maxLength: 450),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Grade, unique: true);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                        Name = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Grades", t => t.ImageId)
                .ForeignKey("dbo.Sections", t => t.ImageId)
                .ForeignKey("dbo.Users", t => t.ImageId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        GradeId = c.Guid(nullable: false),
                        ImageId = c.Guid(),
                        Section = c.String(maxLength: 450),
                        Deleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.GradeId)
                .Index(t => t.GradeId)
                .Index(t => t.Section, unique: true);
            
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
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        ImageId = c.Guid(),
                        Email = c.String(maxLength: 450),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        Pincode = c.String(),
                        PincodeSalt = c.String(),
                        Role = c.String(),
                        SecretQuestion = c.String(),
                        SecretAnswer = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Email, unique: true);
            
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
            DropForeignKey("dbo.Students", "UserId", "dbo.Users");
            DropForeignKey("dbo.Images", "ImageId", "dbo.Users");
            DropForeignKey("dbo.Images", "ImageId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Images", "ImageId", "dbo.Grades");
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Students", new[] { "UserId" });
            DropIndex("dbo.Sections", new[] { "Section" });
            DropIndex("dbo.Sections", new[] { "GradeId" });
            DropIndex("dbo.Images", new[] { "ImageId" });
            DropIndex("dbo.Grades", new[] { "Grade" });
            DropTable("dbo.UserAddresses");
            DropTable("dbo.Users");
            DropTable("dbo.Students");
            DropTable("dbo.Sections");
            DropTable("dbo.Images");
            DropTable("dbo.Grades");
        }
    }
}
