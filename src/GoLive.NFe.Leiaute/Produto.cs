using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    public sealed class Produto
    {
        
        public string prod_cProd { get; set; }
        public string prod_cEAN { get; set; }
        public string prod_cEANTrib { get; set; }
        public string prod_xProd { get; set; }
        public string prod_NCM { get; set; }
        public string prod_NVE { get; set; }
        public string prod_EXTIPI { get; set; }
        public string prod_CFOP { get; set; }

        public string prod_uCom { get; set; }
        public decimal prod_qCom { get; set; }
        public decimal prod_vUnCom { get; set; }
        public decimal prod_vProd { get; set; }

        public string prod_uTrib { get; set; }
        public decimal prod_qTrib { get; set; }
        public decimal prod_vUnTrib { get; set;  }
        public decimal prod_vFrete { get; set; }
        public decimal prod_vSeg { get; set; }
        public decimal prod_vDesc { get; set; }
        public decimal prod_vOutro { get; set; }
        public int prod_indTot { get; set; }

        //DI E ADI
        public IEnumerable<DI> prod_DI { get; set; }
        public IEnumerable<detExport> exportacao { get; set; }
        public string prod_xPed { get; set; }
        public string prod_nItemPed { get; set; }
        public string prod_nFCI { get; set; }
       
        //medicamentos

        public string comb_cProdANP { get; set; }
        public decimal comb_pMixGN { get; set; }
        public string comb_CODIF { get; set; }
        public decimal comb_qTemp { get; set; }
        public string comb_UFCons { get; set; }
        public decimal comb_CIDE_qBCProd { get; set; }
        public decimal comb_CIDE_vAliqProd { get; set; }
        public decimal comb_CIDE_vCIDE { get; set; }
        public string prod_nRECOPI { get; set; }

        public decimal imposto_vTotTrib { get; set; }
        public int ICMS_orig { get; set; }
        public string ICMS_CST { get; set; }
        public int ICMS_modBC { get; set; }
        public decimal ICMS_vBC { get; set; }
        public decimal ICMS_pICMS { get; set; }
        public decimal ICMS_vICMS { get; set; }
        public int ICMS_modBCST { get; set; }
        public decimal ICMS_pMVAST { get; set; }
        public decimal ICMS_pRedBCST { get; set; }
        public decimal ICMS_vBCST { get; set; }
        public decimal ICMS_pICMSST { get; set; }
        public decimal ICMS_vICMSST { get; set; }
        public decimal ICMS_pRedBC { get; set; }
       
        public decimal ICMS_vICMSDeson { get; set; }
        public int ICMS_motDesICMS { get; set; }
        public decimal ICMS_vICMSOp { get; set; }
        public decimal ICMS_pDif { get; set; }
        public decimal ICMS_vICMSDif { get; set; }
        public decimal ICMS_vBCSTRet { get; set; }
        public decimal ICMS_vICMSSTRet { get; set; }
        public decimal ICMS_pBCOp { get; set; }
        public string ICMS_UFST { get; set; }
        public decimal ICMS_vBCSTDest { get; set; }
        public decimal ICMS_vICMSSTDest { get; set; }
        public string ICMS_CSOSN { get; set; }
        public decimal ICMS_pCredSN { get; set; }
        public decimal ICMS_vCredICMSSN { get; set; }

        public string IPI_clEnq { get; set; }
        public string IPI_CNPJProd { get; set; }
        public string IPI_cSelo { get; set; }
        public int IPI_qSelo { get; set; }
        public int IPI_cEnq { get; set; }

        public string IPITrib_CST { get; set; }
        public decimal IPITrib_vBC { get; set; }
        public decimal IPITrib_pIPI { get; set; }
        public decimal IPITrib_qUnid { get; set; }
        public decimal IPITrib_vUnid { get; set; }
        public decimal IPITrib_vIPI { get; set; }

        public decimal II_vBC { get; set; }
        public decimal II_vDespAdu { get; set; }
        public decimal II_vII { get; set; }
        public decimal II_vIOF { get; set; }

        public string PIS_CST { get; set; }
        public decimal PIS_vBC { get; set; }
        public decimal PIS_pPIS { get; set; }
        public decimal PIS_vPIS { get; set; }
        public decimal PIS_qBCProd { get; set; }
        public decimal PIS_vAliqProd { get; set; }
        public decimal PISST_vBC { get; set; }
        public decimal PISST_pPIS { get; set; }
        public decimal PISST_qBCProd { get; set; }
        public decimal PISST_vAliqProd { get; set; }
        public decimal PISST_vPIS { get; set; }

        public string COFINS_CST { get; set; }
        public decimal COFINS_vBC { get; set; }
        public decimal COFINS_pCOFINS { get; set; }
        public decimal COFINS_vCOFINS { get; set; }
        public decimal COFINS_qBCProd { get; set; }
        public decimal COFINS_vAliqProd { get; set; }
        public decimal COFINSST_vBC { get; set; }
        public decimal COFINSST_pCOFINS { get; set; }
        public decimal COFINSST_qBCProd { get; set; }
        public decimal COFINSST_vAliqProd { get; set; }
        public decimal COFINSST_vCOFINS { get; set; }

        public decimal ISSQN_vBC { get; set; }
        public decimal ISSQN_vAliq { get; set; }
        public decimal ISSQN_vISSQN { get; set; }
        public string ISSQN_cMunFG { get; set; }
        public string ISSQN_cListServ { get; set; }
        public decimal ISSQN_vDeducao { get; set; }
        public decimal ISSQN_vOutro { get; set; }
        public decimal ISSQN_vDescIncond { get; set; }
        public decimal ISSQN_vDescCond { get; set; }
        public decimal ISSQN_vISSRet { get; set; }
        public int ISSQN_indISS { get; set; }
        public string ISSQN_cServico { get; set; }
        public string ISSQN_cMun { get; set; }
        public string ISSQN_cPais { get; set; }
        public string ISSQN_nProcesso { get; set; }
        public int ISSQN_indIncentivo { get; set; }

        public decimal impostoDevol_pDevol { get; set; }
        public decimal impostoDevol_IPI_vIPIDevol { get; set; }
        public string prod_infAdProd { get; set; }

    }
}