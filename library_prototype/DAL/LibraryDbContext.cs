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
            Database.SetInitializer<LibraryDbContext>(new CreateDatabaseIfNotExists<LibraryDbContext>());
            Database.SetInitializer<LibraryDbContext>(new DropCreateDatabaseIfModelChanges<LibraryDbContext>());

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserAddressModel> UserAddresses { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<GradesModel> Grades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        [Table("Users")]
        public class UserModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid UserId { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordSalt { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Pincode { get; set; }
            public string Role { get; set; }
            public string SecretQuestion { get; set; }
            public string SecretAnswer { get; set; }
            [DefaultValue("false")]
            public bool Deleted { get; set; }
            public bool Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual StudentModel Student { get; set; }
        }

        [Table("Students")]
        public class StudentModel
        {
            [Key, ForeignKey("User")]
            public Guid UserId { get; set; }
            public string FirstName { get; set; }
            public string MiddleInitial { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public string Grade { get; set; }
            public string Section { get; set; }
            [DisplayFormat(DataFormatString = "{MM/DD/YYYY}")]
            [Range(typeof(DateTime), "1/1/1950", "12/31/2012")]
            public DateTime? Birthday { get; set; }
            public string ContactNumber { get; set; }
            public bool Status { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual UserModel User { get; set; }
        }

        [Table("Grades")]
        public class GradesModel
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Guid Id { get; set; }
            [StringLength(450)]
            [Index(IsUnique =true)]
            public string Grade { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public String Section { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
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

        [Table("Images")]
        public class ImageModel
        {
            [Key, ForeignKey("User")]
            public Guid UserId { get; set; }
            public string Path { get; set; }
            [StringLength(450)]
            [Index(IsUnique = true)]
            public string Name { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public virtual UserModel User { get; set; }
        }
        
    }
}