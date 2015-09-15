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
                        Delete = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.ImageId)
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
                .PrimaryKey(t => t.ImageId);
            
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
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .Index(t => t.GradeId)
                .Index(t => t.ImageId)
                .Index(t => t.Section, unique: true);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SectionId = c.Guid(nullable: false),
                        FirstName = c.String(),
                        MiddleInitial = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Birthday = c.DateTime(),
                        ContactNumber = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StudentId = c.Guid(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ImageId)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAddresses", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Users", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Students", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Sections", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Grades", "ImageId", "dbo.Images");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "ImageId" });
            DropIndex("dbo.Users", new[] { "StudentId" });
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropIndex("dbo.Students", new[] { "SectionId" });
            DropIndex("dbo.Sections", new[] { "Section" });
            DropIndex("dbo.Sections", new[] { "ImageId" });
            DropIndex("dbo.Sections", new[] { "GradeId" });
            DropIndex("dbo.Grades", new[] { "Grade" });
            DropIndex("dbo.Grades", new[] { "ImageId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserAddresses");
            DropTable("dbo.Students");
            DropTable("dbo.Sections");
            DropTable("dbo.Images");
            DropTable("dbo.Grades");
        }
    }
}
