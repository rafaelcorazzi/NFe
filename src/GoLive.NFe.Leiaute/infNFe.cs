using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    /// <summary>
    /// Classe das Informacoes da NF-e.</summary>
  
    public sealed class infNFe
    {

        #region "identificacao da NFe"
        /// <summary>
        /// Codigo da UF do Emitente Fiscal</summary>
        public string ide_cUF { get; set; }
        /// <summary>
        /// Codigo da NFe 8 posicoes</summary>
        public string ide_cNF { get; set; }
         /// <summary>
        /// Natureza da Operacao</summary>
        public string ide_natOp { get; set; }
        /// <summary>
        ///Indicador de Pagamento (0 - Pagamento a vista; 1 - Pagamento a prazo; 2 - Outros)</summary>
        public int ide_indPag { get; set; }
        /// <summary>
        ///Modelo Fiscal (55 - NF-e ; 65 - NFC-e)</summary>
        public int ide_mod { get; set; }
        /// <summary>
        ///Serie da Nota Fiscal</summary>
        public int ide_serie { get; set; }
        /// <summary>
        ///Numero da Nota Fiscal</summary>
        public int ide_nNF { get; set; }
        /// <summary>
        ///Data e hora de emissão do Documento Fiscal</summary>
        public DateTime ide_dhEmit { get; set; }
        public Nullable<DateTime> ide_dhSaiEnt { get; set; }
        public int ide_tpNF { get; set; }
        public int ide_idDest { get; set; }
        public string ide_cMunFG { get; set; }
        public int ide_tpImp { get; set; }
        public int ide_tpEmiss { get; set; }
        public int ide_finNFe { get; set; }
        public int ide_indFinal { get; set; }
        public int ide_indPres { get; set; }
        public int ide_procEmi { get; set; }
        public string ide_verProc { get; set; }
        public Nullable<DateTime> ide_dhCont { get; set; }
        public string ide_xJust { get; set; }
        #endregion

      

        #region "Dados do Emitente"

        public string emit_CNPJ { get; set; }
        public string emit_CPF { get; set; }
        public string emit_xNome { get; set; }
        public string emit_xFant { get; set; }
        public string emit_enderEmit_xLgr { get; set; }
        public string emit_enderEmit_nro { get; set; }
        public string emit_enderEmit_xCpl { get; set; }
        public string emit_enderEmit_xBairro { get; set; }
        public string emit_enderEmit_cMun { get; set; }
        public string emit_enderEmit_xMun { get; set; }
        public string emit_enderEmit_UF { get; set; }
        public string emit_enderEmit_CEP { get; set; }
        public string emit_enderEmit_cPais { get; set; }
        public string emit_enderEmit_xPais { get; set; }
        public string emit_fone { get; set; }
        public string emit_IE { get; set; }
        public string emit_IEST { get; set; }
        public string emit_IM { get; set; }
        public string emit_CNAE { get; set; }
        public int emit_CRT { get; set; }


        #endregion

        #region "Avulsa"

        public string avulsa_CNPJ { get; set; }
        public string avulsa_xOrgao { get; set; }
        public string avulsa_matr { get; set; }
        public string avulsa_xAgente { get; set; }
        public string avulsa_fone { get; set; }
        public string avulsa_UF { get; set; }
        public string avulsa_nDAR { get; set; }
        public Nullable<DateTime> avulsa_dEmi { get; set; }
        public decimal avulsa_vDAR { get; set; }
        public string avulsa_repEmi { get; set; }
        public Nullable<DateTime> avulsa_dPag { get; set; }
       
        #endregion

        #region "Destinatario"

        public string dest_CNPJ { get; set; }
        public string dest_CPF { get; set; }
        public string dest_idEstrangeiro { get; set; }
        public string dest_xNome { get; set; }
        public string dest_enderDest_xLgr { get; set; }
        public string dest_enderDest_nro { get; set; }
        public string dest_enderDest_xCpl { get; set; }
        public string dest_enderDest_xBairro { get; set; }
        public string dest_enderDest_cMun { get; set; }
        public string dest_enderDest_xMun { get; set; }
        public string dest_enderDest_UF { get; set; }
        public string dest_enderDest_CEP { get; set; }
        public string dest_enderDest_cPais { get; set; }
        public string dest_enderDest_xPais { get; set; }
        public string dest_fone { get; set; }
        public int dest_indIEDest { get; set; }
        public string dest_IE { get; set; }
        public string dest_ISUF { get; set; }
        public string dest_IM { get; set; }
        public string dest_email { get; set; }
        #endregion

        #region "retirada"

        public string retirada_CNPJ { get; set; }
        public string retirada_CPF { get; set; }
        public string retirada_xLgr { get; set; }
        public string retirada_nro { get; set; }
        public string retirada_xCpl { get; set; }
        public string retirada_xBairro { get; set; }
        public string retirada_cMun { get; set; }
        public string retirada_xMun { get; set; }
        public string retirada_UF { get; set; }

        #endregion

        #region "entrega"
        public string entrega_CNPJ { get; set; }
        public string entrega_CPF { get; set; }
        public string entrega_xLgr { get; set; }
        public string entrega_nro { get; set; }
        public string entrega_xCpl { get; set; }
        public string entrega_xBairro { get; set; }
        public string entrega_cMun { get; set; }
        public string entrega_xMun { get; set; }
        public string entrega_UF { get; set; }

        #endregion

        #region "total"
        public decimal total_ICMSTot_vBC { get; set; }
        public decimal total_ICMSTot_vICMS { get; set; }
        public decimal total_ICMSTot_vICMSDeson { get; set; }
        public decimal total_ICMSTot_vBCST { get; set; }
        public decimal total_ICMSTot_vST { get; set; }
        public decimal total_ICMSTot_vProd { get; set; }
        public decimal total_ICMSTot_vFrete { get; set; }
        public decimal total_ICMSTot_vSeg { get; set; }
        public decimal total_ICMSTot_vDesc { get; set; }
        public decimal total_ICMSTot_vII { get; set; }
        public decimal total_ICMSTot_vIPI { get; set; }
        public decimal total_ICMSTot_vPIS { get; set; }
        public decimal total_ICMSTot_vCOFINS { get; set; }
        public decimal total_ICMSTot_vOutro { get; set; }
        public decimal total_ICMSTot_vNF { get; set; }
        public decimal total_ICMSTot_vTotTrib { get; set; }

        #endregion

        #region "ISSQNTot"

        public decimal total_ISSQNTot_vServ { get; set; }
        public decimal total_ISSQNTot_vBC { get; set; }
        public decimal total_ISSQNTot_vISS { get; set; }
        public decimal total_ISSQNTot_vPIS { get; set; }
        public decimal total_ISSQNTot_vCOFINS { get; set; }
        public Nullable<DateTime> total_ISSQNTot_dCompet { get; set; }
        public decimal total_ISSQNTot_vDeducao { get; set; }
        public decimal total_ISSQNTot_vOutro { get; set; }
        public decimal total_ISSQNTot_vDescIncond { get; set; }
        public decimal total_ISSQNTot_vDescCond { get; set; }
        public decimal total_ISSQNTot_vISSRet { get; set; }
        public int total_ISSQNTot_cRegTrib { get; set; }

        #endregion

        #region "retTrib"
        public decimal total_retTrib_vRetPIS { get; set; }
        public decimal total_retTrib_vRetCOFINS { get; set; }
        public decimal total_retTrib_vRetCSLL { get; set; }
        public decimal total_retTrib_vBCIRRF { get; set; }
        public decimal total_retTrib_vIRRF { get; set; }
        public decimal total_retTrib_vBCRetPrev { get; set; }
        public decimal total_retTrib_vRetPrev { get; set; }
        
        #endregion

        #region "transp"

        public int transp_modFrete { get; set; }
        public string transporta_CNPJ { get; set; }
        public string transporta_CPF { get; set; }
        public string transporta_xNome { get; set; }
        public string transporta_IE { get; set; }
        public string transporta_xEnder { get; set; }
        public string transporta_xMun { get; set; }
        public string transporta_UF { get; set; }

        public decimal retTransp_vServ { get; set; }
        public decimal retTransp_vBCRet { get; set; }
        public decimal retTransp_pICMSRet { get; set; }
        public decimal retTransp_vICMSRet { get; set; }
        public string retTransp_CFOP { get; set; }
        public string retTransp_cMunFG { get; set; }

        public string veicTransp_placa { get; set; }
        public string veicTransp_UF { get; set; }
        public string veicTransp_RNTC { get; set; }

        #endregion

        #region "cobranca"
        public string cobr_nFat { get; set; }
        public decimal cobr_vOrig { get; set; }
        public decimal cobr_vDesc { get; set; }
        public decimal cobr_vLiq { get; set; }


        #endregion

        #region "informacoes Adicionais"

        public string infAdic_infAdFisco { get; set; }
        public string infAdic_infCpl { get; set; }
        #endregion

        #region "exporta"
        public string exporta_UFSaidaPais { get; set; }
        public string exporta_xLocExporta { get; set; }
        public string exporta_xLocDespacho { get; set; }
        #endregion

        #region "compra"
        public string compra_xNEmp { get; set; }
        public string compra_xPed { get; set; }
        public string compra_xCont { get; set; }

        #endregion

        public IEnumerable<NFeRef> NotaReferenciada { get; set; }

        public IEnumerable<duplicata> Duplicata { get; set; }

        public IEnumerable<volumes> Volumes { get; set; }

        public IEnumerable<reboque> reboque { get; set; }

        public IEnumerable<Pagamentos> pagamento { get; set; }
    }
}
