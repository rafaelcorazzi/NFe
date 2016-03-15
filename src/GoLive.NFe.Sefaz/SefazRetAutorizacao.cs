using GoLive.NFe.ResponseParser.Entidades;
using GoLive.NFe.SOAP;
using GoLive.NFe.SOAP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class SefazRetAutorizacao
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();
        public RetConsReciNFe RetornoAutorizacao(int Ambiente, string nRec, int Contingencia,  X509Certificate2 certificado, int Modelo)
        {
            RetConsReciNFe reciNFe = new RetConsReciNFe();
            if(nRec.Length == 0)
            {
                throw new SefazException("Informe o número do recibo para consulta");
            }
            else if(Ambiente < 1 || Ambiente > 2)
            {
                throw new SefazException("Ambiente inconsistente informe 1 para produção ou 2 para homologação");
            }
            else if(certificado == null)
            {
                throw new SefazException("Informe o certificado para consulta");
            }
            else
            {
                string ConsultarRecibo = TSoap.soapXmlReciboNFe(nRec.Substring(0, 2), Ambiente, nRec);
                string Resultado = string.Empty;
                
                switch (Ambiente)
                {
                    case 1:
                        Resultado = prodNFe.NFeRetAutorizacao(ConsultarRecibo, int.Parse(nRec.Substring(0, 2)), Contingencia, certificado, Modelo);
                        break;
                    case 2:
                        Resultado = homNFe.NFeRetAutorizacao(ConsultarRecibo, int.Parse(nRec.Substring(0, 2)), Contingencia, certificado, Modelo);
                        break;
                }
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(Resultado);


                XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
                namespaces.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
                XmlNodeList consReciNFe = doc.SelectNodes("descendant::nfe:retConsReciNFe", namespaces);

                List<RetInfProt> infProt = new List<RetInfProt>();

                foreach (XmlNode NfeRetAutorizacao in consReciNFe)
                {

                    string prot_NFe = string.Empty;
                    foreach (XmlNode retConsReciNFe in NfeRetAutorizacao.ChildNodes)
                    {

                        //Console.WriteLine(retConsReciNFe.Name);
                        if ((retConsReciNFe.Name) == "cStat") reciNFe.cStat = int.Parse(retConsReciNFe.InnerText);
                        if ((retConsReciNFe.Name) == "xMotivo") reciNFe.xMotivo = retConsReciNFe.InnerText;
                        if ((retConsReciNFe.Name) == "dhRecbto") reciNFe.dhRecbto = DateTime.Parse(retConsReciNFe.InnerText);


                        prot_NFe = "<protNFe>" + retConsReciNFe.InnerXml + "</protNFe>";
                        if (retConsReciNFe.Name == "protNFe")
                        {



                            foreach (XmlNode prot in retConsReciNFe)
                            {

                                string chNFe = string.Empty;
                                Nullable<DateTime> dhRecbto = (DateTime?)null;
                                string nProt = string.Empty;
                                string digVal = string.Empty;
                                int cStat = 0;
                                string xMotivo = string.Empty;


                                foreach (XmlNode protNFe in prot)
                                {

                                    if ((protNFe.Name) == "chNFe") chNFe = protNFe.InnerText;
                                    if ((protNFe.Name) == "dhRecbto") dhRecbto = DateTime.Parse(protNFe.InnerText);
                                    if ((protNFe.Name) == "nProt") nProt = protNFe.InnerText;
                                    if ((protNFe.Name) == "digVal") digVal = protNFe.InnerText;
                                    if ((protNFe.Name) == "cStat") cStat = int.Parse(protNFe.InnerText);
                                    if ((protNFe.Name) == "xMotivo") xMotivo = protNFe.InnerText;

                                }
                                infProt.Add(new RetInfProt()
                                {
                                    prot_chNFe = chNFe,
                                    prot_dhRecbto = (DateTime)dhRecbto,
                                    prot_cStat = cStat,
                                    prot_nProt = nProt,
                                    prot_xMotivo = xMotivo,
                                    prot_autorizacao = prot_NFe
                                });
                                reciNFe.infProt = infProt;
                            }
                        }
                    }

                }
            }
           

            return reciNFe;
        }
    }
}
