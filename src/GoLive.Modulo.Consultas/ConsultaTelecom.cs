using GoLive.Modulo.Consultas.Entidades;
using GoLive.Modulo.Consultas.Exceptions;
using GoLive.Modulo.Consultas.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace GoLive.Modulo.Consultas
{
     
    public class ConsultaTelecom
    {
        private string urlBase = "http://consultanumero.abr.net.br:8080/consultanumero/consulta/consultaSituacaoAtualCtg";
        private CookieContainer cookies = new CookieContainer();
      
        public String GetCaptcha()
        {

            var httpNFeCaptcha = (HttpWebRequest)WebRequest.Create(urlBase);
            httpNFeCaptcha.Timeout = 50000; //Timeout after 1000 ms
            httpNFeCaptcha.CookieContainer = cookies;
            httpNFeCaptcha.ContentType = "application/x-www-form-urlencoded";
            httpNFeCaptcha.Method = "GET";
            httpNFeCaptcha.UserAgent = "Mozilla/4.0 (compatible; Synapse)";
            HttpWebResponse responsePost = (HttpWebResponse)httpNFeCaptcha.GetResponse();

            string imagem = string.Empty;
            string jsession = string.Empty;
            Stream stream = responsePost.GetResponseStream();
            StreamReader result = new StreamReader(stream, System.Text.Encoding.UTF8);
            
            const string pattern = @"<img\b[^\<\>]+?\bsrc\s*=\s*[""'](?<L>.+?)[""'][^\<\>]*?\>";
            WebClient w = new WebClient();
            foreach (Match match in Regex.Matches(result.ReadToEnd(), pattern, RegexOptions.IgnoreCase))
            {
                var imageLink = match.Groups["L"].Value;
                if (imageLink.Contains("captcha"))
                {
                    using (var wc = new CookieAwareWebClient())
                    {


                        wc.CookieContainer = cookies;
                        wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                        wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                        
                        byte[] data = wc.DownloadData("http://consultanumero.abrtelecom.com.br" + imageLink);
                        imagem = base64Image.ImageTobase64(data);
                       jsession = imageLink.Replace("/consultanumero/jcaptcha.jpg?jcid=", "");

                    }



                }

            }
            return imagem + "|" + jsession;
        }
        private string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }
        public Operadora ConsultarOperadora(string Telefone, string Captcha, string Session)
        {
            Operadora operadora = new Operadora();

           
                if(String.IsNullOrEmpty(Telefone))
                {
                    throw new  ConsultasException("Insira um Número de Telefone");
                }
                else if(Telefone.Length > 11 && Telefone.Length  < 11)
                {
                    throw new ConsultasException("Formato de Numero Invalido");
                }
                else if(String.IsNullOrEmpty(Captcha))
                {
                    throw new ConsultasException("Captcha Incorreto");
                }
                else if(String.IsNullOrEmpty(Session))
                {
                    throw new ConsultasException("Informe dados da Sessao");
                }
                else
                {
                    Uri urlpost = new Uri(urlBase);
                    HttpWebRequest httpPostConsultaNFe = (HttpWebRequest)HttpWebRequest.Create("http://consultanumero.abrtelecom.com.br/consultanumero/consulta/executaConsultaSituacaoAtual");


                    string postConsultaComParametros = "telefone=" + Telefone + "&jcid=" + Session + "&jCaptchaValue=" + Captcha;
                    byte[] buffer2 = Encoding.ASCII.GetBytes(postConsultaComParametros);


                    using (var wc = new CookieAwareWebClient())
                    {
                        wc.CookieContainer = cookies;
                        wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                        wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                        wc.Headers[HttpRequestHeader.Cookie] = "JSESSIONID=" + Session + "; __utma=116930902.1920017025.1432573735.1432573735.1432573840.2; __utmz=116930902.1432573840.2.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided)";

                    }
                    //JSESSIONID=3B0BB581944AD5F01064C3029E593859; __utma=116930902.1920017025.1432573735.1432573735.1432573840.2; __utmz=116930902.1432573840.2.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided)
                    httpPostConsultaNFe.UserAgent = "Mozilla/4.0 (compatible; Synapse)";
                    httpPostConsultaNFe.CookieContainer = cookies;
                    httpPostConsultaNFe.Timeout = 500000;
                    httpPostConsultaNFe.ContentType = "application/x-www-form-urlencoded";
                    httpPostConsultaNFe.Method = "POST";
                    httpPostConsultaNFe.ContentLength = buffer2.Length;


                    Stream PostData = httpPostConsultaNFe.GetRequestStream();
                    PostData.Write(buffer2, 0, buffer2.Length);
                    PostData.Close();


                    HttpWebResponse responsePost = (HttpWebResponse)httpPostConsultaNFe.GetResponse();
                    Stream istreamPost = responsePost.GetResponseStream();
                    StreamReader strRespotaUrlConsultaNFe = new StreamReader(istreamPost, Encoding.Default);
                    string html = RemoverAcentos(strRespotaUrlConsultaNFe.ReadToEnd());

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                    doc.LoadHtml(html);

                    var output = doc.DocumentNode.SelectNodes("//table[@id='resultado']/tbody/tr/td");

                    Dictionary<int, string> dictionary =
                new Dictionary<int, string>();

                    int indice = 0;
                    foreach (var item in output)
                    {
                        indice++;
                        dictionary.Add(indice, item.InnerText);

                    }
                    if (dictionary.Count == 3)
                    {
                        DateTime dataPortabilidade = DateTime.Parse(dictionary[1]);
                        string strOperadora = dictionary[2];
                        string razao = dictionary[3];

                        operadora.dataPortabilidade = dataPortabilidade;
                        operadora.operadoraCelular = strOperadora.ToUpper();
                        operadora.RazaoSocial = razao.ToUpper();
                    }
                }
            
            
            return operadora;

        }
    }
}
