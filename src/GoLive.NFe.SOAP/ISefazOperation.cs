using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GoLive.NFe.SOAP
{
    public interface ISefazOperation
    {
        string NFeRetAutorizacao(String xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo);
        string NFeAutorizacao(String xml, int cUF, int contingencia, X509Certificate2 certificado, int modelo);
        string NFeEvento(String xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo);
        string NFeConsultaCadastro(String xml, int cUF, int contingencia, X509Certificate2 certificado);
        string NFeStatusServico(String xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo);
        string NFeConsultaProtocolo(String xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo);
        string NFeInutilizacao(String xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo);
        string NFeDownload(String xml, int cUF, int contingencia, X509Certificate2 certificado);
      
    }
}
