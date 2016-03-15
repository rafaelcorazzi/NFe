using GoLive.NFe.SOAP;
using GoLive.NFe.ResponseParser.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class SefazConsultaNFe
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();

        public RetConsNFe ConsultarNFe(int Ambiente, string ChaveNFe, int Contingencia, X509Certificate2 certificado, int Modelo)
        {
            RetConsNFe retornoConsulta = new RetConsNFe();
            string consultar = TSoap.soapXmlConsultaNFe(Ambiente, ChaveNFe);

            string Resultado = string.Empty;

            switch (Ambiente)
            {
                case 1:
                    Resultado = prodNFe.NFeConsultaProtocolo(consultar, int.Parse(ChaveNFe.Substring(0, 2)), Contingencia, certificado, Modelo);
                    break;
                case 2:
                    Resultado = homNFe.NFeConsultaProtocolo(consultar, int.Parse(ChaveNFe.Substring(0, 2)), Contingencia, certificado, Modelo);
                    break;
            }

            XmlDocument document = new XmlDocument();
            document.LoadXml(Resultado);

            XmlNamespaceManager namespaces = new XmlNamespaceManager(document.NameTable);
            namespaces.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
            XmlNodeList nl_infProt = document.SelectNodes("descendant::nfe:protNFe/nfe:infProt", namespaces);
            XmlNodeList nl_infCanc = document.SelectNodes("descendant::nfe:retCancNFe/nfe:infCanc", namespaces);

            if (nl_infCanc.Count != 0)
            {
                XmlNodeList nl_procEvento = document.SelectNodes("descendant::nfe:procEventoNFe/nfe:evento/nfe:infEvento", namespaces);

                foreach (XmlNode infProt in nl_infCanc)
                {
                    foreach (XmlNode var in infProt)
                    {
                        if ((var.Name) == "chNFe") retornoConsulta.prot_chNFe = var.InnerText;
                        if ((var.Name) == "cStat") retornoConsulta.prot_cStat = int.Parse(var.InnerText);
                        if ((var.Name) == "xMotivo") retornoConsulta.prot_xMotivo = var.InnerText;
                        if ((var.Name) == "dhRecbto") retornoConsulta.prot_dhRecbto = DateTime.Parse(var.InnerText);
                        if ((var.Name) == "nProt") retornoConsulta.prot_nProt = var.InnerText;

                    }
                    retornoConsulta.protocolo = "<protNFe versao=\"3.10\"><infProt Id=\"Id" + retornoConsulta.prot_nProt + "\">" + infProt.InnerXml + "</infProt></protNFe>";
                }


             
            }
            else
            {
                foreach (XmlNode infProt in nl_infProt)
                {
                    foreach (XmlNode var in infProt)
                    {
                        if ((var.Name) == "chNFe") retornoConsulta.prot_chNFe = var.InnerText;
                        if ((var.Name) == "cStat") retornoConsulta.prot_cStat = int.Parse(var.InnerText);
                        if ((var.Name) == "xMotivo") retornoConsulta.prot_xMotivo = var.InnerText;
                        if ((var.Name) == "dhRecbto") retornoConsulta.prot_dhRecbto = DateTime.Parse(var.InnerText);
                        if ((var.Name) == "nProt") retornoConsulta.prot_nProt = var.InnerText;




                    }
                    retornoConsulta.protocolo = "<protNFe versao=\"3.10\"><infProt Id=\"Id" + retornoConsulta.prot_nProt + "\">" + infProt.InnerXml + "</infProt></protNFe>";
                }
            }




            return retornoConsulta;

        }
    }
}
