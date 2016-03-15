using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class TSoap
    {
        public static String soapXmlStatusServico(string cUF, int ambiente)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeStatusServico2");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "3.10");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeStatusServico2");
                writer.WriteStartElement("consStatServ");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");
                writer.WriteElementString("tpAmb", ambiente.ToString());
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("xServ", "STATUS");
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlEnvioNFe(string cUF, string raw)
        {

            MemoryStream stream = new MemoryStream(); // The writer closes this for us
            StringBuilder result = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "3.10");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao");


                writer.WriteRaw(raw);


                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result.Append(reader.ReadToEnd());

                reader.Close();

            }

            return result.ToString();
        }
        public static String soapXmlReciboNFe(string cUF, int ambiente, string Recibo)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "3.10");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao");
                writer.WriteStartElement("consReciNFe");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");
                writer.WriteElementString("tpAmb", ambiente.ToString());
                writer.WriteElementString("nRec", Recibo);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlConsultaCadastro(int cUF, string UF, string CpfCnpj)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/CadConsultaCadastro2");
                writer.WriteElementString("cUF", cUF.ToString());
                writer.WriteElementString("versaoDados", "2.00");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/CadConsultaCadastro2");
                writer.WriteStartElement("ConsCad");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "2.00");
                writer.WriteStartElement("infCons");
                writer.WriteElementString("xServ", "CONS-CAD");
                writer.WriteElementString("UF", UF);
                if (CpfCnpj.Length == 14)
                    writer.WriteElementString("CNPJ", CpfCnpj);
                else if (CpfCnpj.Length == 11)
                    writer.WriteElementString("CPF", CpfCnpj);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlConsultaNFe(int ambiente, string ChaveNFe)
        {

            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2");
                writer.WriteElementString("cUF", ChaveNFe.Substring(0, 2));
                writer.WriteElementString("versaoDados", "3.10");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2");
                writer.WriteStartElement("consSitNFe");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");
                writer.WriteElementString("tpAmb", ambiente.ToString());
                writer.WriteElementString("xServ", "CONSULTAR");
                writer.WriteElementString("chNFe", ChaveNFe);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlCCe(string cUF, string raw)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "1.00");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento");

                writer.WriteRaw(raw);

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlCancelamento(string cUF, string raw)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "1.00");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento");

                writer.WriteRaw(raw);

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
        public static String soapXmlInutilizarNFe(string cUF, string raw)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {




                writer.WriteStartDocument();
                writer.WriteStartElement("soap:Envelope");
                writer.WriteAttributeString("xmlns:soap", "http://www.w3.org/2003/05/soap-envelope");
                writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                writer.WriteStartElement("soap:Header");
                writer.WriteStartElement("nfeCabecMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2");
                writer.WriteElementString("cUF", cUF);
                writer.WriteElementString("versaoDados", "3.10");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("soap:Body");

                writer.WriteStartElement("nfeDadosMsg");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe/wsdl/NfeInutilizacao2");

                writer.WriteRaw(raw);

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Flush();

                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
    }
}
