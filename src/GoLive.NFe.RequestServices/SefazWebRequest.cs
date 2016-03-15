using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace GoLive.NFe.RequestServices
{
    public class SefazWebRequest
    {
        private CookieContainer cookies = new CookieContainer();
        public string RequestWebService(string wsURL, string param, string action, X509Certificate2 certificado)
        {

            Uri urlpost = new Uri(wsURL);
            HttpWebRequest httpPostConsultaNFe = (HttpWebRequest)HttpWebRequest.Create(urlpost);

            string postConsultaComParametros = param;
            byte[] buffer2 = Encoding.ASCII.GetBytes(postConsultaComParametros);

            httpPostConsultaNFe.CookieContainer = cookies;
            httpPostConsultaNFe.Timeout = 300000;
            httpPostConsultaNFe.ContentType = "application/soap+xml; charset=utf-8; action=" + action;
            httpPostConsultaNFe.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            httpPostConsultaNFe.ProtocolVersion = HttpVersion.Version10;
            httpPostConsultaNFe.ClientCertificates.Add(certificado);
            httpPostConsultaNFe.ContentLength = buffer2.Length;

            

            Stream PostData = httpPostConsultaNFe.GetRequestStream();
            PostData.Write(buffer2, 0, buffer2.Length);
            PostData.Close();

            HttpWebResponse responsePost = (HttpWebResponse)httpPostConsultaNFe.GetResponse();
            Stream istreamPost = responsePost.GetResponseStream();
            StreamReader strRespotaUrlConsultaNFe = new StreamReader(istreamPost, System.Text.Encoding.UTF8);

            return NormalizarStrings.RemoverAcentos(strRespotaUrlConsultaNFe.ReadToEnd());


        }
       
    }
}
