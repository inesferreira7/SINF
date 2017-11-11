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
    public class ArtigosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.artigos = Lib_Primavera.PriIntegration.ListaArtigos();
            return View();
        }

        public ActionResult ArtigoPage(string id)
        {
            ViewBag.artigo = Lib_Primavera.PriIntegration.GetArtigo(id);
            return View();
        }

        public ActionResult ProcuraArtigo(string id)
        {
            ViewBag.procura_artigo = Lib_Primavera.PriIntegration.ProcuraArtigos(id);
            return View();
        }
    }
}

