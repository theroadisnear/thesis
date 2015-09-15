using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using library_prototype.Models;
using library_prototype.DAL;
using SimpleCrypto;
using System.IO;
using library_prototype.CustomClass;

namespace library_prototype.Controllers
{
    [Authorize(Roles = "administrator")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private LibraryDbContext db = new LibraryDbContext();

        public ActionResult Index()
        {
            var students = db.Students.ToList();

            return View(students);
        }

        [HttpPost]
        public ActionResult Index(string asd)
        {
            return View();
        }
        
        public ActionResult UpdateImage(Guid? id)
        {
            if (Request.Files.Count > 0)
            {
                var img = Request.Files[0];
                if (img == null)
                {
                    ModelState.AddModelError("ImageErrorMessage", "Image field is empty");
                }
                else if (img != null)
                {
                    string name = Guid.NewGuid().ToString() + "_" + Path.GetFileName(img.FileName);
                    string path = Path.Combine(Server.MapPath("~/Image/group_image"), name);
                    img.SaveAs(path);

                    var newImage = db.Images.Find(id);
                    newImage.Name = name;
                    newImage.Path = "~/Image/group_image/" + name;
                    newImage.UpdatedAt = DateTime.Now;
                    
                    db.Entry(newImage).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return View();
        }
        
        public ActionResult AddBook()
        {
            return View();
        }

        public ActionResult BookStatus()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserRegister()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccountIndex()
        {
            return View();
        }

        public ActionResult ShowUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserInformation(Guid? id)
        {
            MultipleModel.UserInformationVM users = new MultipleModel.UserInformationVM();
            var userTempData = TempData["UserInformationTD"] as MultipleModel.UserInformationVM;
            
            if (userTempData != null)
            {
                users.Error = userTempData.Error;
                users.Message = userTempData.Message;
            }

            if (id != null)
            {
                var getUser = db.Users.SingleOrDefault(u => u.Id == id);
                
                if(getUser != null && getUser.Deleted != true)
                {
                    users.User = getUser;
                }
                else if(getUser != null && getUser.Deleted == true)
                {
                    return RedirectToAction("UserIndex", new { id = getUser.Student.SectionId});
                }
            }
            else
            {
                return RedirectToAction("GradesIndex", "Admin");
            }
            return View(users);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateUser(MultipleModel.UserInformationVM vm)
        {
            var errorList = new List<string>();
            string message = null;
            var user = db.Users.SingleOrDefault(u=>u.Id == vm.User.Id);
            if(user != null)
            {
                if(ModelState.IsValid)
                {
                    user.Student.FirstName = vm.NewUserInfo.FirstName;
                    user.Student.MiddleInitial = vm.NewUserInfo.MiddleInitial;
                    user.Student.LastName = vm.NewUserInfo.LastName;
                    user.Student.Gender = vm.NewUserInfo.Gender;
                    user.Email = vm.NewUserInfo.EmailAddress;

                    user.UpdatedAt = DateTime.Now;
                    user.Student.UpdatedAt = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    vm.Error = false;
                    message = "You have successfully updated " + user.Student.LastName + "'s information";
                    errorList.Add(message);
                    vm.Message = errorList;
                    TempData["UserInformationTD"] = vm;
                    return RedirectToAction("UserInformation", new { id = vm.User.Id });
                }
                else
                {
                    vm.Error = true;
                    vm.Message = CustomValidationMessage.GetErrorList(ViewData.ModelState);
                    TempData["UserInformationTD"] = vm;
                    return RedirectToAction("UserInformation", new { id = vm.User.Id });
                }
            }
            vm.Error = true;
            message = "The selected user doesn't exist";
            errorList.Add(message);
            vm.Message = errorList;
            TempData["UserInformationTD"] = vm;

            return RedirectToAction("UserInformation", new { id = vm.User.Id});
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteUser(MultipleModel.UserInformationVM vm)
        {
            var errorList = new List<string>();
            string message = null;
            var user = db.Users.SingleOrDefault(u => u.Id == vm.User.Id);
            if (user != null)
            {
                user.Deleted = true;
                user.UpdatedAt = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                vm.Error = false;
                message = "You have successfully deleted " + user.Student.LastName + "'s account";
                errorList.Add(message);
                vm.Message = errorList;
                TempData["UserInformationTD"] = vm;
                
                return RedirectToAction("UserIndex", new { id = user.Student.SectionId });
            }
            vm.Error = true;
            message = "The selected user doesn't exist";
            errorList.Add(message);
            vm.Message = errorList;
            TempData["UserInformationTD"] = vm;

            return RedirectToAction("UserInformation", new { id = vm.User.Id });
        }

        [HttpGet]
        public ActionResult UserIndex(Guid? id)
        {
            var userVM = new MultipleModel.UserIndexVM();
            userVM.Error = null;
            var section = db.Sections.Find(id);
            userVM.GroupID = section.GradeId;
            userVM.SectionID = id;
            userVM.SectionName = section.Section;
            var getUsers = db.Users.Where(u=>u.Student.SectionId == id).Where(u => u.Deleted == false).OrderBy(u=>u.Student.Gender)
                .ThenBy(u=>u.Student.LastName).ToList();
            userVM.Users = getUsers;

            var tempData = TempData["UserIndexTD"] as MultipleModel.UserIndexVM;
            var userInformation = TempData["UserInformationTD"] as MultipleModel.UserInformationVM;

            if (tempData != null)
            {
                userVM.Error = tempData.Error;
                userVM.Message = tempData.Message;
                if(tempData.Error == false)
                {
                    userVM.Register = null;
                }
                else if (tempData.Error == true)
                {
                    userVM.Register = tempData.Register;
                }
            }
            else if(userInformation != null)
            {
                userVM.Error = userInformation.Error;
                userVM.Message = userInformation.Message;
            }
            return View(userVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUser(MultipleModel.UserIndexVM reg)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Where(u => u.Email == reg.Register.EmailAddress).Any())
                {
                    reg.Error = true;
                    var errorList = new List<string>();
                    string message = "Email " + reg.Register.EmailAddress + " is already existing";
                    errorList.Add(message);
                    reg.Message = errorList;

                    TempData["UserIndexTD"] = reg;
                    return RedirectToAction("UserIndex", new { id = reg.SectionID });
                }
                else
                {
                    using (var db = new LibraryDbContext())
                    {
                        var newUser = db.Users.Create();
                        string pin = RandomPassword.Generate(6, PasswordGroup.Lowercase, PasswordGroup.Lowercase, PasswordGroup.Numeric);
                        var crypto = new PBKDF2();
                        var encrypPin = crypto.Compute(pin);

                        newUser.Pincode = encrypPin;
                        newUser.PincodeSalt = crypto.Salt;

                        newUser.Email = reg.Register.EmailAddress;
                        newUser.Role = "student";
                        newUser.CreatedAt = DateTime.Now;
                        newUser.UpdatedAt = DateTime.Now;
                        db.Users.Add(newUser);
                        var newStudent = db.Students.Create();
                        var section = db.Sections.FirstOrDefault(s => s.Id == reg.SectionID);
                        newStudent.SectionId = section.Id;
                        newStudent.FirstName = reg.Register.FirstName;
                        newStudent.MiddleInitial = reg.Register.MiddleInitial;
                        newStudent.LastName = reg.Register.LastName;
                        newStudent.Gender = reg.Register.Gender;
                        newStudent.CreatedAt = DateTime.Now;
                        newStudent.UpdatedAt = DateTime.Now;

                        db.Students.Add(newStudent);
                        db.SaveChanges();

                        SMTP smtp = new SMTP();
                        smtp.SendEmal(newUser.Email, pin);

                        reg.Error = false;
                        var errorList = new List<string>();
                        string message = "You have successfully added a user(" + newUser.Email + ")";
                        errorList.Add(message);
                        reg.Message = errorList;
                        TempData["UserIndexTD"] = reg;

                        return RedirectToAction("UserIndex", new { id = reg.SectionID });
                    }
                }

            }
            else
            {
                reg.Error = true;
                reg.Message = CustomValidationMessage.GetErrorList(ViewData.ModelState);
            }
            TempData["UserIndexTD"] = reg;
            return RedirectToAction("UserIndex", new { id = reg.SectionID });
        }

        [HttpGet]
        public ActionResult GradesIndex()
        {
            MultipleModel.CreateGradeVM grades = new MultipleModel.CreateGradeVM();
            grades.Grades = db.Grades.OrderBy(g => g.Grade).ToList();

            var groupCreate = TempData["AddGroup"] as MultipleModel.CreateGradeVM;

            if (groupCreate != null)
            {
                grades.CreateGrade = groupCreate.CreateGrade;
                grades.Error = groupCreate.Error;
                grades.Message = groupCreate.Message;
            }

            return View(grades);
        }

        [HttpPost]
        public ActionResult CreateGroup(MultipleModel.CreateGradeVM vm)
        {
            if (ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    if (db.Grades.Where(u => u.Grade == vm.CreateGrade.Grade).Any())
                    {
                        vm.Error = true;
                        var errorList = new List<string>();
                        string message = "Grade " + vm.CreateGrade.Grade + " is already existing";
                        errorList.Add(message);
                        vm.Message = errorList;

                        TempData["AddGroup"] = vm;
                        return RedirectToAction("GradesIndex", "Admin");
                    }
                    else
                    {
                        var newGroup = db.Grades.Create();
                        newGroup.Grade = vm.CreateGrade.Grade;
                        newGroup.CreatedAt = DateTime.Now;
                        if (Request.Files.Count > 0)
                        {
                            var img = Request.Files[0];
                            if ((img != null) && (img.FileName == null))
                            {
                                string name = Guid.NewGuid().ToString() + "_" + Path.GetFileName(img.FileName);
                                string path = Path.Combine(Server.MapPath("~/Image/group_image"), name);
                                img.SaveAs(path);

                                var image = newGroup.Image;
                                image.Name = name;
                                image.Path = "~/Image/group_image/" + name;
                                image.CreatedAt = DateTime.Now;

                            }
                        }

                        db.Grades.Add(newGroup);
                        db.SaveChanges();

                        vm.Error = false;
                        var errorList = new List<string>();
                        string message = "You have successfully added a group(" + newGroup.Grade + ")";
                        errorList.Add(message);
                        vm.Message = errorList;
                        TempData["AddGroup"] = vm;

                        return RedirectToAction("GradesIndex", "Admin");
                    }
                }
            }
            vm.Error = true;
            vm.Message = CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["AddGroup"] = vm;
            return RedirectToAction("GradesIndex", "Admin");
        }

        [HttpGet]
        public ActionResult SectionIndex(Guid? id)
        {

            MultipleModel.CreateSectionVM vm = new MultipleModel.CreateSectionVM();
            vm.GroupID = id;
            vm.Sections = db.Sections.Where(s => s.GradeId == id).ToList();
            var getSectionName = db.Grades.Find(id);
            vm.SectionName = getSectionName.Grade;

            var sectionCreate = TempData["AddSection"] as MultipleModel.CreateSectionVM;

            if (sectionCreate != null)
            {
                vm.CreateSection = sectionCreate.CreateSection;
                vm.Error = sectionCreate.Error;
                vm.Message = sectionCreate.Message;
            }

            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateSection(MultipleModel.CreateSectionVM vm)
        {
            if (ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    if (db.Sections.Where(u => u.Section == vm.CreateSection.Section).Any())
                    {
                        vm.Error = true;
                        var errorList = new List<string>();
                        string message = "Section " + vm.CreateSection.Section + " is already existing";
                        errorList.Add(message);
                        vm.Message = errorList;
                        TempData["AddSection"] = vm;
                        return RedirectToAction("SectionIndex", new { id = vm.GroupID });
                    }
                    else
                    {
                        var grade = db.Grades.Find(vm.GroupID);
                        var section = db.Sections.Create();
                        section.Section = vm.CreateSection.Section;
                        section.CreatedAt = DateTime.Now;
                        grade.Sections.Add(section);
                        db.SaveChanges();

                        vm.Error = false;
                        var errorList = new List<string>();
                        string message = "You have successfully added a section(" + vm.CreateSection.Section + ")";
                        errorList.Add(message);
                        vm.Message = errorList;
                        TempData["UserRegistration"] = vm;

                        return RedirectToAction("SectionIndex", new { id = section.GradeId });
                    }
                }
            }
            vm.Error = true;
            vm.Message = CustomValidationMessage.GetErrorList(ViewData.ModelState);
            TempData["AddSection"] = vm;
            return RedirectToAction("SectionIndex", new { id = vm.GroupID });
        }
        
        public ActionResult LibraryIndex()
        {
            return View();
        }     
    }
    
}
