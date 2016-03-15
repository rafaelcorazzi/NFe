using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GoLive.NFe.RequestServices
{
    public class NormalizarStrings
    {
        public static string RemoverAcentos(string texto)
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
        public static string CaracteresEspeciais(string sChar)
        {
            string aspas = "\"\"";
            aspas = aspas.Substring(0, 1);
            string sSaida = sChar.Replace("&", "&amp;").Replace(aspas, "&quot;").Replace("'", "&#39;").Replace("<", "&lt;").Replace(">", "&gt;");

            return sSaida;
        }
    }
}
