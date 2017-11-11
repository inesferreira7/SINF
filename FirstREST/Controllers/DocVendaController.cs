using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;


namespace FirstREST.Controllers
{
    public class DocVendaController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.encomendas = Lib_Primavera.PriIntegration.Encomendas_List();
            return View();
        }
    }
}
