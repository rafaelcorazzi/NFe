using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoLive.NFe.ResponseParser.Entidades;
using GoLive.NFe.Sefaz;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using GoLive.NFe.SOAP;
namespace GoLive.NFe.Sefaz
{
    public class SefazAutorizacao
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();
       
        public RetEnvNFe EnviarNFe(int ambiente, int cUF, X509Certificate2 certificado, string enviNFe, int modelo, int Contingencia)
        {
            RetEnvNFe resultado = new RetEnvNFe();

            string xmlEnvio = TSoap.soapXmlEnvioNFe(cUF.ToString(), enviNFe);

            string respostaSefaz = string.Empty;

            switch(ambiente)
            {
                case 1:
                    respostaSefaz = prodNFe.NFeAutorizacao(xmlEnvio, cUF, Contingencia, certificado, modelo);
                    break;
                case 2:
                    respostaSefaz = homNFe.NFeAutorizacao(xmlEnvio, cUF, Contingencia, certificado, modelo);
                    break;
            }

          
                XmlDocument xmlRetorno = new XmlDocument();
                xmlRetorno.LoadXml(respostaSefaz);
                XmlNamespaceManager namespaces = new XmlNamespaceManager(xmlRetorno.NameTable);
                namespaces.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
                XmlNodeList retEnviNFe = xmlRetorno.SelectNodes("descendant::nfe:retEnviNFe", namespaces);


                foreach (XmlNode ret in retEnviNFe)
                {


                    foreach (XmlNode retEnvio in ret)
                    {
                      

                        if (retEnvio.Name == "infRec")
                        {
                            XmlNodeList nInfRec = xmlRetorno.SelectNodes("descendant::nfe:retEnviNFe/nfe:infRec", namespaces);
                            foreach (XmlNode node_infRec in nInfRec)
                            {
                                foreach (XmlNode recibo in node_infRec)
                                {
                                    if ((recibo.Name) == "nRec") resultado.rec_nRec = recibo.InnerText;
                                    if ((recibo.Name) == "tMed") resultado.rec_tMed = int.Parse(recibo.InnerText);
                                }
                            }
                        }
                        else
                        {
                            if ((retEnvio.Name) == "cStat") resultado.cStat = int.Parse(retEnvio.InnerText);
                            if ((retEnvio.Name) == "xMotivo") resultado.xMotivo = retEnvio.InnerText;
                            if ((retEnvio.Name) == "cUF") resultado.cUF = int.Parse(retEnvio.InnerText);
                            if ((retEnvio.Name) == "dhRecbto") resultado.dhRecbto = DateTime.Parse(retEnvio.InnerText);
                        }

                    }

                }
        
            
            

            return resultado;

        }
    }
}
