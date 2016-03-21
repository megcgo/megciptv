using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MegCiptvAccounts.Models
{
    public class listaDados
    {
        public string servidor { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool working { get; set; }

    }

    public class listaResult
    {
        public IList<listaDados> ListaDados { get; set; }
        public int linhasProcessadas { get; set; }

    }
}