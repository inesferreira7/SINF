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
            if (Request.Cookies["UserID"] != null)
                return Redirect("http://localhost:49822/home/");

            return View();
        }

        public ActionResult Register()
        {
            if (Request.Cookies["UserID"] != null)
                return Redirect("http://localhost:49822/home/");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection form)
        {
            string full_name = form["full_name"];
            string abrev = form["abrev"];
            string username = form["username"];
            string address = form["address"];
            string password = form["password"];
            string nif = form["nif"];
            string phone = form["phone"];
            string email = form["email"];

            FirstREST.Lib_Primavera.Model.Cliente new_cli = new Lib_Primavera.Model.Cliente();
            new_cli.CodCliente = abrev;
            new_cli.NomeCliente = full_name;
            new_cli.Morada = address;
            new_cli.NumContribuinte = nif;
            new_cli.Telefone = phone;
            new_cli.Email = email;
            new_cli.Moeda = "EUR";
 

            FirstREST.Models.User new_user = new Models.User { Username = username, Password = password, Client_Name = abrev };
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.Users.Where(a => a.Username.Equals(username) || a.Client_Name.Equals(abrev)).FirstOrDefault();
                    if (obj != null)
                    {
                        ViewBag.errorMessage = "Nome de utilizador ou nome abreviado já existentes";
                        System.Diagnostics.Debug.WriteLine("EXISTENTE");
                        return Register();
                    }
                    FirstREST.Lib_Primavera.PriIntegration.InsereClienteObj(new_cli);
                    db.Users.Add(new_user);

                    db.SaveChanges();
                }
            }
            return Login(new_user);
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["UserID"] == null)
                return Redirect("http://localhost:49822/account/login");

           Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
           Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
           Response.Cookies["Client"].Expires = DateTime.Now.AddDays(-1);

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
                        ViewBag.errorMessage = "Wrong Username or Password. Try again.";

                        return View();
                    }
                }
            }
            return View(objUser);
        }

    }
}
