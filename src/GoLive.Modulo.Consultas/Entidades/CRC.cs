using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.Modulo.Consultas.Entidades
{
    public class CRC
    {
        public string Nome { get; set; }
        public string Situacao { get; set; }
        public string Categoria { get; set; }
        public string Mensagem { get; set; }
        public List<string> ServicosNaoHabilitados { get; set; }
    }
}
