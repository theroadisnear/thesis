using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using library_prototype.Models;

namespace library_prototype.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetProfilePicture(HttpPostedFileBase img, MultipleModel.ProfileVM prof)
        {
            return View();
        }

    }
}
