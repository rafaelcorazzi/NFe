using GoLive.Modulo.Consultas.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GoLive.Modulo.Consultas
{
    public class ContabilidadeCRC
    {
        public static readonly CookieContainer cookieContainer = new CookieContainer();
        public CRC ConsultaContabilidadeCRC(string Registro)
        {
            CRC crc = new CRC();
            if (String.IsNullOrEmpty(Registro))
            {
                throw new Consultas.Exceptions.ConsultasException("Por Favor informar o registro para consulta");

            }
            else
            {
                
                try
                {
                    Uri urlpost = new Uri("http://online.crcsp.org.br/visitantes/registro/consulta_registro.aspx?TP=1");
                    HttpWebRequest httpPostConsulta = (HttpWebRequest)HttpWebRequest.Create(urlpost);
                    //Xml que vai para o servidor do sinesp cidadao
                    StringBuilder postConsultaComParametros = new StringBuilder();
                    postConsultaComParametros.Append("__VIEWSTATE=%2FwEPDwUKMTU3ODk0MTE4Ng8WAh4IdGlwb19yZWcFATEWAgIDD2QWFAIBDw8WAh4EVGV4dAUqQ29uc3VsdGEgZGUgUHJvZmlzc2lvbmFpcyBkYSBDb250YWJpbGlkYWRlZGQCAw8PZBYCHglvbmtleWRvd24FpQFpZihldmVudC53aGljaCB8fCBldmVudC5rZXlDb2RlKXtpZiAoKGV2ZW50LndoaWNoID09IDEzKSB8fCAoZXZlbnQua2V5Q29kZSA9PSAxMykpIHtkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnaWJQZXNxUmVnJykuY2xpY2soKTtyZXR1cm4gZmFsc2U7fX0gZWxzZSB7cmV0dXJuIHRydWV9OyBkAgkPFgIeB1Zpc2libGVoZAILDxYCHwNoZAINDxYCHwNoZAIPDxYCHwNoZAITD2QWAmYPZBYCAgMPPCsACwBkAhUPFgIfA2hkAhcPFgIfA2gWAmYPZBYCAgEPPCsACwBkAhsPFgIfA2hkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQlpYlBlc3FSZWcWvqbmHkeDovdxpuKv7ALCnHnJZQ%3D%3D&__VIEWSTATEGENERATOR=631546EC&__EVENTVALIDATION=%2FwEdAAUKOozaYmLX%2FrPO8Xj5m6muBUq%2BxEzZkTkdtbYNBMPY2CNdXxAfSf3f5H%2BYEwqijE2Kd37G%2FXFWsS2E%2BwOvp1nw6sWPFtnFJoOoKZ0rSVbdRVVtp7SAuJfLJ6%2FHuDBCAgplcYH6&txtRegistro=" + Registro + "&ibPesqReg.x=9&ibPesqReg.y=9");




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
                    string html = strRespotaUrlConsultaNFe.ReadToEnd();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var trNome = doc.DocumentNode.SelectNodes("//tr[@id='trNome']/td/span[@id='txtNome']");
                    var trSituacao = doc.DocumentNode.SelectNodes("//tr[@id='trSituacao']/td/span[@id='txtSituacao']");
                    var trCategoria = doc.DocumentNode.SelectNodes("//tr[@id='trCategoria']/td/span[@id='txtCategoria']");
                    var trNaoHabilitado = doc.DocumentNode.SelectNodes("//tr[@id='trFinalidades']/td/table[@id='dgCertidoes']/tr/td");
                    var trMensagem = doc.DocumentNode.SelectNodes("//tr[@id='trMsg']/td/div/span[@id='lblMsg']");
                  
                    foreach (var item in trNome)
                        crc.Nome = item.InnerText;
                    foreach (var item in trSituacao)
                        crc.Situacao = item.InnerText;
                    foreach (var item in trCategoria)
                        crc.Categoria = RemoverAcentos(item.InnerText);
                    foreach (var item in trMensagem)
                    {
                        string[] mensagem = item.InnerText.Split(',');
                        if (mensagem.Length > 0)
                            crc.Mensagem = RemoverAcentos(mensagem[0].ToString());
                    }
                    int indice = 0;
                    List<string> servicos = new List<string>();
                    foreach (var item in trNaoHabilitado)
                    {
                        indice++;
                        if (indice > 1)
                        {
                            servicos.Add(item.InnerText.Replace("-", "").TrimStart());
                            crc.ServicosNaoHabilitados = servicos;
                        }

                    }
                }
                catch
                {
                    throw new Consultas.Exceptions.ConsultasException("Ocorreu algum erro na requisição da consulta!");
                }
            }
           
            return crc;
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
    }
}
