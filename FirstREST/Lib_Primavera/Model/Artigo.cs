using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {
        public string CodArtigo
        {
            get;
            set;
        }

        public string DescArtigo
        {
            get;
            set;
        }

        public string CodBArtigo
        {
            get;
            set;
        }

        public string MarcaArtigo
        {
            get;
            set;
        }

        public string ModeloArtigo
        {
            get;
            set;
        }

        public bool PermDevArtigo
        {
            get;
            set;
        }

        public double PesoArtigo
        {
            get;
            set;
        }

        public double PesoLArtigo
        {
            get;
            set;
        }

        public double STKActualArtigo
        {
            get;
            set;
        }

        public string IvaArtigo
        {
            get;
            set;
        }

        public string ObsArtigo
        {
            get;
            set;
        }

        public List<Armazens> armArtigo
        {
            get;
            set;
        }

        public double precoArtigo
        {
            get;
            set;
        }

        public double precomIvaArtigo
        {
            get;
            set;
        }
    }
}