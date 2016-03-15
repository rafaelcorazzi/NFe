using GoLive.Modulo.Consultas.Entidades;
using GoLive.Modulo.Consultas.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace GoLive.Modulo.Consultas
{
    public class ConsultaCPF
    {
        public readonly CookieContainer cookieContainer = new CookieContainer();
        private string urlBase = "http://www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/ConsultaPublica.asp";
        private string urlCaptcha = "http://www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/captcha/gerarCaptcha.asp";
        private string urlPostConsulta = "http://www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/ConsultaPublicaExibir.asp";

        public Image GetCaptcha()
        {
            string htmlResult = string.Empty;
            using (var wc = new CookieAwareWebClient())
            {
                wc.CookieContainer = this.cookieContainer;
                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                htmlResult = wc.DownloadString(this.urlBase);
            }
            if (htmlResult != string.Empty)
            {
                using (var wc = new CookieAwareWebClient())
                {
                    wc.CookieContainer = this.cookieContainer;
                    wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                    wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                    byte[] data = wc.DownloadData(this.urlCaptcha);
                    return Image.FromStream(new MemoryStream(data));
                }
            }
            return null;

        }
        private string[] GetArrayData(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string[] result;
            doc.LoadHtml(html);
            HtmlNodeCollection itens = doc.DocumentNode.SelectNodes("//span[@class='clConteudoDados']");
            if (itens != null)
            {
                result = new string[itens.Count];
            }
            else
            {
                result = new string[0];
            }
            for (int i = 0; i < result.Length; i++)
            {
                string elem = itens[i].InnerText;
                int startIndex = elem.IndexOf(":");
                result[i] = elem.Substring(startIndex + 1);
            }
            return result;
        }
        public CPF ObtemSituacaoCadastral(string cpf, string captcha)
        {
            CPF retorno = new CPF();

            string parametros = "txtCPF=" + HttpUtility.UrlEncode(cpf) + "&txtTexto_captcha_serpro_gov_br=" + HttpUtility.UrlEncode(captcha) + "&Enviar=Consultar";
            byte[] byteArray = Encoding.UTF8.GetBytes(parametros);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.urlPostConsulta);
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.UserAgent = "Mozilla/4.0 (compatible; Synapse)";
            request.ContentLength = parametros.Length;
            Stream sw = request.GetRequestStream();
            sw.Write(byteArray, 0, byteArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string htmlText = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
            {
                htmlText = sr.ReadToEnd();
            }

           
            string[] arrConsulta = GetArrayData(htmlText);
            if (arrConsulta.Length >= 4)
            {
                retorno.Documento = arrConsulta[0];
                retorno.Nome = arrConsulta[1];
                retorno.SituacaoCadastral = arrConsulta[2];
              
            }
            return retorno;
        }
        private string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
        public CPF ObtemSituacaoCadastral(string cpf, DateTime DataNascimento)
        {
            CPF _cpf = new CPF();
            Uri urlpost = new Uri("https://movel01.receita.fazenda.gov.br/servicos-rfb/v2/IRPF/cpf");
            HttpWebRequest httpPostConsulta = (HttpWebRequest)HttpWebRequest.Create(urlpost);
            string key = "Sup3RbP4ssCr1t0grPhABr4sil";
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);
            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(cpf + String.Format("{0:ddMMyyyy}", DataNascimento));
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string hmac2 = ByteToString(hashmessage);

            httpPostConsulta.Headers["plataforma"] = "iPhone OS";
            httpPostConsulta.Headers["dispositivo"] = "iPhone";
            httpPostConsulta.Headers["aplicativo"] = "Pessoa Física";
            httpPostConsulta.Headers["versao"] = "8.3";
            httpPostConsulta.Headers["token"] = hmac2.ToLower();
            httpPostConsulta.Headers["versao_app"] = "4.1";
       
            httpPostConsulta.Accept = "*/*";
         

            //Xml que vai para o servidor do sinesp cidadao
            StringBuilder postConsultaComParametros = new StringBuilder();
            postConsultaComParametros.Append("cpf=" + cpf + "&dataNascimento=" + String.Format("{0:ddMMyyyy}", DataNascimento));
           



            byte[] buffer2 = Encoding.ASCII.GetBytes(postConsultaComParametros.ToString());
            httpPostConsulta.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_0 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9A334 Safari/7534.48.3";

            httpPostConsulta.CookieContainer = cookieContainer;
            httpPostConsulta.Timeout = 900000;
            httpPostConsulta.ContentType = "application/x-www-form-urlencoded";
            httpPostConsulta.Method = "POST";
            httpPostConsulta.ContentLength = buffer2.Length;


            Stream PostData = httpPostConsulta.GetRequestStream();
            PostData.Write(buffer2, 0, buffer2.Length);
            PostData.Close();




            HttpWebResponse responsePost = (HttpWebResponse)httpPostConsulta.GetResponse();
            Stream istreamPost = responsePost.GetResponseStream();
            StreamReader strRespotaUrlConsultaNFe = new StreamReader(istreamPost, Encoding.Default);
           string retorno = strRespotaUrlConsultaNFe.ReadToEnd();


           Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(retorno);

           _cpf.Resultado = (string)jObject["mensagemRetorno"];
           _cpf.SituacaoCadastral = (string)jObject["descSituacaoCadastral"];
           _cpf.Nome = (string)jObject["nome"];
           _cpf.Documento = (string)jObject["cpf"];
           _cpf.AnoObito = (string)jObject["anoObito"];
            return _cpf;
        }

    }
}
