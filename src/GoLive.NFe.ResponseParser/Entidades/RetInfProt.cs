using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public sealed class RetInfProt
    {
        public string prot_chNFe { get; set; }
        public DateTime prot_dhRecbto { get; set; }
        public string prot_nProt { get; set; }
        public int prot_cStat { get; set; }
        public string prot_digVal { get; set; }
        public string prot_xMotivo { get; set; }
        public string prot_autorizacao { get;  set; }
    }
}
