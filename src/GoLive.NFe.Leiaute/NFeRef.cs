using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    public sealed class NFeRef
    {
        public int NFref_tp { get; set; }

        #region "NFe Referenciada"
        public string NFref_refNFe { get; set; }
        public string NFref_refCTe { get; set; }
        #endregion

        #region "NF modelo 1 referenciada"
        public string NFref_reNF_cUF { get; set; }
        public string NFref_reNF_AAMM { get; set; }
        public string NFref_reNF_CNPJ { get; set; }
        public string NFref_reNF_mod { get; set; }
        public int NFref_reNF_serie { get; set; }
        public string NFref_reNF_nNF { get; set; }
        #endregion

        #region "NF de produtor rural"
        public string NFref_refNFP_cUF { get; set; }
        public string NFref_refNFP_AAMM { get; set; }
        public string NFref_refNFP_CNPJ { get; set; }
        public string NFref_refNFP_CPF { get; set; }
        public string NFref_refNFP_IE { get; set; }
        public string NFref_refNFP_mod { get; set; }
        public int NFref_refNFP_serie { get; set; }
        public string NFref_refNFP_nNF { get; set; }
        #endregion

        #region "NF de cupom fiscal"

        public string NFref_refECF_mod { get; set; }
        public string NFref_refECF_nECF { get; set; }
        public string NFref_refECF_nCOO { get; set; }
        #endregion
    }
}
