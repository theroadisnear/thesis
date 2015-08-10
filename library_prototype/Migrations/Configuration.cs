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
            var id = new Guid();
            var crypto = new SimpleCrypto.PBKDF2();
            var encrypPass = crypto.Compute("rodnerraymundo");
            string pin = RandomPassword.Generate(6, PasswordGroup.Lowercase, PasswordGroup.Lowercase, PasswordGroup.Numeric);

            var accounts = new List<library_prototype.DAL.LibraryDbContext.UserModel>
            {

                new library_prototype.DAL.LibraryDbContext.UserModel
                {
                    UserId = id, Email = "rodnerraymundo@gmail.com",
                    Password = encrypPass, PasswordSalt = crypto.Salt, Pincode = pin, Role = "administrator",
                    SecretQuestion = "Who are you?", SecretAnswer = "rodnerraymundo", CreatedAt = DateTime.Now,
                    Status = true
                },
            };
            accounts.ForEach(a => context.Users.AddOrUpdate(a));

            var information = new List<library_prototype.DAL.LibraryDbContext.StudentModel>
            {
                new DAL.LibraryDbContext.StudentModel
                {
                    UserId =id, FirstName = "Rodner", MiddleInitial = "Y", LastName = "Raymundo", Status = true,
                    ContactNumber = "09176508082", CreatedAt = DateTime.Now, Gender = "male",
                }
            };
            information.ForEach(i => context.Students.AddOrUpdate(i));


            context.SaveChanges();
            base.Seed(context);
        }
    }
}
