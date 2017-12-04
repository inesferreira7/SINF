using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
           Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
           Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
           Response.Cookies["Client"].Expires = DateTime.Now.AddDays(-1);
           if (Request.Cookies["errorMessage"] != null)
           {
               Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);
           }  

            string url = "http://localhost:49822/home/";
            return Redirect(url);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FirstREST.Models.User objUser)
        {
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.Users.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Response.Cookies["UserID"].Value = obj.Id.ToString();
                        Response.Cookies["Username"].Value = obj.Username.ToString();
                        Response.Cookies["Client"].Value = obj.Client_Name.ToString();
                        ViewBag.errorMessage = null;

                        string url = "http://localhost:49822/home/";
                        return Redirect(url);
                    }
                    else
                    {
                        Response.Cookies["errorMessage"].Value = "Wrong Username or Password. Try again.";

                        return View();
                    }
                }
            }
            return View(objUser);
        }

    }
}
