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
            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("Client");

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
                        Session["UserID"] = obj.Id.ToString();
                        Session["UserName"] = obj.Username.ToString();
                        Session["Client"] = obj.Client_Name.ToString();
                        ViewBag.errorMessage = null;

                        string url = "http://localhost:49822/home/";
                        return Redirect(url);
                    }
                    else
                    {
                        ViewBag.errorMessage = "Wrong Username or Password. Try again.";

                        return View();
                    }
                }
            }
            return View(objUser);
        }

    }
}
