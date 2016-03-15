using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    public sealed class medicamento
    {
        public string med_nLote { get; set; }
        public decimal med_qLote { get; set; }
        public DateTime med_dFab { get; set; }
        public DateTime med_dVal { get; set; }
        public decimal med_vPMC { get; set; }
    }
}
