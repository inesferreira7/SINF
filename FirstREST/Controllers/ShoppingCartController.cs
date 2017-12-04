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
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                     var obj = db.ShoppingCarts.Where(a => a.IdUser.Equals(item.IdUser) && a.CodArtigo.Equals(item.CodArtigo)).FirstOrDefault();
                     if (obj == null)
                     {
                         var new_item = new FirstREST.Models.ShoppingCart { IdUser = item.IdUser, CodArtigo = item.CodArtigo, DescArtigo = item.DescArtigo, ArmazemArtigo = item.ArmazemArtigo, QuantidadeArtigo = item.QuantidadeArtigo, PrecoArtigo = item.PrecoArtigo };
                         db.ShoppingCarts.Add(new_item);
                     }
                     else
                     {
                         int updatedQuantity = obj.QuantidadeArtigo + item.QuantidadeArtigo;
                         obj.QuantidadeArtigo = updatedQuantity;

                     }
                        db.SaveChanges();
                }
            }

             return Redirect("http://localhost:49822/home/");
        }

    }
}
