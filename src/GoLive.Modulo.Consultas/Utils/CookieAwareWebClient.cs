using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GoLive.Modulo.Consultas.Utils
{
    internal class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }
        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                this.CookieContainer.SetCookies(response.ResponseUri, setCookieHeader);
            }
            return response;
        }
    }
}
