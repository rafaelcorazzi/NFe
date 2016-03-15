using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.ResponseParser.Entidades
{
    public sealed class RetConsCad
    {
        public int cStat { get; set; }
        public string infCad_IE { get; set; }
        public string infCad_CNPJ { get; set; }
        //1 Habilitado ; 2 Nao Habilitado
        public int infCad_cSit { get; set; }
        public string infCad_xNome { get; set; }
        public int infCad_indCredNFe { get; set; }
        public int infCad_indCredCTe { get; set; }
        public string infCad_xRegApur { get; set; }
        public string infCad_CNAE { get; set; }
        public DateTime infCad_dIniAtiv { get; set; }
        public DateTime infCad_dUltSit { get; set; }
        public string infCad_xLgr { get; set; }
        public string infCad_nro { get; set; }
        public string infCad_xBairro { get; set; }
        public string infCad_cMun { get; set; }
        public string infCad_xMun { get; set; }
        public string infCad_UF { get; set; }
        public string infCad_CEP { get; set; }
    }
}
