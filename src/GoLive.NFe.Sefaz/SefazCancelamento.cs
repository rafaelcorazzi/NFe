using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoLive.NFe.ResponseParser.Entidades;
using System.Security.Cryptography.X509Certificates;
using GoLive.NFe.SOAP;
using GoLive.NFe.Leiaute;
using System.Xml;
namespace GoLive.NFe.Sefaz
{
    public class SefazCancelamento
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();
        /// <summary>
        /// Metodo para cancelamento de NF-e
        /// </summary>
        ///<param name="Ambiente">1-Produção | 2-Homologação</param>
        ///<param name="ChaveNFe">Chave da NF-e com 44 dígitos</param>
        ///<param name="numeroProtocolo">Protocolo da autorização de uso da NF-e</param>
        ///<param name="Justificativa">Justificativa do cancelamento com no min 15 caracteres</param>
        ///<param name="certificado">Certificado da Empresa Emissora Exemplo: X509Certificate2 certificado = NFeCertificadoDigital.getCertificate("My", Caminho_Certificado, Senha);</param>  
        ///<exception cref="GoLive.NFe.Sefaz.Exceptions.NFeException">Thrown when a non-numeric value is assigned.</exception>
   
        public RetEvento CancelarNFe(int Ambiente, Eventos evento, X509Certificate2 certificado, int Contingencia , int Modelo)
        {
            RetEvento eventoCanc = new RetEvento();
            string envioEvento = TSoap.soapXmlCancelamento(evento.ChaveNFe.Substring(0, 2), SefazEvento.EventoNFeCancelamento(Ambiente, evento.ChaveNFe, evento.Protocolo, evento.xCorrecao, certificado));

            string Resultado = string.Empty;

            switch(Ambiente)
            {
                case 1:
                    Resultado = prodNFe.NFeEvento(envioEvento, int.Parse(evento.ChaveNFe.Substring(0, 2)), Contingencia, certificado, Modelo);
                    break;
                case 2:
                    Resultado = homNFe.NFeEvento(envioEvento, int.Parse(evento.ChaveNFe.Substring(0, 2)), Contingencia, certificado, Modelo);
                    break;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Resultado);

            XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
            namespaces.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
            XmlNodeList retEnvEvento = doc.SelectNodes("descendant::nfe:retEnvEvento", namespaces);
            int processamento = 0;
            foreach (XmlNode ret in retEnvEvento)
            {
                foreach (XmlNode retEvento in ret.ChildNodes)
                {
                    //Console.WriteLine(infEvento.Name);
                    if ((retEvento.Name) == "cStat") processamento = int.Parse(retEvento.InnerText);
                    if (processamento == 128)
                    {
                        foreach (XmlNode inf in retEvento)
                        {
                            foreach (XmlNode infEvento in inf)
                            {
                                if ((infEvento.Name) == "cStat") eventoCanc.cStat = int.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "xMotivo") eventoCanc.xMotivo = infEvento.InnerText;
                                if ((infEvento.Name) == "chNFe") eventoCanc.chNFe = infEvento.InnerText;
                                if ((infEvento.Name) == "tpEvento") eventoCanc.tpEvento = infEvento.InnerText;
                                if ((infEvento.Name) == "xEvento") eventoCanc.xEvento =infEvento.InnerText;
                                if ((infEvento.Name) == "nSeqEvento") eventoCanc.nSeqEvento = int.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "CNPJDest") eventoCanc.CNPJDest = infEvento.InnerText;
                                if ((infEvento.Name) == "dhRegEvento") eventoCanc.dhRegEvento = DateTime.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "nProt") eventoCanc.nProt = infEvento.InnerText;
                            }
                        }
                    }
                    else
                    {
                        eventoCanc.cStat = 999;
                        eventoCanc.xMotivo = "Erro no processamento do Evento";
                        eventoCanc.dhRegEvento = DateTime.Now;
                        eventoCanc.xEvento = "Cancelamento";
                        eventoCanc.chNFe = evento.ChaveNFe;
                        eventoCanc.nProt = "999999999999999";
                        eventoCanc.tpEvento = "110111";
                    }

                }
            }

            return eventoCanc;
        }
    }
}
