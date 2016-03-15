using GoLive.NFe.ResponseParser.Entidades;
using GoLive.NFe.SOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using GoLive.NFe.Leiaute;
namespace GoLive.NFe.Sefaz
{
    public class SefazCCe
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();
        /// <summary>
        /// Metodo para Carta de Correca de NF-e
        /// </summary>
        ///<param name="Ambiente">1-Produção | 2-Homologação</param>
        ///<param name="ChaveNFe">Chave da NF-e com 44 dígitos</param>
        ///<param name="Correcao">Correção a ser feita na NF-e</param>
        ///<param name="certificado">Certificado da Empresa Emissora Exemplo: X509Certificate2 certificado = NFeCertificadoDigital.getCertificate("My", Caminho_Certificado, Senha);</param>  
        ///<exception cref="GoLive.NFe.Sefaz.Exceptions.NFeException">Thrown when a non-numeric value is assigned.</exception>
        public RetEvento CartaDeCorrecao(int Ambiente, Eventos evento,  X509Certificate2 certificado, int Contingencia, int Modelo)
        {
            RetEvento eventoCCe = new RetEvento();
            string envioEvento = TSoap.soapXmlCCe(evento.ChaveNFe.Substring(0, 2), SefazEvento.EventoNFeCCe(Ambiente, evento.ChaveNFe, evento.xCorrecao, evento.nSeqEvento, certificado));

            string Resultado = string.Empty;

            switch (Ambiente)
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
                                if ((infEvento.Name) == "cStat") eventoCCe.cStat = int.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "xMotivo") eventoCCe.xMotivo = infEvento.InnerText;
                                if ((infEvento.Name) == "chNFe") eventoCCe.chNFe = infEvento.InnerText;
                                if ((infEvento.Name) == "tpEvento") eventoCCe.tpEvento = infEvento.InnerText;
                                if ((infEvento.Name) == "xEvento") eventoCCe.xEvento = infEvento.InnerText;
                                if ((infEvento.Name) == "nSeqEvento") eventoCCe.nSeqEvento = int.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "CNPJDest") eventoCCe.CNPJDest = infEvento.InnerText;
                                if ((infEvento.Name) == "dhRegEvento") eventoCCe.dhRegEvento = DateTime.Parse(infEvento.InnerText);
                                if ((infEvento.Name) == "nProt") eventoCCe.nProt = infEvento.InnerText;
                            }
                        }
                    }
                    else
                    {
                        eventoCCe.cStat = 999;
                        eventoCCe.xMotivo = "Erro no processamento do Evento";
                        eventoCCe.dhRegEvento = DateTime.Now;
                        eventoCCe.xEvento = "Carta de Correcao";
                        eventoCCe.chNFe = evento.ChaveNFe;
                        eventoCCe.nProt = "999999999999999";
                        eventoCCe.tpEvento = "110110";
                    }

                }
            }

            return eventoCCe;
        }
    }
}
