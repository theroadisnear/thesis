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
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class LoginModelVM
        {
            public AuthModel AuthModel { get; set; }
            public ActivationModel ActivationModel { get; set; }
            public LibraryDbContext.UserModel UserModel { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class ProfileVM
        {

            public LibraryDbContext.UserModel UserModel { get; set; }
            public LibraryDbContext.UserAddressModel UserAddressModel { get; set; }
            public LibraryDbContext.StudentModel StudentModel { get; set; }
            public Models.ImageModel ImageModel { get; set; }
        }

        public class CreateGradeVM
        {
            public ICollection<LibraryDbContext.GradesModel> Grades { get; set; }
            public ICollection<LibraryDbContext.ImageModel> Images { get; set; }
            public ImageModel ImageModel { get; set; }
            public CreateGradeModel CreateGrade { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class CreateSectionVM
        {
            public ICollection<LibraryDbContext.SectionsModel> Sections { get; set; }
            public ICollection<LibraryDbContext.ImageModel> Images { get; set; }
            public ImageModel ImageModel { get; set; }
            public CreateSectionModel CreateSection { get; set; }
            public string SectionName { get; set; }
            public Guid? GroupID { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class UserIndexVM
        {
            public IEnumerable<LibraryDbContext.UserModel> Users { get; set; }
            public ICollection<LibraryDbContext.StudentModel> Student { get; set; }
            public RegisterModel Register { get; set; }
            public Guid? SectionID { get; set; }
            public Guid? GroupID { get; set; }
            public string SectionName { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

        public class UserInformationVM
        {
            public LibraryDbContext.UserModel User { get; set; }
            public LibraryDbContext.SectionsModel Section { get; set; }
            public UpdateUser NewUserInfo { get; set; }
            public bool? Error { get; set; }
            public List<string> Message { get; set; }
        }

    }
}