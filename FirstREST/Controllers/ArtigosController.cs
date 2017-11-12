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
            ViewBag.related = Lib_Primavera.PriIntegration.ListaArtigosPorCategoria(ViewBag.artigo.tipoArtigo);
            return View();
        }
    }
}

