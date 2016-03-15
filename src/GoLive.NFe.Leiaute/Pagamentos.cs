using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    public sealed class Pagamentos
    {
        public string pag_tPag { get; set; }
        public decimal pag_vPag { get; set; }
        public string pag_card_CNPJ { get; set; }
        public string pag_card_tBand { get; set; }
        public string pag_card_cAut { get; set; }
    }
}
