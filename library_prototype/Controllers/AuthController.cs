﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using library_prototype.Models;
using library_prototype.DAL;
using SimpleCrypto;
using System.Security.Claims;
using System.Net.Http;
using System.Data.Entity;

namespace library_prototype.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "administrator")]
    public class AuthController : Controller
    {
        


        [HttpGet]
        public ActionResult Login()
        {
            MultipleModel.LoginModelVM loginVM = new MultipleModel.LoginModelVM(); 
            return View(loginVM);
        }

        [HttpPost]
        public ActionResult Login(MultipleModel.LoginModelVM user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var emailCheck = db.Users.FirstOrDefault(u => u.Email == user.AuthModel.Email);
                    var getPasswordSalt = db.Users.Where(u => u.Email == user.AuthModel.Email).Select(u => u.PasswordSalt);
                    
                    if ((emailCheck != null) && (getPasswordSalt != null) && (emailCheck.Status == true))
                    {
                        var materializePasswordSalt = getPasswordSalt.ToList();
                        var passwordSalt = materializePasswordSalt[0];
                        var encryptedPassword = crypto.Compute(user.AuthModel.Password, passwordSalt);

                        if (user.AuthModel.Email != null && emailCheck.Password == encryptedPassword)
                        {
                            var getName = db.Students.Where(u => u.UserId == emailCheck.UserId).Select(u => u.FirstName);
                            var materializeName = getName.ToList();
                            var name = materializeName[0];

                            var getEmail = db.Users.Where(u => u.UserId == emailCheck.UserId).Select(u => u.Email);
                            var materializeEmail = getEmail.ToList();
                            var email = materializeEmail[0];

                            var getRole = db.Users.Where(u => u.UserId == emailCheck.UserId).Select(u => u.Role);
                            var materializeRole = getRole.ToList();
                            var role = materializeRole[0];

                            var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, name),
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, role)
                        }, "ApplicationCookie");

                            var ctx = Request.GetOwinContext();
                            var authManager = ctx.Authentication;
                            authManager.SignIn(identity);

                            if (emailCheck.Role == "administrator")
                            {
                                return RedirectToAction("UserIndex", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Auth");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid email or password");
                        }
                    }
                    else if((emailCheck != null) && (emailCheck.Status == false))
                    {
                        ModelState.AddModelError("", "Please activate the account");
                    }
                    else if(emailCheck == null)
                    {
                        ModelState.AddModelError("", "Account does not exist");
                    }
                }
                
            }
            
            return View(user);
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult AccountActivationSaveData([Bind(Prefix ="Item2")] ActivationModel act)
        {
            var authModel = new AuthModel();
            var activationModel = new ActivationModel();
            var tuple = Tuple.Create(authModel, activationModel);
            tuple.Item2.Email = act.Email;
            tuple.Item2.PinCode = act.PinCode;
            //var gg= "<p class='uk-text-danger'>" + act.Email + "</p>";
            return Json(new { success = "true" },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book()
        {
            return View();
        }
        
        
        [HttpPost]
        public ActionResult ActivateAccount(MultipleModel.LoginModelVM login)
        {
            if(ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    var email = db.Users.SingleOrDefault(u => u.Email == login.ActivationModel.Email);

                    if ((email.Password != null) && (email.PasswordSalt != null))
                    {
                        ModelState.AddModelError("", "The account is already activated");
                    }
                    else if ((email != null) && (email.Pincode == login.ActivationModel.PinCode))
                    {
                        return RedirectToAction("ActivateAccount2", new { id = email.UserId});
                    }
                }
            }
            ModelState.AddModelError("","The account does not exist");
            return View("Login", login);
        }
        
        [HttpGet]
        public ActionResult ActivateAccount2(Guid? id)
        {
            using (var db = new LibraryDbContext())
            {
                MultipleModel.AuthModelVM vm = new MultipleModel.AuthModelVM();
                vm.UserModel = db.Users.Include(u => u.Student).SingleOrDefault(u => u.UserId == id);
                return View(vm);
            }
        }

        [HttpPost]
        public ActionResult ActivateAccount2(MultipleModel.AuthModelVM request)
        {
            if(ModelState.IsValid)
            {
                using (var db = new LibraryDbContext())
                {
                    MultipleModel.AuthModelVM vm = new MultipleModel.AuthModelVM();
                    vm.UserModel = db.Users.SingleOrDefault(u => u.UserId == request.UserModel.UserId);
                    var crypto = new SimpleCrypto.PBKDF2();
                    var encrypPass = crypto.Compute(request.ActivationModel1.Password);

                    vm.UserModel.PasswordSalt = crypto.Salt;
                    vm.UserModel.Password = encrypPass;
                    vm.UserModel.SecretQuestion = request.ActivationModel1.SecretQuestion;
                    vm.UserModel.SecretAnswer = request.ActivationModel1.SecretAnswer;
                    vm.UserModel.Status = true;
                    vm.UserModel.Deleted = false;
                    vm.UserModel.UpdatedAt = DateTime.Now;

                    vm.UserModel.Student.Birthday = request.ActivationModel1.Birthday;
                    vm.UserAddressModel = db.UserAddresses.Create();
                    vm.UserAddressModel.UserId = vm.UserModel.UserId;
                    vm.UserAddressModel.ZipCode = request.ActivationModel1.ZipCode;
                    vm.UserAddressModel.Address1 = request.ActivationModel1.Address1;
                    vm.UserAddressModel.Address2 = request.ActivationModel1.Address2;
                    vm.UserAddressModel.City = request.ActivationModel1.City;
                    vm.UserAddressModel.Country = request.ActivationModel1.Country;
                    vm.UserAddressModel.CreatedAt = DateTime.Now;
                    db.UserAddresses.Add(vm.UserAddressModel);
                    db.Entry(vm.UserModel).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            return View();
        }
    }
}