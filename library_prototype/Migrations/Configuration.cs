namespace library_prototype.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SimpleCrypto;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<library_prototype.DAL.LibraryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(library_prototype.DAL.LibraryDbContext context)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute("rodnerraymundo");
            string pin = RandomPassword.Generate(6, PasswordGroup.Lowercase, PasswordGroup.Lowercase, PasswordGroup.Numeric);
            var cryptoPin = new SimpleCrypto.PBKDF2();
            var encrypPin = crypto.Compute(pin);

            var grades = new List<library_prototype.DAL.LibraryDbContext.GradesModel>
            {
                new library_prototype.DAL.LibraryDbContext.GradesModel
                {
                    Grade = "Administrator", CreatedAt = DateTime.Now,
                    Sections = new List<library_prototype.DAL.LibraryDbContext.SectionsModel>
                    {
                        context.Sections.SingleOrDefault(s=>s.Section == "Developer")
                    }
                }
            };
            grades.ForEach(g => context.Grades.AddOrUpdate(g));

            var sections = new List<library_prototype.DAL.LibraryDbContext.SectionsModel>
            {
                new library_prototype.DAL.LibraryDbContext.SectionsModel
                {
                    Section = "Developer", CreatedAt = DateTime.Now,
                    
                }
            };
            sections.ForEach(s => context.Sections.AddOrUpdate(s));

            var accounts = new List<library_prototype.DAL.LibraryDbContext.UserModel>
            {

                new library_prototype.DAL.LibraryDbContext.UserModel
                {
                    Email = "rodnerraymundo@gmail.com",
                    Password = encrypPass, PasswordSalt = crypto.Salt, Pincode = encrypPin, PincodeSalt = cryptoPin.Salt,
                    Role = "administrator", SecretQuestion = "Who are you?", SecretAnswer = "rodnerraymundo",
                    CreatedAt = DateTime.Now, Status = true,
                    Student = new DAL.LibraryDbContext.StudentModel
                    {
                        FirstName = "Rodner", MiddleInitial = "Y", LastName = "Raymundo", Status = true,
                        ContactNumber = "09176508082", CreatedAt = DateTime.Now, Gender = "male",
                    }
                },
            };
            accounts.ForEach(a => context.Users.AddOrUpdate(a));

            /*var information = new List<library_prototype.DAL.LibraryDbContext.StudentModel>
            {
                new DAL.LibraryDbContext.StudentModel
                {
                    FirstName = "Rodner", MiddleInitial = "Y", LastName = "Raymundo", Status = true,
                    ContactNumber = "09176508082", CreatedAt = DateTime.Now, Gender = "male",
                }
            };
            information.ForEach(i => context.Students.AddOrUpdate(i));
            */
            /*
            var sections = new List<library_prototype.DAL.LibraryDbContext.SectionsModel>
            {
                new DAL.LibraryDbContext.SectionsModel
                {
                    Section = "Administrator", CreatedAt = DateTime.Now,
                },

                new DAL.LibraryDbContext.SectionsModel
                {
                    Section = "Co-Administrator", CreatedAt = DateTime.Now,
                }
            };
            var nonStudentGroup = context.Grades.FirstOrDefault(g => g.Grade == "Non-student");
            sections.ForEach(s => nonStudentGroup.Sections.Add(s));
            context.SaveChanges();
            */
            base.Seed(context);
        }
    }
}
