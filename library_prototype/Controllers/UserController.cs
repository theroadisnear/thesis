using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace library_prototype.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        [AllowAnonymous]
        public ActionResult UserProfile()
        {
            return View();
        }

    }
}
