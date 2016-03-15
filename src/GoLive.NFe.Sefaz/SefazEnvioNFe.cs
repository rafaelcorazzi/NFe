using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoLive.NFe.Leiaute;
using GoLive.NFe.Certificados;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
namespace GoLive.NFe.Sefaz
{
    public class SefazEnvioNFe
    {
        public static String NFeEnvio(int ambiente, infNFe NotaInformacoes,  IEnumerable<Produto> Produtos, X509Certificate2 certificado)
        {
            MemoryStream stream = new MemoryStream(); // The writer closes this for us
            StringBuilder result = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {

                string codNF = "00000000" + NotaInformacoes.ide_cNF;
                codNF = codNF.Substring(codNF.Length - 8, 8);

                string serie = "000" + NotaInformacoes.ide_serie;
                serie = serie.Substring(serie.Length - 3, 3);

                string numero = "000000000" + NotaInformacoes.ide_nNF;
                numero = numero.Substring(numero.Length - 9, 9);

                string ChaveNFe = NotaInformacoes.ide_cUF + String.Format("{0:yyMM}", NotaInformacoes.ide_dhEmit) + NotaInformacoes.emit_CNPJ + NotaInformacoes.ide_mod + serie + numero + NotaInformacoes.ide_tpEmiss + codNF;
                int cDV = int.Parse(NFeUtils.CalculaDigitoMod11(ChaveNFe, 1, 9));
               
                writer.WriteStartElement("NFe");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");

                writer.WriteStartElement("infNFe");
                //Id=\"NFe13140311707347000195650030000004591064552496\" versao=\"3.10\">"
                writer.WriteAttributeString("Id", "NFe" + ChaveNFe + cDV);
                writer.WriteAttributeString("versao", "3.10");

                #region "ide"
                
                writer.WriteStartElement("ide");
                writer.WriteElementString("cUF", NotaInformacoes.ide_cUF);
                writer.WriteElementString("cNF", NotaInformacoes.ide_cNF.ToString());
                writer.WriteElementString("natOp", NotaInformacoes.ide_natOp);
                writer.WriteElementString("indPag", NotaInformacoes.ide_indPag.ToString());
                writer.WriteElementString("mod", NotaInformacoes.ide_mod.ToString());
                writer.WriteElementString("serie", NotaInformacoes.ide_serie.ToString());
                writer.WriteElementString("nNF", NotaInformacoes.ide_nNF.ToString());
                writer.WriteElementString("dhEmi", String.Format("{0:yyyy-MM-ddTHH:mm:sszzz}", NotaInformacoes.ide_dhEmit));
                if (NotaInformacoes.ide_dhSaiEnt.HasValue && NotaInformacoes.ide_mod ==  55)
                    writer.WriteElementString("dhSaiEnt", String.Format("{0:yyyy-MM-ddTHH:mm:sszzz}", NotaInformacoes.ide_dhSaiEnt));
                writer.WriteElementString("tpNF", NotaInformacoes.ide_tpNF.ToString());
                writer.WriteElementString("idDest", NotaInformacoes.ide_idDest.ToString());
                writer.WriteElementString("cMunFG", NotaInformacoes.ide_cMunFG);
                writer.WriteElementString("tpImp", NotaInformacoes.ide_tpImp.ToString());
                writer.WriteElementString("tpEmis", NotaInformacoes.ide_tpEmiss.ToString());
                writer.WriteElementString("cDV", cDV.ToString());
                writer.WriteElementString("tpAmb", ambiente.ToString());
                writer.WriteElementString("finNFe", NotaInformacoes.ide_finNFe.ToString());
                writer.WriteElementString("indFinal", NotaInformacoes.ide_indFinal.ToString());
                writer.WriteElementString("indPres", NotaInformacoes.ide_indPres.ToString());
                writer.WriteElementString("procEmi", "0");
                writer.WriteElementString("verProc", "ConsultemaSFe_3.10");
                if (NotaInformacoes.ide_dhCont.HasValue)
                {
                    writer.WriteElementString("dhCont", String.Format("{0:yyyy-MM-ddTHH:mm:sszzz}", NotaInformacoes.ide_dhCont));
                    writer.WriteElementString("xJust", NotaInformacoes.ide_xJust);
                }
                #region "NFe Referenciada"

                if (NotaInformacoes.NotaReferenciada != null)
                {


                    foreach (NFeRef referencia in NotaInformacoes.NotaReferenciada)
                    {
                        writer.WriteStartElement("NFref");
                        switch (referencia.NFref_tp)
                        {
                            //NFe
                            case 1:
                                writer.WriteElementString("refNFe", referencia.NFref_refNFe);
                                break;
                            //NF normal
                            case 2:
                                writer.WriteStartElement("refNF");
                                writer.WriteElementString("cUF", referencia.NFref_refNFP_cUF);
                                writer.WriteElementString("AAMM", referencia.NFref_reNF_AAMM);
                                writer.WriteElementString("CNPJ", referencia.NFref_reNF_CNPJ);
                                writer.WriteElementString("mod", referencia.NFref_refNFP_mod);
                                writer.WriteElementString("serie", referencia.NFref_refNFP_serie.ToString());
                                writer.WriteElementString("nNF", referencia.NFref_refNFP_nNF);
                                writer.WriteEndElement();
                                break;
                            case 3:
                                writer.WriteStartElement("refNFP");
                                writer.WriteElementString("cUF", referencia.NFref_refNFP_cUF);
                                writer.WriteElementString("AAMM", referencia.NFref_reNF_AAMM);
                                if (!String.IsNullOrEmpty(referencia.NFref_reNF_CNPJ) || referencia.NFref_reNF_CNPJ.Length > 0)
                                    writer.WriteElementString("CNPJ", referencia.NFref_reNF_CNPJ);
                                if (!String.IsNullOrEmpty(referencia.NFref_refNFP_CPF) || referencia.NFref_refNFP_CPF.Length > 0)
                                    writer.WriteElementString("CPF", referencia.NFref_refNFP_CPF);
                                writer.WriteElementString("mod", referencia.NFref_refNFP_mod);
                                writer.WriteElementString("serie", referencia.NFref_refNFP_serie.ToString());
                                writer.WriteElementString("nNF", referencia.NFref_refNFP_nNF);
                                writer.WriteEndElement();
                                break;
                            case 4:
                                writer.WriteStartElement("refECF");
                                writer.WriteElementString("mod", referencia.NFref_refNFP_mod);
                                writer.WriteElementString("nECF", referencia.NFref_refECF_nECF);
                                writer.WriteElementString("nCOO", referencia.NFref_refECF_nCOO);
                                writer.WriteEndElement();
                                break;
                            case 5:
                                writer.WriteElementString("refCTe", referencia.NFref_refCTe);
                                break;
                        }
                        writer.WriteEndElement();
                    }
                #endregion
                }
                //</ide>
                writer.WriteEndElement();

                #endregion

                #region "emit"

                writer.WriteStartElement("emit");
                writer.WriteElementString("CNPJ", NotaInformacoes.emit_CNPJ);
                writer.WriteElementString("xNome", NotaInformacoes.emit_xNome);
                if (NotaInformacoes.emit_xFant.Length > 0)
                    writer.WriteElementString("xFant", NotaInformacoes.emit_xFant);
                writer.WriteStartElement("enderEmit");
                writer.WriteElementString("xLgr", NotaInformacoes.emit_enderEmit_xLgr);
                writer.WriteElementString("nro", NotaInformacoes.emit_enderEmit_nro);
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_enderEmit_xCpl) ||NotaInformacoes.emit_enderEmit_xCpl.Length > 0)
                    writer.WriteElementString("xCpl", NotaInformacoes.emit_enderEmit_xCpl);
                writer.WriteElementString("xBairro", NotaInformacoes.emit_enderEmit_xBairro);
                writer.WriteElementString("cMun", NotaInformacoes.emit_enderEmit_cMun);
                writer.WriteElementString("xMun", NotaInformacoes.emit_enderEmit_xMun);
                writer.WriteElementString("UF", NotaInformacoes.emit_enderEmit_UF);
                writer.WriteElementString("CEP", NotaInformacoes.emit_enderEmit_CEP);
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_enderEmit_cPais))
                {
                    writer.WriteElementString("cPais", NotaInformacoes.emit_enderEmit_cPais);
                    writer.WriteElementString("xPais", NotaInformacoes.emit_enderEmit_xPais);
                }
                
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_fone))
                    writer.WriteElementString("fone", NotaInformacoes.emit_fone);
                //enderEmit
                writer.WriteEndElement();

                writer.WriteElementString("IE", NotaInformacoes.emit_IE);
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_IEST))
                    writer.WriteElementString("IEST", NotaInformacoes.emit_IEST);
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_IM))
                    writer.WriteElementString("IM", NotaInformacoes.emit_IM);
                if (!String.IsNullOrEmpty(NotaInformacoes.emit_CNAE) && NotaInformacoes.total_ISSQNTot_dCompet.HasValue)
                    writer.WriteElementString("CNAE", NotaInformacoes.emit_CNAE);
                writer.WriteElementString("CRT", NotaInformacoes.emit_CRT.ToString());

                //emit
                writer.WriteEndElement();

                #endregion

                #region "avulsa"

                if (!String.IsNullOrEmpty(NotaInformacoes.avulsa_CNPJ))
                {
                    writer.WriteStartElement("avulsa");
                    writer.WriteElementString("CNPJ", NotaInformacoes.avulsa_CNPJ);
                    writer.WriteElementString("xOrgao", NotaInformacoes.avulsa_xOrgao);
                    writer.WriteElementString("matr", NotaInformacoes.avulsa_matr);
                    writer.WriteElementString("xAgente", NotaInformacoes.avulsa_xAgente);
                    if (NotaInformacoes.avulsa_fone.Length > 0)
                        writer.WriteElementString("fone", NotaInformacoes.avulsa_fone);
                    writer.WriteElementString("UF", NotaInformacoes.avulsa_xOrgao);
                    if (NotaInformacoes.avulsa_nDAR.Length > 0)
                        writer.WriteElementString("nDAR", NotaInformacoes.avulsa_xOrgao);
                    if (NotaInformacoes.avulsa_dEmi.HasValue)
                        writer.WriteElementString("dEmi", NotaInformacoes.avulsa_xOrgao);
                    if (NotaInformacoes.avulsa_vDAR > 0)
                        writer.WriteElementString("vDAR", NotaInformacoes.avulsa_xOrgao);
                    writer.WriteElementString("repEmi", NotaInformacoes.avulsa_xOrgao);
                    if (NotaInformacoes.avulsa_dPag.HasValue)
                        writer.WriteElementString("dPag", NotaInformacoes.avulsa_xOrgao);
                    //avulsa
                    writer.WriteEndElement();

                }
                #endregion

                #region "dest"

                writer.WriteStartElement("dest");
                if (!String.IsNullOrEmpty(NotaInformacoes.dest_CNPJ))
                    writer.WriteElementString("CNPJ", NotaInformacoes.dest_CNPJ);
                if (!String.IsNullOrEmpty(NotaInformacoes.dest_CPF))
                    writer.WriteElementString("CPF", NotaInformacoes.dest_CPF);
                if (!String.IsNullOrEmpty(NotaInformacoes.dest_idEstrangeiro) && NotaInformacoes.dest_idEstrangeiro != "0")
                    writer.WriteElementString("idEstrangeiro", NotaInformacoes.dest_idEstrangeiro);
               
                    writer.WriteElementString("xNome", NotaInformacoes.dest_xNome);
                    writer.WriteStartElement("enderDest");
                    writer.WriteElementString("xLgr", NotaInformacoes.dest_enderDest_xLgr);
                    writer.WriteElementString("nro", NotaInformacoes.dest_enderDest_nro);
                    if (NotaInformacoes.dest_enderDest_xCpl.Length > 0)
                        writer.WriteElementString("xCpl", NotaInformacoes.dest_enderDest_xCpl);
                    writer.WriteElementString("xBairro", NotaInformacoes.dest_enderDest_xBairro);
                    writer.WriteElementString("cMun", NotaInformacoes.dest_enderDest_cMun);
                    writer.WriteElementString("xMun", NotaInformacoes.dest_enderDest_xMun);
                    writer.WriteElementString("UF", NotaInformacoes.dest_enderDest_UF);
                    if (NotaInformacoes.dest_enderDest_CEP.Length > 0)
                        writer.WriteElementString("CEP", NotaInformacoes.dest_enderDest_CEP);
                    if (NotaInformacoes.dest_enderDest_cPais.Length > 0)
                        writer.WriteElementString("cPais", NotaInformacoes.dest_enderDest_cPais);
                    if (NotaInformacoes.dest_enderDest_xPais.Length > 0)
                        writer.WriteElementString("xPais", NotaInformacoes.dest_enderDest_xPais);
                    if (NotaInformacoes.dest_fone.Length > 0)
                        writer.WriteElementString("fone", NotaInformacoes.dest_fone);
                    //enderDest
                    writer.WriteEndElement();

                    writer.WriteElementString("indIEDest", NotaInformacoes.dest_indIEDest.ToString());
                    if (NotaInformacoes.dest_indIEDest == 1)
                        writer.WriteElementString("IE", NotaInformacoes.dest_IE);
                    if (!String.IsNullOrEmpty(NotaInformacoes.dest_ISUF))
                        writer.WriteElementString("ISUF", NotaInformacoes.dest_ISUF);
                    if (!String.IsNullOrEmpty(NotaInformacoes.dest_IM))
                        writer.WriteElementString("IM", NotaInformacoes.dest_IM);
                    if (!String.IsNullOrEmpty(NotaInformacoes.dest_email))
                        writer.WriteElementString("email", NotaInformacoes.dest_email);

                

                //dest
                writer.WriteEndElement();
                #endregion

                #region "retirada"



                if (!String.IsNullOrEmpty(NotaInformacoes.retirada_xLgr) && !String.IsNullOrEmpty(NotaInformacoes.retirada_cMun))
                {
                    writer.WriteStartElement("retirada");
                    if (NotaInformacoes.retirada_CNPJ.Length > 0 && NotaInformacoes.retirada_CNPJ.Length == 14)
                        writer.WriteElementString("CNPJ", NotaInformacoes.retirada_CNPJ);

                    if (NotaInformacoes.retirada_CPF.Length > 0 && NotaInformacoes.retirada_CPF.Length == 11)
                        writer.WriteElementString("CPF", NotaInformacoes.retirada_CPF);

                    writer.WriteElementString("xLgr", NotaInformacoes.retirada_xLgr);
                    writer.WriteElementString("nro", NotaInformacoes.retirada_nro);

                    if (NotaInformacoes.retirada_xCpl.Length > 0)
                        writer.WriteElementString("xCpl", NotaInformacoes.retirada_xCpl);
                    writer.WriteElementString("xBairro", NotaInformacoes.retirada_xBairro);
                    writer.WriteElementString("cMun", NotaInformacoes.retirada_cMun);
                    writer.WriteElementString("xMun", NotaInformacoes.retirada_xMun);
                    writer.WriteElementString("UF", NotaInformacoes.retirada_UF);
                    //retirada
                    writer.WriteEndElement();
                }


                #endregion

                #region "entrega"

                if (!String.IsNullOrEmpty(NotaInformacoes.entrega_xLgr) && !String.IsNullOrEmpty(NotaInformacoes.entrega_cMun))
                {
                    writer.WriteStartElement("entrega");
                    if (NotaInformacoes.entrega_CNPJ.Length > 0 && NotaInformacoes.entrega_CNPJ.Length == 14)
                        writer.WriteElementString("CNPJ", NotaInformacoes.entrega_CNPJ);
                    if (NotaInformacoes.entrega_CPF.Length > 0 && NotaInformacoes.entrega_CPF.Length == 11)
                        writer.WriteElementString("CPF", NotaInformacoes.entrega_CPF);

                    writer.WriteElementString("xLgr", NotaInformacoes.entrega_xLgr);
                    writer.WriteElementString("nro", NotaInformacoes.entrega_nro);
                    if (NotaInformacoes.entrega_xCpl.Length > 0)
                        writer.WriteElementString("xCpl", NotaInformacoes.entrega_xCpl);
                    writer.WriteElementString("xBairro", NotaInformacoes.entrega_xBairro);
                    writer.WriteElementString("cMun", NotaInformacoes.entrega_cMun);
                    writer.WriteElementString("xMun", NotaInformacoes.entrega_xMun);
                    writer.WriteElementString("UF", NotaInformacoes.entrega_UF);
                    //entrega
                    writer.WriteEndElement();
                }



                #endregion

                // #region "autXML"

                // writer.WriteStartElement("autXML");

                // //autXML
                // writer.WriteEndElement();

                // #endregion

                #region "det"
                int nItem = 0;
                foreach (Produto produtos  in Produtos)
                {
             
                    writer.WriteStartElement("det");
                    nItem++;
                    writer.WriteAttributeString("nItem", nItem.ToString());

                  #region "prod"

                    writer.WriteStartElement("prod");

                    writer.WriteElementString("cProd", produtos.prod_cProd);
                    writer.WriteElementString("cEAN", produtos.prod_cEAN);
                    writer.WriteElementString("xProd", produtos.prod_xProd.TrimEnd().TrimStart());
                    writer.WriteElementString("NCM", produtos.prod_NCM);
                    if (!String.IsNullOrEmpty(produtos.prod_NVE))
                        writer.WriteElementString("NVE", produtos.prod_NVE.Trim());
                    if (!String.IsNullOrEmpty(produtos.prod_EXTIPI))
                        writer.WriteElementString("EXTIPI", produtos.prod_EXTIPI);

                    writer.WriteElementString("CFOP", produtos.prod_CFOP);
                    writer.WriteElementString("uCom", produtos.prod_uCom);
                    writer.WriteElementString("qCom", produtos.prod_qCom.ToString().Replace(",", "."));
                    writer.WriteElementString("vUnCom", produtos.prod_vUnCom.ToString().Replace(",", "."));
                  
                    writer.WriteElementString("vProd", String.Format("{0:0.00}", produtos.prod_vProd).Replace(",", "."));
                    writer.WriteElementString("cEANTrib", produtos.prod_cEANTrib);
                    writer.WriteElementString("uTrib", produtos.prod_uTrib);
                    writer.WriteElementString("qTrib", String.Format("{0:0.####}", produtos.prod_qTrib).ToString().Replace(",", "."));
                    writer.WriteElementString("vUnTrib", String.Format("{0:0.####}", produtos.prod_vUnTrib).ToString().Replace(",", "."));

                    if (produtos.prod_vFrete > 0)
                        writer.WriteElementString("vFrete", produtos.prod_vFrete.ToString().Replace(",", "."));
                    if (produtos.prod_vSeg > 0)
                        writer.WriteElementString("vSeg", produtos.prod_vSeg.ToString().Replace(",", "."));
                    if (produtos.prod_vDesc > 0)
                        writer.WriteElementString("vDesc", produtos.prod_vDesc.ToString().Replace(",", "."));
                    if (produtos.prod_vOutro > 0)
                        writer.WriteElementString("vOutro", produtos.prod_vOutro.ToString().Replace(",", "."));

                    writer.WriteElementString("indTot", produtos.prod_indTot.ToString());
                    if (produtos.prod_DI != null)
                    {
                        foreach (DI di in produtos.prod_DI)
                        {
                            //DI
                            writer.WriteStartElement("DI");
                            writer.WriteElementString("nDI", di.DI_nDI);
                            writer.WriteElementString("dDI", String.Format("{0:yyyy-MM-dd}", di.DI_dDI));
                            writer.WriteElementString("xLocDesemb", di.DI_xLocDesemb);
                            writer.WriteElementString("UFDesemb", di.DI_UFDesemb);
                            writer.WriteElementString("dDesemb", String.Format("{0:yyyy-MM-dd}", di.DI_dDesemb));
                            writer.WriteElementString("tpViaTransp", di.DI_tpViaTransp.ToString());
                            writer.WriteElementString("vAFRMM", di.DI_vAFRMM.ToString().Replace(",", "."));
                            writer.WriteElementString("tpIntermedio", di.DI_tpIntermedio.ToString());
                            writer.WriteElementString("CNPJ", di.DI_CNPJ);
                            writer.WriteElementString("UFTerceiro", di.DI_UFTerceiro);
                            writer.WriteElementString("cExportador", di.DI_cExportador);


                            if(di.DI_Adi != null)
                            {
                                foreach (Adi adi in di.DI_Adi.Where(c => c.Adi_nDI == di.DI_nDI))
                                {
                                    writer.WriteStartElement("adi");
                                    writer.WriteElementString("nAdicao", adi.Adi_nAdicao.ToString());
                                    writer.WriteElementString("nSeqAdic", adi.Adi_nSeqAdic.ToString());
                                    writer.WriteElementString("cFabricante", adi.Adi_cFabricante);
                                    if (adi.Adi_vDescDI > 0)
                                        writer.WriteElementString("vDescDI", adi.Adi_vDescDI.ToString().Replace(",", "."));
                                    if (!String.IsNullOrEmpty(adi.Adi_nDraw))
                                        writer.WriteElementString("nDraw", adi.Adi_nDraw);
                                    //ADI
                                    writer.WriteEndElement();
                                }
                            }
                           

                            //DI
                            writer.WriteEndElement();
                        }

                    }
                       



                        if (!String.IsNullOrEmpty(produtos.prod_xPed))
                            writer.WriteElementString("xPed", produtos.prod_xPed);
                        if (!String.IsNullOrEmpty(produtos.prod_nItemPed))
                            writer.WriteElementString("nItemPed", produtos.prod_nItemPed);
                        if (!String.IsNullOrEmpty(produtos.prod_nFCI))
                            writer.WriteElementString("nFCI", produtos.prod_nFCI);




                    //prod
                    writer.WriteEndElement();

                  #endregion

                    #region "imposto"

                    writer.WriteStartElement("imposto");
                    if (produtos.imposto_vTotTrib > 0)
                        writer.WriteElementString("vTotTrib", produtos.imposto_vTotTrib.ToString().Replace(",", "."));

                    #region "ICMS"

                    writer.WriteStartElement("ICMS");

                    switch (produtos.ICMS_CST)
                    {
                        case "00":
                            writer.WriteStartElement("ICMS00");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            writer.WriteEndElement();
                            break;
                        case "10":
                            writer.WriteStartElement("ICMS10");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("modBCST", produtos.ICMS_modBCST.ToString());
                            if (produtos.ICMS_pMVAST > 0)
                                writer.WriteElementString("pMVAST", produtos.ICMS_pMVAST.ToString().Replace(",", "."));
                            if (produtos.ICMS_pRedBCST > 0)
                                writer.WriteElementString("pRedBCST", produtos.ICMS_pRedBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("vBCST", produtos.ICMS_vBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMSST", produtos.ICMS_pICMSST.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMSST", produtos.ICMS_vICMSST.ToString().Replace(",", "."));
                            writer.WriteEndElement();
                            break;
                        case "20":
                            writer.WriteStartElement("ICMS20");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            writer.WriteElementString("pRedBC", produtos.ICMS_pRedBC.ToString().Replace(",", "."));
                            writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                        case "30":
                            writer.WriteStartElement("ICMS30");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBCST", produtos.ICMS_modBCST.ToString());
                            if (produtos.ICMS_pMVAST > 0)
                                writer.WriteElementString("pMVAST", produtos.ICMS_pMVAST.ToString().Replace(",", "."));
                            if (produtos.ICMS_pRedBCST > 0)
                                writer.WriteElementString("pRedBCST", produtos.ICMS_pRedBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("vBCST", produtos.ICMS_vBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMSST", produtos.ICMS_pICMSST.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMSST", produtos.ICMS_vICMSST.ToString().Replace(",", "."));
                            writer.WriteEndElement();
                            break;
                        case "40":
                            writer.WriteStartElement("ICMS40");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                        case "41":
                            writer.WriteStartElement("ICMS40");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                        case "50":
                            writer.WriteStartElement("ICMS50");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                        case "51":
                            writer.WriteStartElement("ICMS51");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            if (!String.IsNullOrEmpty(produtos.ICMS_modBC.ToString()) && produtos.ICMS_modBC > -1)
                                writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            if (produtos.ICMS_pRedBC == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("pRedBC", produtos.ICMS_pRedBC.ToString().Replace(",", "."));
                            if (produtos.ICMS_vBC == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            if (produtos.ICMS_pICMS == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMSOp == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("vICMSOp", produtos.ICMS_vICMSOp.ToString().Replace(",", "."));
                            if (produtos.ICMS_pDif > 0)
                                writer.WriteElementString("pDif", produtos.ICMS_pDif.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMSDif == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("vICMSDif", produtos.ICMS_vICMSDif.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMS == 0 && produtos.ICMS_pDif > 0)
                                writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            writer.WriteEndElement();
                            break;
                        case "60":
                            writer.WriteStartElement("ICMS60");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            if (produtos.ICMS_vBCSTRet > 0 && produtos.ICMS_vICMSSTRet > 0)
                            {
                                writer.WriteElementString("vBCSTRet", produtos.ICMS_vBCSTRet.ToString().Replace(",", "."));
                                writer.WriteElementString("vICMSSTRet", produtos.ICMS_vICMSSTRet.ToString().Replace(",", "."));
                            }

                            writer.WriteEndElement();
                            break;
                        case "70":
                            writer.WriteStartElement("ICMS70");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            writer.WriteElementString("pRedBC", produtos.ICMS_pRedBC.ToString().Replace(",", "."));
                            writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("modBCST", produtos.ICMS_modBCST.ToString());
                            if (produtos.ICMS_pMVAST > 0)
                                writer.WriteElementString("pMVAST", produtos.ICMS_pMVAST.ToString().Replace(",", "."));
                            if (produtos.ICMS_pRedBCST > 0)
                                writer.WriteElementString("pRedBCST", produtos.ICMS_pRedBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("vBCST", produtos.ICMS_vBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMSST", produtos.ICMS_pICMSST.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMSST", produtos.ICMS_vICMSST.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                        case "90":
                            writer.WriteStartElement("ICMS90");
                            writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                            writer.WriteElementString("CST", produtos.ICMS_CST);
                            writer.WriteElementString("modBC", produtos.ICMS_modBC.ToString());
                            writer.WriteElementString("vBC", produtos.ICMS_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pRedBC", produtos.ICMS_pRedBC.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMS", produtos.ICMS_pICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMS", produtos.ICMS_vICMS.ToString().Replace(",", "."));
                            writer.WriteElementString("modBCST", produtos.ICMS_modBCST.ToString());
                            if (produtos.ICMS_pMVAST > 0)
                                writer.WriteElementString("pMVAST", produtos.ICMS_pMVAST.ToString().Replace(",", "."));
                            if (produtos.ICMS_pRedBCST > 0)
                                writer.WriteElementString("pRedBCST", produtos.ICMS_pRedBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("vBCST", produtos.ICMS_vBCST.ToString().Replace(",", "."));
                            writer.WriteElementString("pICMSST", produtos.ICMS_pICMSST.ToString().Replace(",", "."));
                            writer.WriteElementString("vICMSST", produtos.ICMS_vICMSST.ToString().Replace(",", "."));
                            if (produtos.ICMS_vICMSDeson > 0 && produtos.ICMS_motDesICMS != 0)
                            {
                                writer.WriteElementString("vICMSDeson", produtos.ICMS_vICMSDeson.ToString().Replace(",", "."));
                                writer.WriteElementString("motDesICMS", produtos.ICMS_motDesICMS.ToString());
                            }
                            writer.WriteEndElement();
                            break;
                    }

                    //ICMS
                    writer.WriteEndElement();

                 #endregion

                    #region "ICMSST"

                    if (produtos.ICMS_vBCSTRet > 0 &&
                        produtos.ICMS_vICMSSTRet > 0 &&
                        produtos.ICMS_vBCSTDest > 0 &&
                         produtos.ICMS_vICMSSTDest > 0)
                    {
                        writer.WriteStartElement("ICMSST");
                        writer.WriteElementString("orig", produtos.ICMS_orig.ToString());
                        writer.WriteElementString("CST", produtos.ICMS_CST);
                        writer.WriteElementString("vBCSTRet", produtos.ICMS_vBCSTRet.ToString().Replace(",", "."));
                        writer.WriteElementString("vICMSSTRet", produtos.ICMS_vICMSSTRet.ToString().Replace(",", "."));
                        writer.WriteElementString("vBCSTDest", produtos.ICMS_vBCSTDest.ToString().Replace(",", "."));
                        writer.WriteElementString("vICMSSTDest", produtos.ICMS_vICMSSTDest.ToString().Replace(",", "."));
                        //ICMSST
                        writer.WriteEndElement();
                    }

                 #endregion

                    #region "IPI"

                    if (!String.IsNullOrEmpty(produtos.IPITrib_CST))
                    {
                        writer.WriteStartElement("IPI");
                        if (!String.IsNullOrEmpty(produtos.IPI_clEnq))
                            writer.WriteElementString("clEnq", produtos.IPI_clEnq);
                        if (!String.IsNullOrEmpty(produtos.IPI_CNPJProd))
                            writer.WriteElementString("CNPJProd", produtos.IPI_CNPJProd);
                        if (!String.IsNullOrEmpty(produtos.IPI_cSelo))
                            writer.WriteElementString("cSelo", produtos.IPI_cSelo);
                        if (produtos.IPI_qSelo > 0)
                            writer.WriteElementString("qSelo", produtos.IPI_qSelo.ToString());
                        if (produtos.IPI_cEnq == 0)
                            writer.WriteElementString("cEnq", "999");
                        else
                            writer.WriteElementString("cEnq", produtos.IPI_cEnq.ToString());

                        if (produtos.IPITrib_CST == "00" ||
                            produtos.IPITrib_CST == "49" ||
                            produtos.IPITrib_CST == "50" ||
                            produtos.IPITrib_CST == "99"
                            )
                        {
                            writer.WriteStartElement("IPITrib");
                            writer.WriteElementString("CST", produtos.IPITrib_CST);

                            if (produtos.IPITrib_pIPI > 0 && produtos.IPITrib_vBC > 0)
                            {
                                writer.WriteElementString("vBC", produtos.IPITrib_vBC.ToString().Replace(",", "."));
                                writer.WriteElementString("pIPI", produtos.IPITrib_pIPI.ToString().Replace(",", "."));

                            }
                            if (produtos.IPITrib_vUnid > 0 && produtos.IPITrib_qUnid > 0)
                            {
                                writer.WriteElementString("qUnid", produtos.IPITrib_qUnid.ToString().Replace(",", "."));
                                writer.WriteElementString("vUnid", produtos.IPITrib_vUnid.ToString().Replace(",", "."));

                            }
                            if (produtos.IPITrib_pIPI == 0 && produtos.IPITrib_vBC == 0 && produtos.IPITrib_vUnid == 0 && produtos.IPITrib_qUnid == 0)
                            {
                                writer.WriteElementString("vBC", produtos.IPITrib_vBC.ToString().Replace(",", "."));
                                writer.WriteElementString("pIPI", produtos.IPITrib_pIPI.ToString().Replace(",", "."));
                            }

                            writer.WriteElementString("vIPI", produtos.IPITrib_vIPI.ToString().Replace(",", "."));
                            //IPITrib
                            writer.WriteEndElement();
                        }

                        if (produtos.IPITrib_CST == "01" ||
                            produtos.IPITrib_CST == "02" ||
                            produtos.IPITrib_CST == "03" ||
                            produtos.IPITrib_CST == "04" ||
                            produtos.IPITrib_CST == "05" ||
                            produtos.IPITrib_CST == "51" ||
                            produtos.IPITrib_CST == "52" ||
                            produtos.IPITrib_CST == "53" ||
                            produtos.IPITrib_CST == "54" ||
                            produtos.IPITrib_CST == "55")
                        {
                            writer.WriteStartElement("IPINT");
                            writer.WriteElementString("CST", produtos.IPITrib_CST);
                            //IPINT
                            writer.WriteEndElement();
                        }


                        //IPI
                        writer.WriteEndElement();
                    }


                    #endregion

                    #region "II"
                    if (NotaInformacoes.ide_idDest == 9 && NotaInformacoes.ide_idDest == 3 && NotaInformacoes.dest_idEstrangeiro.Length > 0)
                    {
                        writer.WriteStartElement("II");
                        writer.WriteElementString("vBC", produtos.II_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("vDespAdu", produtos.II_vDespAdu.ToString().Replace(",", "."));
                        writer.WriteElementString("vII", produtos.II_vII.ToString().Replace(",", "."));
                        writer.WriteElementString("vIOF", produtos.II_vIOF.ToString().Replace(",", "."));
                        //II
                        writer.WriteEndElement();
                    }
                    else
                    {
                        if (produtos.II_vBC > 0 &&
                        produtos.II_vDespAdu > 0 &&
                        produtos.II_vII > 0 &&
                        produtos.II_vIOF > 0)
                        {
                            writer.WriteStartElement("II");
                            writer.WriteElementString("vBC", produtos.II_vBC.ToString().Replace(",", "."));
                            writer.WriteElementString("vDespAdu", produtos.II_vDespAdu.ToString().Replace(",", "."));
                            writer.WriteElementString("vII", produtos.II_vII.ToString().Replace(",", "."));
                            writer.WriteElementString("vIOF", produtos.II_vIOF.ToString().Replace(",", "."));
                            //II
                            writer.WriteEndElement();
                        }
                    }


                    #endregion

                    #region "PIS"

                    writer.WriteStartElement("PIS");

                    if (produtos.PIS_CST == "01" || produtos.PIS_CST == "02")
                    {
                        writer.WriteStartElement("PISAliq");
                        writer.WriteElementString("CST", produtos.PIS_CST);
                        writer.WriteElementString("vBC", produtos.PIS_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pPIS", produtos.PIS_pPIS.ToString().Replace(",", "."));
                        writer.WriteElementString("vPIS", produtos.PIS_vPIS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    else if (produtos.PIS_CST == "03")
                    {
                        writer.WriteStartElement("PISQtde");
                        writer.WriteElementString("CST", produtos.PIS_CST);
                        writer.WriteElementString("qBCProd", produtos.PIS_qBCProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vAliqProd", produtos.PIS_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vPIS", produtos.PIS_vPIS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    else if (produtos.PIS_CST == "04" ||
                            produtos.PIS_CST == "05" ||
                            produtos.PIS_CST == "06" ||
                            produtos.PIS_CST == "07" ||
                            produtos.PIS_CST == "08" ||
                            produtos.PIS_CST == "09")
                    {
                        writer.WriteStartElement("PISNT");
                        writer.WriteElementString("CST", produtos.PIS_CST);
                        writer.WriteEndElement();
                    }
                    else if (produtos.PIS_CST == "49" ||
                            produtos.PIS_CST == "50" ||
                            produtos.PIS_CST == "51" ||
                            produtos.PIS_CST == "52" ||
                            produtos.PIS_CST == "53" ||
                            produtos.PIS_CST == "54" ||
                            produtos.PIS_CST == "55" ||
                            produtos.PIS_CST == "56" ||
                            produtos.PIS_CST == "60" ||
                            produtos.PIS_CST == "61" ||
                            produtos.PIS_CST == "62" ||
                            produtos.PIS_CST == "63" ||
                            produtos.PIS_CST == "64" ||
                            produtos.PIS_CST == "65" ||
                            produtos.PIS_CST == "66" ||
                            produtos.PIS_CST == "67" ||
                            produtos.PIS_CST == "70" ||
                            produtos.PIS_CST == "71" ||
                            produtos.PIS_CST == "72" ||
                            produtos.PIS_CST == "73" ||
                            produtos.PIS_CST == "74" ||
                            produtos.PIS_CST == "75" ||
                            produtos.PIS_CST == "98" ||
                            produtos.PIS_CST == "99")
                    {
                        writer.WriteStartElement("PISOutr");
                        writer.WriteElementString("CST", produtos.PIS_CST);
                        writer.WriteElementString("vBC", produtos.PIS_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pPIS", produtos.PIS_pPIS.ToString().Replace(",", "."));
                        if (produtos.PIS_qBCProd > 0)
                            writer.WriteElementString("qBCProd", produtos.PIS_qBCProd.ToString().Replace(",", "."));
                        if (produtos.PIS_vAliqProd > 0)
                            writer.WriteElementString("vAliqProd", produtos.PIS_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vPIS", produtos.PIS_vPIS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    //PIS
                    writer.WriteEndElement();
                    #endregion

                    #region "PISST"

                    if(produtos.PISST_vPIS > 0)
                    {
                        writer.WriteStartElement("PISST");
                        writer.WriteElementString("vBC", produtos.PISST_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pPIS", produtos.PISST_pPIS.ToString().Replace(",", "."));
                        writer.WriteElementString("qBCProd", produtos.PISST_qBCProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vAliqProd", produtos.PISST_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vPIS", produtos.PISST_vPIS.ToString().Replace(",", "."));
                        //IPINT
                        writer.WriteEndElement();
                    }
                 
                    #endregion

                    #region "COFINS"
                    writer.WriteStartElement("COFINS");

                    if (produtos.COFINS_CST == "01" || produtos.COFINS_CST == "02")
                    {
                        writer.WriteStartElement("COFINSAliq");
                        writer.WriteElementString("CST", produtos.COFINS_CST);
                        writer.WriteElementString("vBC", produtos.COFINS_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pCOFINS", produtos.COFINS_pCOFINS.ToString().Replace(",", "."));
                        writer.WriteElementString("vCOFINS", produtos.COFINS_vCOFINS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    else if (produtos.COFINS_CST == "03")
                    {
                        writer.WriteStartElement("COFINSQtde");
                        writer.WriteElementString("CST", produtos.COFINS_CST);
                        writer.WriteElementString("qBCProd", produtos.COFINS_qBCProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vAliqProd", produtos.COFINS_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vCOFINS", produtos.COFINS_vCOFINS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    else if (produtos.COFINS_CST == "04" ||
                            produtos.COFINS_CST == "05" ||
                            produtos.COFINS_CST == "06" ||
                            produtos.COFINS_CST == "07" ||
                            produtos.COFINS_CST == "08" ||
                            produtos.COFINS_CST == "09")
                    {
                        writer.WriteStartElement("COFINSNT");
                        writer.WriteElementString("CST", produtos.COFINS_CST);
                        writer.WriteEndElement();
                    }
                    else if (produtos.COFINS_CST == "49" ||
                              produtos.COFINS_CST == "50" ||
                              produtos.COFINS_CST == "51" ||
                              produtos.COFINS_CST == "52" ||
                              produtos.COFINS_CST == "53" ||
                              produtos.COFINS_CST == "54" ||
                              produtos.COFINS_CST == "55" ||
                              produtos.COFINS_CST == "56" ||
                              produtos.COFINS_CST == "60" ||
                              produtos.COFINS_CST == "61" ||
                              produtos.COFINS_CST == "62" ||
                              produtos.COFINS_CST == "63" ||
                              produtos.COFINS_CST == "64" ||
                              produtos.COFINS_CST == "65" ||
                              produtos.COFINS_CST == "66" ||
                              produtos.COFINS_CST == "67" ||
                              produtos.COFINS_CST == "70" ||
                              produtos.COFINS_CST == "71" ||
                              produtos.COFINS_CST == "72" ||
                              produtos.COFINS_CST == "73" ||
                              produtos.COFINS_CST == "74" ||
                              produtos.COFINS_CST == "75" ||
                              produtos.COFINS_CST == "98" ||
                              produtos.COFINS_CST == "99")
                    {
                        writer.WriteStartElement("COFINSOutr");
                        writer.WriteElementString("CST", produtos.COFINS_CST);
                        writer.WriteElementString("vBC", produtos.COFINS_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pCOFINS", produtos.COFINS_pCOFINS.ToString().Replace(",", "."));
                        if (produtos.COFINS_qBCProd > 0)
                            writer.WriteElementString("qBCProd", produtos.COFINS_qBCProd.ToString().Replace(",", "."));
                        if (produtos.COFINS_vAliqProd > 0)
                            writer.WriteElementString("vAliqProd", produtos.COFINS_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vCOFINS", produtos.COFINS_vCOFINS.ToString().Replace(",", "."));
                        writer.WriteEndElement();
                    }
                    //PIS
                    writer.WriteEndElement();
                    #endregion

                    #region "COFINSST"
                    if (produtos.COFINSST_vCOFINS > 0)
                    {
                        writer.WriteStartElement("COFINSST");
                        writer.WriteElementString("vBC", produtos.COFINSST_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("pCOFINS", produtos.COFINSST_pCOFINS.ToString().Replace(",", "."));
                        writer.WriteElementString("qBCProd", produtos.COFINSST_qBCProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vAliqProd", produtos.COFINSST_vAliqProd.ToString().Replace(",", "."));
                        writer.WriteElementString("vCOFINS", produtos.COFINSST_vCOFINS.ToString().Replace(",", "."));
                        //IPINT
                        writer.WriteEndElement();
                    }

                

                    #endregion

                    #region "ISSQN"
                    if (!String.IsNullOrEmpty(produtos.ISSQN_cServico))
                    {
                        writer.WriteStartElement("ISSQN");
                        writer.WriteElementString("vBC", produtos.ISSQN_vBC.ToString().Replace(",", "."));
                        writer.WriteElementString("vAliq", produtos.ISSQN_vAliq.ToString().Replace(",", "."));
                        writer.WriteElementString("vISSQN", produtos.ISSQN_vISSQN.ToString().Replace(",", "."));
                        writer.WriteElementString("cMunFG", produtos.ISSQN_cMunFG);
                        writer.WriteElementString("cListServ", produtos.ISSQN_cListServ);
                        writer.WriteElementString("vDeducao", produtos.ISSQN_vDeducao.ToString().Replace(",", "."));
                        writer.WriteElementString("vOutro", produtos.ISSQN_vOutro.ToString().Replace(",", "."));
                        writer.WriteElementString("vDescIncond", produtos.ISSQN_vDescIncond.ToString().Replace(",", "."));
                        writer.WriteElementString("vDescCond", produtos.ISSQN_vDescCond.ToString().Replace(",", "."));
                        writer.WriteElementString("vISSRet", produtos.ISSQN_vISSRet.ToString().Replace(",", "."));
                        writer.WriteElementString("indISS", produtos.ISSQN_indISS.ToString());
                        writer.WriteElementString("cServico", produtos.ISSQN_cServico.ToString());
                        writer.WriteElementString("cMun", produtos.ISSQN_cMun);
                        writer.WriteElementString("cPais", produtos.ISSQN_cPais);
                        writer.WriteElementString("nProcesso", produtos.ISSQN_nProcesso);
                        writer.WriteElementString("indIncentivo", produtos.ISSQN_indIncentivo.ToString());
                        writer.WriteEndElement();

                    }
                
               

                   

                    #endregion


                    //imposto
                    writer.WriteEndElement();

               #endregion

                //    #region "Outras Tags"

                //    #region "impostoDevol"

                //    #endregion

                   #region "infAdProd"

                    if (!String.IsNullOrEmpty(produtos.prod_infAdProd))
                        writer.WriteElementString("infAdProd", produtos.prod_infAdProd);


             #endregion

             
                //    //det
                     writer.WriteEndElement();
                }

                #endregion

                #region "total"

                writer.WriteStartElement("total");

                #region "ICMSTot"

                writer.WriteStartElement("ICMSTot");
                writer.WriteElementString("vBC", NotaInformacoes.total_ICMSTot_vBC.ToString().Replace(",", "."));
                writer.WriteElementString("vICMS", NotaInformacoes.total_ICMSTot_vICMS.ToString().Replace(",", "."));
                writer.WriteElementString("vICMSDeson", NotaInformacoes.total_ICMSTot_vICMSDeson.ToString().Replace(",", "."));
                writer.WriteElementString("vBCST", NotaInformacoes.total_ICMSTot_vBCST.ToString().Replace(",", "."));
                writer.WriteElementString("vST", NotaInformacoes.total_ICMSTot_vST.ToString().Replace(",", "."));
                writer.WriteElementString("vProd", NotaInformacoes.total_ICMSTot_vProd.ToString().Replace(",", "."));
                writer.WriteElementString("vFrete", NotaInformacoes.total_ICMSTot_vFrete.ToString().Replace(",", "."));
                writer.WriteElementString("vSeg", NotaInformacoes.total_ICMSTot_vSeg.ToString().Replace(",", "."));
                writer.WriteElementString("vDesc", NotaInformacoes.total_ICMSTot_vDesc.ToString().Replace(",", "."));
                writer.WriteElementString("vII", NotaInformacoes.total_ICMSTot_vII.ToString().Replace(",", "."));
                writer.WriteElementString("vIPI", NotaInformacoes.total_ICMSTot_vIPI.ToString().Replace(",", "."));
                writer.WriteElementString("vPIS", NotaInformacoes.total_ICMSTot_vPIS.ToString().Replace(",", "."));
                writer.WriteElementString("vCOFINS", NotaInformacoes.total_ICMSTot_vCOFINS.ToString().Replace(",", "."));
                writer.WriteElementString("vOutro", NotaInformacoes.total_ICMSTot_vOutro.ToString().Replace(",", "."));
                writer.WriteElementString("vNF", NotaInformacoes.total_ICMSTot_vNF.ToString().Replace(",", "."));
                if (NotaInformacoes.total_ICMSTot_vTotTrib > 0)
                    writer.WriteElementString("vTotTrib", NotaInformacoes.total_ICMSTot_vTotTrib.ToString().Replace(",", "."));
                //ICMSTot
                writer.WriteEndElement();

                #endregion

                #region "ISSQNtot"

                if (NotaInformacoes.total_ISSQNTot_dCompet.HasValue)
                {
                    writer.WriteStartElement("ISSQNtot");
                    if (NotaInformacoes.total_ISSQNTot_vServ > 0)
                        writer.WriteElementString("vServ", NotaInformacoes.total_ISSQNTot_vServ.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vBC > 0)
                        writer.WriteElementString("vBC", NotaInformacoes.total_ISSQNTot_vBC.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vISS > 0)
                        writer.WriteElementString("vISS", NotaInformacoes.total_ISSQNTot_vISS.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vPIS > 0)
                        writer.WriteElementString("vPIS", NotaInformacoes.total_ISSQNTot_vPIS.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vCOFINS > 0)
                        writer.WriteElementString("vCOFINS", NotaInformacoes.total_ISSQNTot_vCOFINS.ToString().Replace(",", "."));
                    writer.WriteElementString("dCompet", String.Format("{0:yyyy-MM-dd}", NotaInformacoes.total_ISSQNTot_dCompet));
                    if (NotaInformacoes.total_ISSQNTot_vDeducao > 0)
                        writer.WriteElementString("vDeducao", NotaInformacoes.total_ISSQNTot_vDeducao.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vOutro > 0)
                        writer.WriteElementString("vOutro", NotaInformacoes.total_ISSQNTot_vOutro.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vDescIncond > 0)
                        writer.WriteElementString("vDescIncond", NotaInformacoes.total_ISSQNTot_vDescIncond.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vDescCond > 0)
                        writer.WriteElementString("vDescCond", NotaInformacoes.total_ISSQNTot_vDescCond.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_vISSRet > 0)
                        writer.WriteElementString("vISSRet", NotaInformacoes.total_ISSQNTot_vISSRet.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_ISSQNTot_cRegTrib > 0)
                        writer.WriteElementString("cRegTrib", NotaInformacoes.total_ISSQNTot_cRegTrib.ToString());
                    //ISSQNtot
                    writer.WriteEndElement();
                }

                //ISSQNtot
                #endregion

                #region "retTrib"

                if (NotaInformacoes.total_retTrib_vRetPIS > 0 ||
                    NotaInformacoes.total_retTrib_vRetCOFINS > 0 ||
                    NotaInformacoes.total_retTrib_vRetCSLL > 0 ||
                    NotaInformacoes.total_retTrib_vBCIRRF > 0 ||
                    NotaInformacoes.total_retTrib_vIRRF > 0 ||
                    NotaInformacoes.total_retTrib_vBCRetPrev > 0 ||
                    NotaInformacoes.total_retTrib_vRetPrev > 0)
                {
                    //retTrib
                    writer.WriteStartElement("retTrib");
                    if (NotaInformacoes.total_retTrib_vRetPIS > 0)
                        writer.WriteElementString("vRetPIS", NotaInformacoes.total_retTrib_vRetPIS.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vRetCOFINS > 0)
                        writer.WriteElementString("vRetCOFINS", NotaInformacoes.total_retTrib_vRetCOFINS.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vRetCSLL > 0)
                        writer.WriteElementString("vRetCSLL", NotaInformacoes.total_retTrib_vRetCSLL.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vBCIRRF > 0)
                        writer.WriteElementString("vBCIRRF", NotaInformacoes.total_retTrib_vBCIRRF.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vIRRF > 0)
                        writer.WriteElementString("vIRRF", NotaInformacoes.total_retTrib_vIRRF.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vBCRetPrev > 0)
                        writer.WriteElementString("vBCRetPrev", NotaInformacoes.total_retTrib_vBCRetPrev.ToString().Replace(",", "."));
                    if (NotaInformacoes.total_retTrib_vRetPrev > 0)
                        writer.WriteElementString("vRetPrev", NotaInformacoes.total_retTrib_vRetPrev.ToString().Replace(",", "."));
                    writer.WriteEndElement();

                }

                #endregion
                //total
                writer.WriteEndElement();

                #endregion

                #region "transp"

                writer.WriteStartElement("transp");
                writer.WriteElementString("modFrete", NotaInformacoes.transp_modFrete.ToString());

                if (NotaInformacoes.transp_modFrete != 9)
                {
                    #region "transporta"

                    
                        writer.WriteStartElement("transporta");
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_CNPJ))
                            writer.WriteElementString("CNPJ", NotaInformacoes.transporta_CNPJ);
                        if (NotaInformacoes.transporta_CPF.Length > 0 && NotaInformacoes.transporta_CPF.Length == 11)
                            writer.WriteElementString("CPF", NotaInformacoes.transporta_CPF);
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_xNome))
                            writer.WriteElementString("xNome", NotaInformacoes.transporta_xNome);
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_IE))
                            writer.WriteElementString("IE", NotaInformacoes.transporta_IE);
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_xEnder))
                            writer.WriteElementString("xEnder", NotaInformacoes.transporta_xEnder);
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_xMun))
                            writer.WriteElementString("xMun", NotaInformacoes.transporta_xMun);
                        if (!String.IsNullOrEmpty(NotaInformacoes.transporta_UF))
                            writer.WriteElementString("UF", NotaInformacoes.transporta_UF);


                        //transporta
                        writer.WriteEndElement();
                 
                    #endregion

                    #region "retTransp"
                    if (NotaInformacoes.retTransp_vServ > 0)
                    {
                        writer.WriteStartElement("retTransp");
                        writer.WriteElementString("vServ", NotaInformacoes.retTransp_vServ.ToString().Replace(",", "."));
                        writer.WriteElementString("vBCRet", NotaInformacoes.retTransp_vBCRet.ToString().Replace(",", "."));
                        writer.WriteElementString("pICMSRet", NotaInformacoes.retTransp_pICMSRet.ToString().Replace(",", "."));
                        writer.WriteElementString("vICMSRet", NotaInformacoes.retTransp_vICMSRet.ToString().Replace(",", "."));
                        writer.WriteElementString("CFOP", NotaInformacoes.retTransp_CFOP);
                        writer.WriteElementString("cMunFG", NotaInformacoes.retTransp_cMunFG);

                        //retTransp
                        writer.WriteEndElement();
                    }
                    #endregion

                    #region "veicTransp"

                    if (!String.IsNullOrEmpty(NotaInformacoes.veicTransp_placa) && !String.IsNullOrEmpty(NotaInformacoes.veicTransp_UF))
                    {
                        writer.WriteStartElement("veicTransp");

                        writer.WriteElementString("placa", NotaInformacoes.veicTransp_placa);
                        writer.WriteElementString("UF", NotaInformacoes.veicTransp_UF);
                        if (!String.IsNullOrEmpty(NotaInformacoes.veicTransp_RNTC))
                            writer.WriteElementString("RNTC", NotaInformacoes.veicTransp_RNTC);
                        //veicTransp
                        writer.WriteEndElement();
                    }


                    #endregion

                    #region "reboque"

                    if (NotaInformacoes.reboque != null)
                    {
                        foreach (reboque Reboque in NotaInformacoes.reboque)
                        {
                            if (!String.IsNullOrEmpty(Reboque.reboque_placa) && !String.IsNullOrEmpty(Reboque.reboque_UF))
                            {
                                writer.WriteStartElement("reboque");

                                writer.WriteElementString("placa", Reboque.reboque_placa);
                                writer.WriteElementString("UF", Reboque.reboque_UF);
                                if (!String.IsNullOrEmpty(Reboque.reboque_RNTC))
                                    writer.WriteElementString("RNTC", Reboque.reboque_RNTC);
                                if (!String.IsNullOrEmpty(Reboque.reboque_vagao))
                                    writer.WriteElementString("vagao", Reboque.reboque_vagao);
                                if (!String.IsNullOrEmpty(Reboque.reboque_balsa))
                                    writer.WriteElementString("balsa", Reboque.reboque_balsa);
                                //veicTransp
                                writer.WriteEndElement();
                            }

                        }
                    }
               
                   
                    #endregion
                }
                
                if(NotaInformacoes.Volumes != null)
                {
                    foreach (volumes Volumes in NotaInformacoes.Volumes)
                    {
                        writer.WriteStartElement("vol");
                        if (!String.IsNullOrEmpty(Volumes.vol_qVol))
                            writer.WriteElementString("qVol", Volumes.vol_qVol);
                        if (!String.IsNullOrEmpty(Volumes.vol_esp))
                            writer.WriteElementString("esp", Volumes.vol_esp);
                        if (!String.IsNullOrEmpty(Volumes.vol_marca))
                            writer.WriteElementString("marca", Volumes.vol_marca);
                        if (!String.IsNullOrEmpty(Volumes.vol_nVol))
                            writer.WriteElementString("nVol", Volumes.vol_nVol);
                        if (Volumes.vol_pesoL > 0)
                            writer.WriteElementString("pesoL", Volumes.vol_pesoL.ToString().Replace(",", "."));
                        if (Volumes.vol_pesoB > 0)
                            writer.WriteElementString("pesoB", Volumes.vol_pesoB.ToString().Replace(",", "."));

                        if(Volumes.vol_Lacres != null)
                        {
                            foreach (lacres Lacres in Volumes.vol_Lacres)
                            {
                                writer.WriteStartElement("lacres");
                                writer.WriteElementString("nLacre", Lacres.lacres_nLacre);
                                writer.WriteEndElement();
                            }

                        }
                       
                        writer.WriteEndElement();
                    }
                }
              
                //transp
                writer.WriteEndElement();

                #endregion



               

                #region "cobr"

                if(NotaInformacoes.ide_mod == 55)
                {
                    writer.WriteStartElement("cobr");

                    if (!String.IsNullOrEmpty(NotaInformacoes.cobr_nFat) ||
                        NotaInformacoes.cobr_vOrig > 0 ||
                        NotaInformacoes.cobr_vDesc > 0 ||
                        NotaInformacoes.cobr_vLiq > 0)
                    {
                        writer.WriteStartElement("fat");
                        if (!String.IsNullOrEmpty(NotaInformacoes.cobr_nFat))
                            writer.WriteElementString("nFat", NotaInformacoes.cobr_nFat);
                        if (NotaInformacoes.cobr_vOrig > 0)
                            writer.WriteElementString("vOrig", NotaInformacoes.cobr_vOrig.ToString().Replace(",", "."));
                        if (NotaInformacoes.cobr_vDesc > 0)
                            writer.WriteElementString("vDesc", NotaInformacoes.cobr_vDesc.ToString().Replace(",", "."));
                        if (NotaInformacoes.cobr_vLiq > 0)
                            writer.WriteElementString("vLiq", NotaInformacoes.cobr_vLiq.ToString().Replace(",", "."));
                        //fat
                        writer.WriteEndElement();
                    }

                    #region "Duplicatas"
                    if (NotaInformacoes.Duplicata != null)
                    {
                        foreach (duplicata dup in NotaInformacoes.Duplicata)
                        {
                            writer.WriteStartElement("dup");
                            writer.WriteElementString("nDup", dup.dup_nDup);
                            writer.WriteElementString("dVenc", String.Format("{0:yyyy-MM-dd}", dup.dup_dVenc));
                            writer.WriteElementString("vDup", dup.dup_vDup.ToString().Replace(",", "."));
                            writer.WriteEndElement();
                        }

                    }



                    #endregion

                    //cobr
                    writer.WriteEndElement();
                }
                

                #endregion

                #region "Pagamento"
                if(NotaInformacoes.pagamento != null)
                {
                    foreach (Pagamentos pag in NotaInformacoes.pagamento)
                    {
                        writer.WriteStartElement("pag");
                        writer.WriteElementString("tPag", pag.pag_tPag);
                        writer.WriteElementString("vPag", pag.pag_vPag.ToString().Replace(",", "."));

                        if (!String.IsNullOrEmpty(pag.pag_card_tBand))
                        {
                            writer.WriteStartElement("card");
                            writer.WriteElementString("CNPJ", pag.pag_card_CNPJ);
                            writer.WriteElementString("tBand", pag.pag_card_tBand);
                            writer.WriteElementString("cAut", pag.pag_card_cAut);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }
              

                #endregion

                #region "infAdic"

                if (!String.IsNullOrEmpty(NotaInformacoes.infAdic_infAdFisco) ||
                    !String.IsNullOrEmpty(NotaInformacoes.infAdic_infCpl))
                {
                    writer.WriteStartElement("infAdic");
                    if (!String.IsNullOrEmpty(NotaInformacoes.infAdic_infAdFisco))
                        writer.WriteElementString("infAdFisco", NotaInformacoes.infAdic_infAdFisco);
                    if (!String.IsNullOrEmpty(NotaInformacoes.infAdic_infCpl))
                        writer.WriteElementString("infCpl", NotaInformacoes.infAdic_infCpl);
                    //infAdic
                    writer.WriteEndElement();
                }

                #endregion

                #region "exporta"

                if (!String.IsNullOrEmpty(NotaInformacoes.exporta_UFSaidaPais) &&
                    !String.IsNullOrEmpty(NotaInformacoes.exporta_xLocExporta))
                {
                    writer.WriteStartElement("exporta");
                    writer.WriteElementString("UFSaidaPais", NotaInformacoes.exporta_UFSaidaPais);
                    writer.WriteElementString("xLocExporta", NotaInformacoes.exporta_xLocExporta);
                    if (!String.IsNullOrEmpty(NotaInformacoes.exporta_xLocDespacho))
                        writer.WriteElementString("xLocDespacho", NotaInformacoes.exporta_xLocDespacho);
                    //exporta
                    writer.WriteEndElement();
                }

                #endregion

                #region "compra"

                if (!String.IsNullOrEmpty(NotaInformacoes.compra_xNEmp) ||
                    !String.IsNullOrEmpty(NotaInformacoes.compra_xPed) ||
                    !String.IsNullOrEmpty(NotaInformacoes.compra_xCont))
                {
                    writer.WriteStartElement("compra");
                    if (!String.IsNullOrEmpty(NotaInformacoes.compra_xNEmp))
                        writer.WriteElementString("xNEmp", NotaInformacoes.compra_xNEmp);
                    if (!String.IsNullOrEmpty(NotaInformacoes.compra_xPed))
                        writer.WriteElementString("xPed", NotaInformacoes.compra_xPed);
                    if (!String.IsNullOrEmpty(NotaInformacoes.compra_xCont))
                        writer.WriteElementString("xCont", NotaInformacoes.compra_xCont);
                    //compra
                    writer.WriteEndElement();
                }

                #endregion

                //NFe
                writer.WriteEndElement();
                //infNFe 
                writer.WriteEndElement();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result.Append(reader.ReadToEnd());

                reader.Close();
            }
            XmlDocument xmlOutputSign = new XmlDocument();
            int intError = 0;

            //carrega o documento xml com os dados da nfe sem assinatura
            XmlDocument docSefaz = new XmlDocument();
            docSefaz.LoadXml(result.ToString());
            //procura pela tag infNFe no xml e insere uma tag chamada Id e assina a NFe colocanod a tag
            //<signature></signature>
             xmlOutputSign = GoLive.NFe.Certificados.NFeCertificadoDigital.SignXML(docSefaz, "infNFe", "Id", certificado, out intError);//SFe.Certificados.SFeCertificadoDigital.SignXML(docSefaz, "infNFe", "Id", Emitente.CnpjTransmissor, out intError);
              result = null;
              String erroXml = string.Empty;
            if (intError > 0)
            {
                erroXml = "Erro ao assinar documento XML";
            }


            return XmlEnvNFe(ambiente, "0", String.Format("{0:yyyyMMddHHmmss0}", DateTime.Now), xmlOutputSign.OuterXml, out erroXml);
        }
      
        private static String XmlEnvNFe(int ambiente, string indSinc,  string Lote, String NFe, out string ErroXml)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us
            ErroXml = string.Empty;
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {


                writer.WriteStartElement("enviNFe");

                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");
                writer.WriteElementString("idLote", Lote);
                writer.WriteElementString("indSinc", indSinc);



                writer.WriteRaw(NFe);

                writer.WriteEndElement();



                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            CompareXSD(result, @"\PL_008e\enviNFe_v3.10.xsd");

           

            ErroXml = erroXML;

            return result;

        }
        public static string erroXML = string.Empty;
        #region "Validation Schema"

        public static void ValidationCallBack(object sender, ValidationEventArgs sErro)
        {

            erroXML += sErro.Message.Replace("http://www.portalfiscal.inf.br/nfe", "") + "<br>";
        }
        public static void CompareXSD(string xmldef, string xsdfile)
        {
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmldef);
                XmlNodeReader nodeReader = new XmlNodeReader(doc);
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add("http://www.portalfiscal.inf.br/nfe", xsdfile);
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                XmlReader reader = XmlReader.Create(nodeReader, settings);
                while (reader.Read()) ;
            }
            catch { }

        }

        #endregion
    }
}
