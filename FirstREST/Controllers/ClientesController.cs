﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ClientesController : Controller
    {
        public ActionResult PaginaCliente(string id)
        {
            if (Request.Cookies["Client"] == null)
                return Redirect("http://localhost:49822/account/login/");

            if (Request.Cookies["Client"].Value.ToString() != id)
            {
                string url = "http://localhost:49822/clientes/paginacliente/" + Request.Cookies["Client"].Value.ToString() + "/";
                return Redirect(url);
            }
            if (Request.Cookies["error"] != null)
            {
                ViewBag.error = Request.Cookies["error"].Value.ToString();
                Response.Cookies["error"].Expires = DateTime.Now.AddDays(-1);
            }
            else
                ViewBag.error = "ok";

            ViewBag.cliente = Lib_Primavera.PriIntegration.GetCliente(id);
            ViewBag.pendentes = Lib_Primavera.PriIntegration.get_Orders_pending(id);
            ViewBag.transferidas = Lib_Primavera.PriIntegration.get_Orders_shipped(id);
            
            return View();
        }
        public ActionResult getClientes()
        {
            ViewBag.clientes = Lib_Primavera.PriIntegration.ListaClientes();
            return View();
        }

        public ActionResult getCliente(string id)
        {
            ViewBag.cliente = Lib_Primavera.PriIntegration.GetCliente(id);
            return View();
        }


        [ValidateAntiForgeryToken]
        public ActionResult AtualizaCliente(FormCollection form)
        {
            string client = Request.Cookies["Client"].Value.ToString();
            string old_pw = form["old-password"];
            string new_pw = form["new-password"];

            if(new_pw != "" && old_pw != "")
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var obj = db.Users.Where(a => a.Client_Name.Equals(client)).FirstOrDefault();
                    if (obj.Password != old_pw)
                    {
                        Response.Cookies["error"].Value = "A palavra passe antiga está incorreta";
                        return Redirect("http://localhost:49822/clientes/paginacliente/" + client + "/");
                    }
                    else
                    {
                        obj.Password = new_pw;
                        db.SaveChanges();
                    }

                }
            }

            Lib_Primavera.Model.Cliente temp_cli = Lib_Primavera.PriIntegration.GetCliente(client);

            if (form["telefone"] != "")
                temp_cli.Telefone = form["telefone"];

            if (form["morada"] != "")
                temp_cli.Morada = form["morada"];

            if (form["email"] != "")
                temp_cli.Email = form["email"];

            Lib_Primavera.PriIntegration.UpdCliente(temp_cli);
            return Redirect("http://localhost:49822/clientes/paginacliente/" + client + "/");
        }

        public ActionResult displayShoppingCart(string id)
        {
            if (Request.Cookies["Client"] == null)
                return Redirect("http://localhost:49822/account/login/");

            if (Request.Cookies["UserID"].Value.ToString() != id)
            {
                string url = "http://localhost:49822/clientes/paginacliente/" + Request.Cookies["Client"].Value.ToString() + "/";
                return Redirect(url);
            }

                if (ModelState.IsValid)
                {
                    using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                    {
                        List<FirstREST.Models.ShoppingCart> list = new List<Models.ShoppingCart>();
                        List<Artigo> list_artigos = new List<Artigo>();
                        int parseId = Int32.Parse(id);
                        var obj = db.ShoppingCarts.Where(a => a.IdUser.Equals(parseId)).ToList();

                        for (int i = 0; i < obj.Count; i++)
                        {
                            list_artigos.Add(Lib_Primavera.PriIntegration.GetArtigo(obj[i].CodArtigo));
                            list.Add(obj[i]);
                        }
                        ViewBag.shoppingCart = list;
                        ViewBag.idUser = id;
                        ViewBag.artigos = list_artigos;
                        ViewBag.scFinal = getShoppingCart(parseId);
                    }
                }
                return View();
        }

        public List<FirstREST.Models.ShoppingCart> getShoppingCart(int idUser)
        {
            if (ModelState.IsValid)
            {
                using (FirstREST.Models.online_storeEntities db = new FirstREST.Models.online_storeEntities())
                {
                    var ret = db.ShoppingCarts.Where(a => a.IdUser.Equals(idUser)).ToList();
                    return ret;
                }
            }

            return null;
        }
    }

    public class ClientsController : ApiController
    {
        //
        // GET: /Clientes/

        public IEnumerable<Lib_Primavera.Model.Cliente> Get()
        {
            return Lib_Primavera.PriIntegration.ListaClientes();
        }


        // GET api/cliente/5    
        public Cliente Get(string id)
        {
            Lib_Primavera.Model.Cliente cliente = Lib_Primavera.PriIntegration.GetCliente(id);
            if (cliente == null)
            {
                throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound));

            }
            else
            {
                return cliente;
            }
        }


        public HttpResponseMessage Post(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereClienteObj(cliente);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(
                   HttpStatusCode.Created, cliente);
                string uri = Url.Link("DefaultApi", new { CodCliente = cliente.CodCliente });
                response.Headers.Location = new Uri(uri);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }


        public HttpResponseMessage Put(string id, Lib_Primavera.Model.Cliente cliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {
                erro = Lib_Primavera.PriIntegration.UpdCliente(cliente);
                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }
            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }



        public HttpResponseMessage Delete(string id)
        {


            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {

                erro = Lib_Primavera.PriIntegration.DelCliente(id);

                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }

            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);

            }

        }
    }   

    
        

    }




