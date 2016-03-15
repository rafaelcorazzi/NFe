using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GoLive.NFe.SOAP.Exceptions;
using GoLive.NFe.RequestServices;
namespace GoLive.NFe.SOAP
{
    public class SefazProducao : ISefazOperation
    {

        public string NFeRetAutorizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeAutorizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeEvento(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeConsultaCadastro(string xml, int cUF, int contingencia, X509Certificate2 certificado)
        {
            throw new NotImplementedException();
        }

        public string NFeStatusServico(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeConsultaProtocolo(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeInutilizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modeloo)
        {
            throw new NotImplementedException();
        }

        public string NFeDownload(string xml, int cUF, int contingencia, X509Certificate2 certificado)
        {
            throw new NotImplementedException();
        }
    }
}
