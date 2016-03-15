using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public sealed class RetEvento
    {
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public string chNFe { get; set; }
        public string tpEvento { get; set; }
        public string xEvento { get; set; }
        public int nSeqEvento { get; set; }
        public string CNPJDest { get; set; }
        public DateTime dhRegEvento { get; set; }
         public string nProt { get; set; }
     
        public string procEvento { get; set; }
    }
}
