using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using GoLive.NFe.Sefaz.Exceptions;

namespace GoLive.NFe.Sefaz
{
    public class SefazEvento
    {
        public static String EventoNFeCancelamento(int Ambiente, string ChaveNFe, string numeroProtocolo, string Justificativa, X509Certificate2 certificado)
        {
            String result = String.Empty;

            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                writer.WriteStartElement("envEvento");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteElementString("idLote", String.Format("{0:ddMMyyyyHHmmssf}", DateTime.Now));

                writer.WriteStartElement("evento");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteStartElement("infEvento");
                writer.WriteAttributeString("Id", "ID110111" + ChaveNFe + "01");
                writer.WriteElementString("cOrgao", ChaveNFe.Substring(0, 2));
                writer.WriteElementString("tpAmb", Ambiente.ToString());
                writer.WriteElementString("CNPJ", ChaveNFe.Substring(6 ,14));
                writer.WriteElementString("chNFe", ChaveNFe);
                writer.WriteElementString("dhEvento", String.Format("{0:yyyy-MM-ddTHH:mm:sszzz}", DateTime.Now));
                writer.WriteElementString("tpEvento", "110111");
                writer.WriteElementString("nSeqEvento", "1");
                writer.WriteElementString("verEvento", "1.00");

                writer.WriteStartElement("detEvento");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteElementString("descEvento", "Cancelamento");
                writer.WriteElementString("nProt", numeroProtocolo);
                writer.WriteElementString("xJust", Justificativa);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteEndElement();




                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }
            XmlDocument xmlOutputSign = new XmlDocument();
            int intError = 0;

            //carrega o documento xml com os dados da nfe sem assinatura
            XmlDocument docSefaz = new XmlDocument();
            docSefaz.LoadXml(result);
            //logTexto("Assinando XML para enviar ao webservice - ", "PASSO - 17");
            //procura pela tag infNFe no xml e insere uma tag chamada Id e assina a NFe colocanod a tag
            //<signature></signature>
            xmlOutputSign = GoLive.NFe.Certificados.NFeCertificadoDigital.SignXML(docSefaz, "infEvento", "Id", certificado, out intError);



            if (intError > 0)
            {
                throw new SefazException("Erro ao assinar xml.Verifique o certificado digital");
            }
            return xmlOutputSign.OuterXml;
        
           
        }
        public static String EventoNFeCCe(int Ambiente, string ChaveNFe,  string Correcao, int nSeqEvento, X509Certificate2 certificado)
        {
            String result = String.Empty;

            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {
                string sequencia =  String.Format("{0:00}", nSeqEvento);
                writer.WriteStartElement("envEvento");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteElementString("idLote", String.Format("{0:ddMMyyyyHHmmssf}", DateTime.Now));

                writer.WriteStartElement("evento");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteStartElement("infEvento");
                writer.WriteAttributeString("Id", "ID110110" + ChaveNFe + sequencia);
                writer.WriteElementString("cOrgao", ChaveNFe.Substring(0, 2));
                writer.WriteElementString("tpAmb", Ambiente.ToString());
                writer.WriteElementString("CNPJ", ChaveNFe.Substring(6, 14));
                writer.WriteElementString("chNFe", ChaveNFe);
                writer.WriteElementString("dhEvento", String.Format("{0:yyyy-MM-ddTHH:mm:sszzz}", DateTime.Now));
                writer.WriteElementString("tpEvento", "110110");
                writer.WriteElementString("nSeqEvento", sequencia);
                writer.WriteElementString("verEvento", "1.00");

                writer.WriteStartElement("detEvento");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteElementString("descEvento", "Carta de Correcao");
                writer.WriteElementString("xCorrecao", Correcao);
                writer.WriteElementString("xCondUso", "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido na emissao de documento fiscal, desde que o erro nao esteja relacionado com: I - as variaveis que determinam o valor do imposto tais como: base de calculo, aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; III - a data de emissao ou de saida.");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteEndElement();




                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }
            XmlDocument xmlOutputSign = new XmlDocument();
            int intError = 0;

            //carrega o documento xml com os dados da nfe sem assinatura
            XmlDocument docSefaz = new XmlDocument();
            docSefaz.LoadXml(result);
            //logTexto("Assinando XML para enviar ao webservice - ", "PASSO - 17");
            //procura pela tag infNFe no xml e insere uma tag chamada Id e assina a NFe colocanod a tag
            //<signature></signature>
            xmlOutputSign = GoLive.NFe.Certificados.NFeCertificadoDigital.SignXML(docSefaz, "infEvento", "Id", certificado, out intError);



            if (intError > 0)
            {
                throw new SefazException("Erro ao assinar xml.Verifique o certificado digital");
            }
            return xmlOutputSign.OuterXml;


        }
        public static String EventoInutilizar(int Ambiente, int cUF, String ChaveInutilizacao, DateTime DataEvento, int ModeloNF,string CnpjEmpresa,  int Serie, int NFIni, int NFFin, String Justificativa, X509Certificate2 certificado)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {


                writer.WriteStartElement("inutNFe");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");


                writer.WriteStartElement("infInut");
                writer.WriteAttributeString("Id", "ID" + ChaveInutilizacao);
                writer.WriteElementString("tpAmb", Ambiente.ToString());
                writer.WriteElementString("xServ", "INUTILIZAR");
                writer.WriteElementString("cUF", cUF.ToString());
                writer.WriteElementString("ano", String.Format("{0:yy}", DataEvento));
                writer.WriteElementString("CNPJ", CnpjEmpresa);
                writer.WriteElementString("mod", ModeloNF.ToString());
                writer.WriteElementString("serie", Serie.ToString());
                writer.WriteElementString("nNFIni", NFIni.ToString());
                writer.WriteElementString("nNFFin", NFFin.ToString());

                writer.WriteElementString("xJust", Justificativa);
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }
            XmlDocument xmlOutputSign = new XmlDocument();
            int intError = 0;

            //carrega o documento xml com os dados da nfe sem assinatura
            XmlDocument docSefaz = new XmlDocument();
            docSefaz.LoadXml(result);
            //logTexto("Assinando XML para enviar ao webservice - ", "PASSO - 17");
            //procura pela tag infNFe no xml e insere uma tag chamada Id e assina a NFe colocanod a tag
            //<signature></signature>
            xmlOutputSign = Certificados.NFeCertificadoDigital.SignXML(docSefaz, "infInut", "Id", certificado, out intError);



            if (intError > 0)
            {
                throw new SefazException("Erro ao Assinar Xml");
            }
            return xmlOutputSign.OuterXml;
        }
    }
}
