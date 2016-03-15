using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public sealed class RetInutNFe
    {
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public DateTime dhRecbto { get; set; }
        public string nProt { get; set; }
        public string prot_Inutilizacao { get; set; }
    }
}
