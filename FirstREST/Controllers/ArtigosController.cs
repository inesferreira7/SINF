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
            ViewBag.related = Lib_Primavera.PriIntegration.ListaArtigosPorCategoria(ViewBag.artigo.catArtigo);
            return View();
        }
    }

    //  [RoutePrefix("api/test")]
    public class BooksController : ApiController
    {
        // GET: /api/books
        //[HttpGet]
        //[AttributeRouting.Web.Mvc.Route("api/test")]
        public IEnumerable<Lib_Primavera.Model.Artigo> Get()
        {
            return Lib_Primavera.PriIntegration.ListaArtigos();
        }

        // GET: /api/books/{id}
        //[AttributeRouting.Web.Mvc.Route]
        public Lib_Primavera.Model.Artigo Get(string id)
        {
            Lib_Primavera.Model.Artigo artigo = Lib_Primavera.PriIntegration.GetArtigo(id);
            if (artigo == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return artigo;
            }
        }

    }
}

