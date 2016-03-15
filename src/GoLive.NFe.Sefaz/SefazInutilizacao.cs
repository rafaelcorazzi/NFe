using GoLive.NFe.SOAP;
using GoLive.NFe.ResponseParser.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class SefazInutilizacao
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();
        public RetInutNFe InutilizarNFe(int Ambiente,int ModeloNF, DateTime DataEvento,  string CnpjEmpresa, string Justificativa, int NFIni, int NFFin, int Serie, int UF, int Contingencia, X509Certificate2 certificado)
        {
            RetInutNFe retornoInutilizacao = new RetInutNFe();
            string chave = UF.ToString() + String.Format("{0:yy}", DataEvento) + CnpjEmpresa + ModeloNF + String.Format("{0:000}" + Serie) + String.Format("{0:000000000}", NFIni) + String.Format("{0:000000000}", NFFin);

            string XmlAssinado = SefazEvento.EventoInutilizar(Ambiente, UF,chave, DataEvento, ModeloNF, CnpjEmpresa, Serie, NFIni, NFIni, Justificativa, certificado);

            string InutilizarNFe = TSoap.soapXmlInutilizarNFe(UF.ToString(), XmlAssinado);

            string Resultado = string.Empty;
            switch (Ambiente)
            {
                case 1:
                    Resultado = prodNFe.NFeInutilizacao(InutilizarNFe, UF, Contingencia, certificado, ModeloNF);
                    break;
                case 2:
                    Resultado = homNFe.NFeRetAutorizacao(InutilizarNFe, UF, Contingencia, certificado, ModeloNF);
                    break;
            }
            XmlDocument xmlRetorno = new XmlDocument();
            xmlRetorno.LoadXml(Resultado);
            XmlElement Elementos = xmlRetorno.DocumentElement;

            foreach (XmlNode n1 in Elementos.ChildNodes)
            {

                foreach (XmlNode n2 in n1.ChildNodes)
                {

                    foreach (XmlNode n3 in n2.ChildNodes)
                    {
                        if (n3.Name == "retInutNFe")
                        {
                            foreach (XmlNode n4 in n3.ChildNodes)
                            {

                                if (n4.Name == "infInut")
                                {
                                    foreach (XmlNode n5 in n4.ChildNodes)
                                    {
                                        if ((n5.Name) == "cStat") retornoInutilizacao.cStat = int.Parse(n5.InnerText);
                                        if ((n5.Name) == "xMotivo") retornoInutilizacao.xMotivo = n5.InnerText;
                                        if ((n5.Name) == "nProt") retornoInutilizacao.nProt = n5.InnerText.Length == 0 ? "" : n5.InnerText;
                                        if ((n5.Name) == "dhRecbto") retornoInutilizacao.dhRecbto = DateTime.Parse(n5.InnerText);
                                    }


                                }
                                if (retornoInutilizacao.cStat == 102)
                                {
                                    retornoInutilizacao.prot_Inutilizacao = SefazInfProc.ProcInut(XmlAssinado, n3.InnerXml);
                                }
                            }
                        }

                    }

                }
            }
         
            return retornoInutilizacao;
        }
    }
}
