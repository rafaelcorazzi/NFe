using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public sealed class RetEnvNFe
    {
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public int cUF { get; set; }
        public DateTime dhRecbto { get; set; }
        public string rec_nRec { get; set; }
        public int rec_tMed { get; set; }
    }
}
