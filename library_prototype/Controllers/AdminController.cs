using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using library_prototype.Models;
using library_prototype.DAL;
using SimpleCrypto;

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

        
        [HttpGet]
        public ActionResult UserIndex()
        {
            var students = db.Students.ToList();
            return View(students);
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

        [HttpPost]
        public ActionResult UserRegister(RegisterModel reg)
        {
            if(ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    var newUser = db.Users.Create();
                    string pin = RandomPassword.Generate(6, PasswordGroup.Lowercase, PasswordGroup.Lowercase, PasswordGroup.Numeric);
                    newUser.Pincode = pin;
                    newUser.Email = reg.EmailAddress;
                    newUser.Role = "student";
                    newUser.CreatedAt = DateTime.Now;
                    newUser.UpdatedAt = DateTime.Now;
                    db.Users.Add(newUser);

                    var newStudent = db.Students.Create();

                    newStudent.FirstName = reg.FirstName;
                    newStudent.MiddleInitial = reg.MiddleInitial;
                    newStudent.LastName = reg.LastName;
                    newStudent.Gender = reg.Gender;
                    newStudent.Grade = reg.Grade;
                    newStudent.Section = reg.Section;
                    newStudent.CreatedAt = DateTime.Now;
                    newStudent.UpdatedAt = DateTime.Now;

                    db.Students.Add(newStudent);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("UserIndex","Admin");
            }

            else
            {
                ModelState.AddModelError("", "Registration failed");
            }
            return View(reg);
        }

        [HttpGet]
        public ActionResult AccountIndex()
        {
            return View();
        }

        public ActionResult ReceiveForm()
        {
            return View();
        }

        public ActionResult Reservation()
        {
            return View();
        }

        public ActionResult ShowUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditUser(Guid? id)
        {
            var users = db.Users.Find(id);
            return View(users);
        }

        [HttpGet]
        public ActionResult EditForms()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormSection()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateSection(FormManagementModel.FormModel form)
        {
            if (ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    var section = db.Grades.Create();
                    section.Grade = form.Grade;
                    section.Section = form.Section;
                    section.CreatedAt = DateTime.Now;

                    db.Grades.Add(section);

                    db.SaveChanges();

                    return RedirectToAction("EditForm", "Admin");
                }
            }
            return View(form);
        }
        
        [HttpPost]
        public ActionResult UpdateSection()
        {
            return View();
        }       
    }
    
}
