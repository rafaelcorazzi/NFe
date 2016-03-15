using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class SefazInfProc
    {
        public static string ProcInut(String NFeInutilizacao, string protocoloAutorizacao)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {


                writer.WriteStartElement("procInutNFe");

                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");



                writer.WriteRaw(NFeInutilizacao);
                writer.WriteRaw(protocoloAutorizacao);

                writer.WriteEndElement();



                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }


            



            return result;
        }
        public static string ProcNFe(String NFe, string protocoloAutorizacao)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {


                writer.WriteStartElement("nfeProc");

                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "3.10");



                writer.WriteRaw(NFe);
                writer.WriteRaw(protocoloAutorizacao);

                writer.WriteEndElement();



                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

          

            return result;
        }
        public static string ProcEventoNFe(String Evento, string protocoloAutorizacao)
        {
            String result = String.Empty;
            MemoryStream stream = new MemoryStream(); // The writer closes this for us

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8))
            {


                writer.WriteStartElement("procEventoNFe");

                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");



                writer.WriteRaw(Evento);
                writer.WriteStartElement("retEvento");
                writer.WriteAttributeString("xmlns", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "1.00");
                writer.WriteRaw(protocoloAutorizacao);
                writer.WriteEndElement();
                writer.WriteEndElement();



                writer.Flush();


                StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
                stream.Seek(0, SeekOrigin.Begin);

                result += reader.ReadToEnd();


            }

            return result;
        }
    }
}
