using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public class RetStatusServico
    {
        public int tpAmb { get; set; }
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public int cUF { get; set; }
        public DateTime dhRecbto { get; set; }
        public int tMed { get; set; }
        public DateTime dhRetorno { get; set; }
    }
}
