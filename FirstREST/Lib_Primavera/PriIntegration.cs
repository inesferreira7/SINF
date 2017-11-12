using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS900;
using Interop.StdPlatBS900;
using Interop.StdBE900;
using Interop.GcpBE900;
using ADODB;

namespace FirstREST.Lib_Primavera
{
    public class PriIntegration
    {
        

        # region Cliente

        public static List<Model.Cliente> ListaClientes()
        {
            
            
            StdBELista objList;

            List<Model.Cliente> listClientes = new List<Model.Cliente>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Clientes.LstClientes();

                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Moeda, NumContrib as NumContribuinte, Fac_Mor AS campo_exemplo FROM  CLIENTES");

                
                while (!objList.NoFim())
                {
                    listClientes.Add(new Model.Cliente
                    {
                        CodCliente = objList.Valor("Cliente"),
                        NomeCliente = objList.Valor("Nome"),
                        Moeda = objList.Valor("Moeda"),
                        NumContribuinte = objList.Valor("NumContribuinte"),
                        Morada = objList.Valor("campo_exemplo")
                    });
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {
            

            GcpBECliente objCli = new GcpBECliente();


            Model.Cliente myCli = new Model.Cliente();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Edita(codCliente);
                    myCli.CodCliente = objCli.get_Cliente();
                    myCli.NomeCliente = objCli.get_Nome();
                    myCli.Moeda = objCli.get_Moeda();
                    myCli.NumContribuinte = objCli.get_NumContribuinte();
                    myCli.Morada = objCli.get_Morada();
                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
           

            GcpBECliente objCli = new GcpBECliente();

            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(cliente.CodCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(cliente.CodCliente);
                        objCli.set_EmModoEdicao(true);

                        objCli.set_Nome(cliente.NomeCliente);
                        objCli.set_NumContribuinte(cliente.NumContribuinte);
                        objCli.set_Moeda(cliente.Moeda);
                        objCli.set_Morada(cliente.Morada);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }



        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBECliente myCli = new GcpBECliente();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {

                    myCli.set_Cliente(cli.CodCliente);
                    myCli.set_Nome(cli.NomeCliente);
                    myCli.set_NumContribuinte(cli.NumContribuinte);
                    myCli.set_Moeda(cli.Moeda);
                    myCli.set_Morada(cli.Morada);

                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

       

        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------


        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {
            StdBELista armList;
            StdBELista descArmList;
            StdBELista precoList;

            Model.Armazens arm = new Model.Armazens();

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Edita(codArtigo);
                    myArt = new Model.Artigo();
                    myArt.CodArtigo = objArtigo.get_Artigo();
                    myArt.DescArtigo = objArtigo.get_Descricao();
                    myArt.CodBArtigo = objArtigo.get_CodBarras();
                    myArt.MarcaArtigo = objArtigo.get_Marca();
                    myArt.ModeloArtigo = objArtigo.get_Modelo();
                    myArt.PermDevArtigo = objArtigo.get_SujeitoDevolucao();
                    myArt.PesoArtigo = objArtigo.get_Peso();
                    myArt.PesoLArtigo = objArtigo.get_PesoLiquido();
                    myArt.STKActualArtigo = objArtigo.get_StkActual();
                    myArt.IvaArtigo = objArtigo.get_IVA();
                    myArt.ObsArtigo = objArtigo.get_Observacoes();
                    myArt.tipoArtigo = objArtigo.get_TipoArtigo();
                    myArt.armArtigo = new List<Model.Armazens>();

                    string queryArmazem = "SELECT DISTINCT Armazem, MAX(StkActual) AS StkActual FROM ArtigoArmazem WHERE ArtigoArmazem.Artigo = '" + myArt.CodArtigo + "' GROUP BY Armazem";

                    armList = PriEngine.Engine.Consulta(queryArmazem);

                    List<Model.Armazens> listArms = new List<Model.Armazens>();

                    while (!armList.NoFim())
                    {
                        arm = new Model.Armazens();
                        arm.idArmazens = armList.Valor("Armazem");
                        arm.StkArmazens = armList.Valor("StkActual");

                        string queryDescArm = "SELECT Descricao FROM Armazens WHERE Armazem = '" + arm.idArmazens + "'";
                        descArmList = PriEngine.Engine.Consulta(queryDescArm);

                        arm.descArmazens = descArmList.Valor("Descricao");

                        listArms.Add(arm);

                        armList.Seguinte();
                    }

                    myArt.armArtigo = listArms;


                    string queryPreco = "SELECT PVP1 FROM ArtigoMoeda WHERE Artigo = '" + myArt.CodArtigo + "'";
                    precoList = PriEngine.Engine.Consulta(queryPreco);

                    myArt.precoArtigo = precoList.Valor("PVP1");
                    myArt.precomIvaArtigo = myArt.precoArtigo + myArt.precoArtigo * Double.Parse(myArt.IvaArtigo) * 0.01;

                    return myArt;
                }
                
            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> ListaArtigos()
        {
                        
            StdBELista objList;
            StdBELista armList;
            StdBELista precoList;
            StdBELista descArmList;

            Model.Artigo art = new Model.Artigo();
            Model.Armazens arm = new Model.Armazens();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                objList = PriEngine.Engine.Consulta("SELECT Artigo, CodBarras, Descricao, Marca, Modelo, PermiteDevolucao, Peso, PesoLiquido, STKActual, Iva, Observacoes FROM Artigo");

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.DescArtigo = objList.Valor("Descricao");
                    art.CodBArtigo = objList.Valor("CodBarras");
                    art.MarcaArtigo = objList.Valor("Marca");
                    art.ModeloArtigo = objList.Valor("Modelo");
                    art.PermDevArtigo = objList.Valor("PermiteDevolucao");
                    art.PesoArtigo = objList.Valor("Peso");
                    art.PesoLArtigo = objList.Valor("PesoLiquido");
                    art.STKActualArtigo = objList.Valor("STKActual");
                    art.IvaArtigo = objList.Valor("Iva");
                    art.ObsArtigo = objList.Valor("Observacoes");
                    art.armArtigo = new List<Model.Armazens>();

                    string queryArmazem = "SELECT Armazem, MAX(StkActual) AS StkActual FROM ArtigoArmazem WHERE ArtigoArmazem.Artigo = '" + art.CodArtigo + "' GROUP BY Armazem";
                    
                    armList = PriEngine.Engine.Consulta(queryArmazem);

                    List<Model.Armazens> listArms = new List<Model.Armazens>();

                    while (!armList.NoFim())
                    {
                        arm = new Model.Armazens();
                        arm.idArmazens = armList.Valor("Armazem");
                        arm.StkArmazens = armList.Valor("StkActual");

                        string queryDescArm = "SELECT Descricao FROM Armazens WHERE Armazem = '" + arm.idArmazens +"'";
                        descArmList = PriEngine.Engine.Consulta(queryDescArm);

                        arm.descArmazens = descArmList.Valor("Descricao");
                        
                        listArms.Add(arm);

                        armList.Seguinte();
                    }

                    art.armArtigo = listArms;


                    string queryPreco = "SELECT PVP1 FROM ArtigoMoeda WHERE Artigo = '" + art.CodArtigo + "'";
                    precoList = PriEngine.Engine.Consulta(queryPreco);

                    art.precoArtigo = precoList.Valor("PVP1");
                    art.precomIvaArtigo = art.precoArtigo + art.precoArtigo * Double.Parse(art.IvaArtigo) * 0.01;

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static List<Model.Artigo> ListaArtigosPorSTK()
        {

            StdBELista objList;
            StdBELista armList;
            StdBELista precoList;
            StdBELista descArmList;

            Model.Artigo art = new Model.Artigo();
            Model.Armazens arm = new Model.Armazens();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                objList = PriEngine.Engine.Consulta("SELECT Artigo, CodBarras, Descricao, Marca, Modelo, PermiteDevolucao, Peso, PesoLiquido, STKActual, Iva, Observacoes FROM Artigo ORDER BY STKActual DESC");

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.DescArtigo = objList.Valor("Descricao");
                    art.CodBArtigo = objList.Valor("CodBarras");
                    art.MarcaArtigo = objList.Valor("Marca");
                    art.ModeloArtigo = objList.Valor("Modelo");
                    art.PermDevArtigo = objList.Valor("PermiteDevolucao");
                    art.PesoArtigo = objList.Valor("Peso");
                    art.PesoLArtigo = objList.Valor("PesoLiquido");
                    art.STKActualArtigo = objList.Valor("STKActual");
                    art.IvaArtigo = objList.Valor("Iva");
                    art.ObsArtigo = objList.Valor("Observacoes");
                    art.armArtigo = new List<Model.Armazens>();

                    string queryArmazem = "SELECT Armazem, MAX(StkActual) AS StkActual FROM ArtigoArmazem WHERE ArtigoArmazem.Artigo = '" + art.CodArtigo + "' GROUP BY Armazem";

                    armList = PriEngine.Engine.Consulta(queryArmazem);

                    List<Model.Armazens> listArms = new List<Model.Armazens>();

                    while (!armList.NoFim())
                    {
                        arm = new Model.Armazens();
                        arm.idArmazens = armList.Valor("Armazem");
                        arm.StkArmazens = armList.Valor("StkActual");

                        string queryDescArm = "SELECT Descricao FROM Armazens WHERE Armazem = '" + arm.idArmazens + "'";
                        descArmList = PriEngine.Engine.Consulta(queryDescArm);

                        arm.descArmazens = descArmList.Valor("Descricao");

                        listArms.Add(arm);

                        armList.Seguinte();
                    }

                    art.armArtigo = listArms;


                    string queryPreco = "SELECT PVP1 FROM ArtigoMoeda WHERE Artigo = '" + art.CodArtigo + "'";
                    precoList = PriEngine.Engine.Consulta(queryPreco);

                    art.precoArtigo = precoList.Valor("PVP1");
                    art.precomIvaArtigo = art.precoArtigo + art.precoArtigo * Double.Parse(art.IvaArtigo) * 0.01;

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static List<Model.Artigo> ProcuraArtigos(string procura)
        {

            StdBELista objList;
            StdBELista armList;
            StdBELista precoList;
            StdBELista descArmList;

            Model.Artigo art = new Model.Artigo();
            Model.Armazens arm = new Model.Armazens();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                //objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                objList = PriEngine.Engine.Consulta("SELECT Artigo, CodBarras, Descricao, Marca, Modelo, PermiteDevolucao, Peso, PesoLiquido, STKActual, Iva, Observacoes FROM Artigo");

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.DescArtigo = objList.Valor("Descricao");
                    art.CodBArtigo = objList.Valor("CodBarras");
                    art.MarcaArtigo = objList.Valor("Marca");
                    art.ModeloArtigo = objList.Valor("Modelo");
                    art.PermDevArtigo = objList.Valor("PermiteDevolucao");
                    art.PesoArtigo = objList.Valor("Peso");
                    art.PesoLArtigo = objList.Valor("PesoLiquido");
                    art.STKActualArtigo = objList.Valor("STKActual");
                    art.IvaArtigo = objList.Valor("Iva");
                    art.ObsArtigo = objList.Valor("Observacoes");
                    art.armArtigo = new List<Model.Armazens>();

                    if (art.DescArtigo.Contains(procura))
                    {

                        string queryArmazem = "SELECT Armazem, MAX(StkActual) AS StkActual FROM ArtigoArmazem WHERE ArtigoArmazem.Artigo = '" + art.CodArtigo + "' GROUP BY Armazem";

                        armList = PriEngine.Engine.Consulta(queryArmazem);

                        List<Model.Armazens> listArms = new List<Model.Armazens>();

                        while (!armList.NoFim())
                        {
                            arm = new Model.Armazens();
                            arm.idArmazens = armList.Valor("Armazem");
                            arm.StkArmazens = armList.Valor("StkActual");

                            string queryDescArm = "SELECT Descricao FROM Armazens WHERE Armazem = '" + arm.idArmazens + "'";
                            descArmList = PriEngine.Engine.Consulta(queryDescArm);

                            arm.descArmazens = descArmList.Valor("Descricao");

                            listArms.Add(arm);

                            armList.Seguinte();
                        }

                        art.armArtigo = listArms;


                        string queryPreco = "SELECT PVP1 FROM ArtigoMoeda WHERE Artigo = '" + art.CodArtigo + "'";
                        precoList = PriEngine.Engine.Consulta(queryPreco);

                        art.precoArtigo = precoList.Valor("PVP1");
                        art.precomIvaArtigo = art.precoArtigo + art.precoArtigo * Double.Parse(art.IvaArtigo) * 0.01;

                        listArts.Add(art);
                    }
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }

        public static List<Tuple<string, string>> ListCategories()
        {
            StdBELista catList;
            List<Tuple<string,string>> categories = new List<Tuple<string,string>>();
            catList = PriEngine.Engine.Consulta("SELECT TipoArtigo, Descricao from TiposArtigo");
            
            while (!catList.NoFim())
            {
                string cat = catList.Valor("Descricao");
                string id = catList.Valor("TipoArtigo");
                categories.Add(new Tuple<string,string>(id,cat));
                catList.Seguinte();
            }

            return categories;
             
        }

        public static List<Model.Artigo> ListaArtigosPorCategoria(string categoria)
        {
            StdBELista objList;
            StdBELista precoList;
            Model.Artigo art = new Model.Artigo();
            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {

                objList = PriEngine.Engine.Consulta("SELECT Artigo, CodBarras, Descricao, Marca, Modelo, STKActual, Iva, Observacoes FROM Artigo where TipoArtigo = '" + categoria + "'");

                while (!objList.NoFim())
                {
                    art = new Model.Artigo();
                    art.CodArtigo = objList.Valor("Artigo");
                    art.DescArtigo = objList.Valor("Descricao");
                    art.CodBArtigo = objList.Valor("CodBarras");
                    art.MarcaArtigo = objList.Valor("Marca");
                    art.ModeloArtigo = objList.Valor("Modelo");
                    art.STKActualArtigo = objList.Valor("STKActual");
                    art.ObsArtigo = objList.Valor("Observacoes");
                    art.IvaArtigo = objList.Valor("Iva");

                    string queryPreco = "SELECT PVP1 FROM ArtigoMoeda WHERE Artigo = '" + art.CodArtigo + "'";
                    precoList = PriEngine.Engine.Consulta(queryPreco);

                    art.precoArtigo = precoList.Valor("PVP1");
                    art.precomIvaArtigo = art.precoArtigo + art.precoArtigo * Double.Parse(art.IvaArtigo) * 0.01;

                    listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;
            }
            else return null;
            
        }

        #endregion Artigo


        #region DocCompra

        public static Model.DocCompra GetDocCompra(string id)
        {
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                string query = "SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where id = '" + id + "'";
                objListCab = PriEngine.Engine.Consulta(query);
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");


                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    objListCab.Seguinte();
                }
            }
            return dc;
        }

        public static List<Model.DocCompra> VGR_List()
        {
                
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where TipoDoc='VGR'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }

                
        public static Model.RespostaErro VGR_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBEDocumentoCompra myGR = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindv = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myGR.set_Entidade(dc.Entidade);
                    myGR.set_NumDocExterno(dc.NumDocExterno);
                    myGR.set_Serie(dc.Serie);
                    myGR.set_Tipodoc("VGR");
                    myGR.set_TipoEntidade("F");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dc.LinhasDoc;
                    //PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR,rl);
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR);
                    foreach (Model.LinhaDocCompra lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myGR, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }


                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myGR, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }


        #endregion DocCompra


        #region DocsVenda

        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();
             
            GcpBELinhaDocumentoVenda myLin = new GcpBELinhaDocumentoVenda();

            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();
             
            PreencheRelacaoVendas rl = new PreencheRelacaoVendas();
            List<Model.LinhaDocVenda> lstlindv = new List<Model.LinhaDocVenda>();
            
            try
            {
                if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myEnc.set_Entidade(dv.Entidade);
                    myEnc.set_Serie(dv.Serie);
                    myEnc.set_Tipodoc("ECL");
                    myEnc.set_TipoEntidade("C");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dv.LinhasDoc;
                    //PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, rl);
                    PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc);
                    foreach (Model.LinhaDocVenda lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Vendas.AdicionaLinha(myEnc, lin.CodArtigo, lin.Quantidade, "", "", lin.PrecoUnitario, lin.Desconto);
                    }


                   // PriEngine.Engine.Comercial.Compras.TransformaDocumento(

                    PriEngine.Engine.IniciaTransaccao();
                    //PriEngine.Engine.Comercial.Vendas.Edita Actualiza(myEnc, "Teste");
                    PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }

     

        public static List<Model.DocVenda> Encomendas_List()
        {
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            List<Model.DocVenda> listdv = new List<Model.DocVenda>();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new
            List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL'");
                while (!objListCab.NoFim())
                {
                    dv = new Model.DocVenda();
                    dv.id = objListCab.Valor("id");
                    dv.Entidade = objListCab.Valor("Entidade");
                    dv.NumDoc = objListCab.Valor("NumDoc");
                    dv.Data = objListCab.Valor("Data");
                    dv.TotalMerc = objListCab.Valor("TotalMerc");
                    dv.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                    listlindv = new List<Model.LinhaDocVenda>();

                    while (!objListLin.NoFim())
                    {
                        lindv = new Model.LinhaDocVenda();
                        lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                        lindv.CodArtigo = objListLin.Valor("Artigo");
                        lindv.DescArtigo = objListLin.Valor("Descricao");
                        lindv.Quantidade = objListLin.Valor("Quantidade");
                        lindv.Unidade = objListLin.Valor("Unidade");
                        lindv.Desconto = objListLin.Valor("Desconto1");
                        lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");

                        listlindv.Add(lindv);
                        objListLin.Seguinte();
                    }

                    dv.LinhasDoc = listlindv;
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }


       

        public static Model.DocVenda Encomenda_Get(string numdoc)
        {
            
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (PriEngine.InitializeCompany(FirstREST.Properties.Settings.Default.Company.Trim(), FirstREST.Properties.Settings.Default.User.Trim(), FirstREST.Properties.Settings.Default.Password.Trim()) == true)
            {
                

                string st = "SELECT id, Entidade, Data, NumDoc, TotalMerc, Serie From CabecDoc where TipoDoc='ECL' and NumDoc='" + numdoc + "'";
                objListCab = PriEngine.Engine.Consulta(st);
                dv = new Model.DocVenda();
                dv.id = objListCab.Valor("id");
                dv.Entidade = objListCab.Valor("Entidade");
                dv.NumDoc = objListCab.Valor("NumDoc");
                dv.Data = objListCab.Valor("Data");
                dv.TotalMerc = objListCab.Valor("TotalMerc");
                dv.Serie = objListCab.Valor("Serie");
                objListLin = PriEngine.Engine.Consulta("SELECT idCabecDoc, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido from LinhasDoc where IdCabecDoc='" + dv.id + "' order By NumLinha");
                listlindv = new List<Model.LinhaDocVenda>();

                while (!objListLin.NoFim())
                {
                    lindv = new Model.LinhaDocVenda();
                    lindv.IdCabecDoc = objListLin.Valor("idCabecDoc");
                    lindv.CodArtigo = objListLin.Valor("Artigo");
                    lindv.DescArtigo = objListLin.Valor("Descricao");
                    lindv.Quantidade = objListLin.Valor("Quantidade");
                    lindv.Unidade = objListLin.Valor("Unidade");
                    lindv.Desconto = objListLin.Valor("Desconto1");
                    lindv.PrecoUnitario = objListLin.Valor("PrecUnit");
                    lindv.TotalILiquido = objListLin.Valor("TotalILiquido");
                    lindv.TotalLiquido = objListLin.Valor("PrecoLiquido");
                    listlindv.Add(lindv);
                    objListLin.Seguinte();
                }

                dv.LinhasDoc = listlindv;
                return dv;
            }
            return null;
        }

        #endregion DocsVenda
    }
}