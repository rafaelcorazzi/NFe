using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Sefaz.Exceptions
{
    public class SefazException : Exception
    {
       
        public SefazException(string mensagem)
            : base(mensagem) { }
        
    }
}
