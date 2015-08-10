using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using library_prototype.DAL;
using library_prototype.Models;
namespace library_prototype.Models
{
    public class MultipleModel
    {
        public class AuthModelVM
        {
            public ActivationModel1 ActivationModel1 { get; set; }
            public LibraryDbContext.UserModel UserModel { get; set; }
            public LibraryDbContext.UserAddressModel UserAddressModel { get; set; }
        }

        public class LoginModelVM
        {
            public AuthModel AuthModel { get; set; }
            public ActivationModel ActivationModel { get; set; }
            public LibraryDbContext.UserModel UserModel { get; set; }
        }

        public class EditUserModel
        {
            public LibraryDbContext.UserModel UserModel { get; set; }
            public LibraryDbContext.UserAddressModel UserAddressModel { get; set; }
            public LibraryDbContext.StudentModel StudentModel { get; set; }
        }
    }
}