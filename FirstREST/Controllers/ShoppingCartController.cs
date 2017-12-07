using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddtoCart(FirstREST.Models.ShoppingCart item){

            item.IdUser = Int32.Parse(Request.Cookies["UserID"].Value.ToString());

            FirstREST.Lib_Primavera.Model.Artigo art = FirstREST.Lib_Primavera.PriIntegration.GetArtigo(item.CodArtigo);
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.ShoppingCarts.Where(a => a.IdUser.Equals(item.IdUser) && a.ArmazemArtigo.Equals(item.ArmazemArtigo) && a.CodArtigo.Equals(item.CodArtigo)).FirstOrDefault();
                    if (obj == null)
                     {
                         var new_item = new FirstREST.Models.ShoppingCart { IdUser = item.IdUser, CodArtigo = item.CodArtigo, DescArtigo = item.DescArtigo, ArmazemArtigo = item.ArmazemArtigo, QuantidadeArtigo = item.QuantidadeArtigo, PrecoArtigo = item.PrecoArtigo };
                         db.ShoppingCarts.Add(new_item);
                     }
                     else
                     {
                         obj.QuantidadeArtigo = obj.QuantidadeArtigo + item.QuantidadeArtigo;

                     }
                        db.SaveChanges();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddDefaultToCart(FirstREST.Lib_Primavera.Model.Artigo artigo)
        {
            FirstREST.Lib_Primavera.Model.Artigo art = FirstREST.Lib_Primavera.PriIntegration.GetArtigo(artigo.CodArtigo);

            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    int userId = Int32.Parse(Request.Cookies["UserID"].Value.ToString());

                    var obj = db.ShoppingCarts.Where(a => a.IdUser.Equals(userId) && a.ArmazemArtigo.Equals(art.armSugestaoArtigo.descArmazens) && a.CodArtigo.Equals(artigo.CodArtigo)).FirstOrDefault();
                    if (obj == null)
                    {
                        var new_item = new FirstREST.Models.ShoppingCart { IdUser = userId, CodArtigo = artigo.CodArtigo, DescArtigo = art.DescArtigo, ArmazemArtigo = art.armSugestaoArtigo.descArmazens, QuantidadeArtigo = 1, PrecoArtigo = art.precomIvaArtigo };
                        db.ShoppingCarts.Add(new_item);
                    }
                    else
                    {
                        int updatedQuantity = obj.QuantidadeArtigo + 1;
                        obj.QuantidadeArtigo = updatedQuantity;

                    }
                    db.SaveChanges();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult UpdateCart(FirstREST.Models.ShoppingCart item)
        {
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.ShoppingCarts.Where(a => a.Id.Equals(item.Id)).FirstOrDefault();

                    //if(obj == null)

                    if (obj.ArmazemArtigo != item.ArmazemArtigo)
                    {
                        var new_obj = db.ShoppingCarts.Where(a => a.IdUser.Equals(obj.IdUser) && a.ArmazemArtigo.Equals(item.ArmazemArtigo) && a.CodArtigo.Equals(obj.CodArtigo)).FirstOrDefault();

                        if (new_obj != null)
                        {
                            new_obj.QuantidadeArtigo = new_obj.QuantidadeArtigo + item.QuantidadeArtigo;
                        }
                        else
                        {
                            new_obj = item;
                        }
                        AddtoCart(new_obj);
                        db.ShoppingCarts.Remove(obj);
                    }
                    else
                    {
                        obj.QuantidadeArtigo = item.QuantidadeArtigo;
                    }

                    db.SaveChanges();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());

        }

        [HttpPost]
        public ActionResult RemoveFromCart(FirstREST.Models.ShoppingCart item)
        {
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.ShoppingCarts.Where(a => a.Id.Equals(item.Id)).FirstOrDefault();
                    db.ShoppingCarts.Remove(obj);

                    db.SaveChanges();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());

        }

    }
}
