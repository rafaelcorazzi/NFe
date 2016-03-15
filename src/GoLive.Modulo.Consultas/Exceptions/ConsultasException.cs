using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.Modulo.Consultas.Exceptions
{
    public class ConsultasException : Exception
    {
        public ConsultasException(string mensagem)
            : base(mensagem) { }
    }
}
