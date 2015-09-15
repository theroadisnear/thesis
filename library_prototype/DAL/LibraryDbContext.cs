using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel;

namespace library_prototype.DAL
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("LibraryDbContext")
        {
            //Database.SetInitializer<LibraryDbContext>(new LibraryDbSeed());
            //Database.SetInitializer<LibraryDbContext>(new CreateDatabaseIfNotExists<LibraryDbContext>());
            //Database.SetInitializer<LibraryDbContext>(new DropCreateDatabaseIfModelChanges<LibraryDbContext>());

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserAddressModel> UserAddresses { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<SectionsModel> Sections { get; set; }
        public DbSet<GradesModel> Grades { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//modelBuilder.Entity<UserModel>().HasRequired(u => u.Image).WithRequiredDependent( u => u.User).WillCascadeOnDelete(false);
            //modelBuilder.Entity<GradesModel>().HasRequired(u => u.Image).WithRequiredDependent( g => g.Grade).WillCascadeOnDelete(false);
            //modelBuilder.Entity<SectionsModel>().HasRequired(u => u.Image).WithRequiredDependent( s => s.Section).WillCascadeOnDelete(false);


            //modelBuilder.Entity<UserModel>().HasOptional(i => i.Image).WithOptionalDependent(i => i.User).WillCascadeOnDelete(false);
            //modelBuilder.Entity<GradesModel>().HasOptional(i => i.Image).WithOptionalDependent(i => i.Grade).WillCascadeOnDelete(false);
            //modelBuilder.Entity<SectionsModel>().HasOptional(i => i.Image).WithOptionalDependent(i => i.Section).WillCascadeOnDelete(false);
            //

            //modelBuilder.Entity<ImageModel>().HasOptional(i => i.Grade).WithOptionalPrincipal();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Entity<UserModel>().HasRequired(u => u.Image).WithRequiredPrincipal().WillCascadeOnDelete(false);
            //modelBuilder.Entity<GradesModel>().HasRequired(u => u.Image).WithRequiredPrincipal().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SectionsModel>().HasRequired(u => u.Image).WithRequiredPrincipal().WillCascadeOnDelete(false);
        }

        [Table("Users")]
        public class UserModel
        {
            [Column(Order = 0), Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Student")]
            public Guid StudentId { get; set; }
            [ForeignKey("Image")]
            public Guid? ImageId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordSalt { get; set; }
            public string Pincode { get; set; }
            public string PincodeSalt { get; set; }
            public string Role { get; set; }
            public string SecretQuestion { get; set; }
            public string SecretAnswer { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public bool Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual StudentModel Student { get; set; }
            public virtual ImageModel Image { get; set; }
        }

        [Table("Students")]
        public class StudentModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Section")]
            public Guid SectionId { get; set; }
            public string FirstName { get; set; }
            public string MiddleInitial { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            [DisplayFormat(DataFormatString = "{MM/DD/YYYY}")]
            [Range(typeof(DateTime), "1/1/1950", "12/31/2012")]
            public DateTime? Birthday { get; set; }
            public string ContactNumber { get; set; }
            public bool Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual SectionsModel Section{ get; set; }
        }

        [Table("UserAddresses")]
        public class UserAddressModel
        {
            [Key, ForeignKey("User")]
            public Guid UserId { get; set; }
            public int ZipCode { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual UserModel User { get; set; }
        }


        [Table("Sections")]
        public class SectionsModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Grade")]
            public Guid GradeId { get; set; }
            [ForeignKey("Image")]
            public Guid? ImageId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public String Section { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual GradesModel Grade { get; set; }
            public virtual ImageModel Image { get; set; }

        }

        [Table("Grades")]
        public class GradesModel
        {
            [Column(Order = 0), Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [ForeignKey("Image")]
            public Guid? ImageId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Grade { get; set; }
            [DefaultValue("false")]
            public bool Delete { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual ICollection<SectionsModel> Sections { get; set; }
            public virtual ImageModel Image { get; set; }

        }


        [Table("Images")]
        public class ImageModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid ImageId { get; set; }
            public string Path { get; set; }
            public string Name { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            
        }

    }
}