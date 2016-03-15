using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Certificados
{
    public sealed class NFeUtils
    {
        public static string CalculaDigitoMod11(string Dado, int NumDig, int LimMult)
        {
            int Mult, Soma, i, n;

            for (n = 1; n <= NumDig; n++)
            {
                Soma = 0;
                Mult = 2;
                for (i = Dado.Length - 1; i >= 0; i--)
                {
                    Soma += (Mult * int.Parse(Dado.Substring(i, 1)));
                    if (++Mult > LimMult) Mult = 2;
                }
                Dado += ((Soma * 10) % 11) % 10;
            }
            return Dado.Substring(Dado.Length - NumDig, NumDig);
        }

        public static string digitoVerificadorMod11(string chave)
        {
            string pesos = "98765432";
            string dv = "";

            int soma = 0, resto = 0, nCadeias;

            Char[] APesos = new char[43];

            Char[] AIdNota = new char[43];
            nCadeias = ((chave.Length) / 8) + 1;

            for (int i = 0; i < nCadeias; i++)
            {
                pesos += pesos;
            }

            pesos = pesos.Substring((nCadeias - 1), 43);
            APesos = pesos.ToCharArray();
            AIdNota = chave.ToCharArray();

            for (int i = 0; i < 43; i++)
            {
                soma += charToInt(APesos[i]) * charToInt(AIdNota[i]);
            }

            resto = soma % 11;

            if (resto < 2)
                return "0";

            dv = Convert.ToString(11 - resto);
            return dv;
        }

        private static int charToInt(Char value)
        {
            return Convert.ToInt32(value) - 48;
        }
    }
}
