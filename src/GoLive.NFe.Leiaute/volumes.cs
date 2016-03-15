using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
    public sealed class volumes
    {
        public string vol_qVol { get; set; }
        public string vol_esp { get; set; }
        public string vol_marca { get; set; }
        public string vol_nVol { get; set; }
        public decimal vol_pesoL { get; set; }
        public decimal vol_pesoB { get; set; }

        public IEnumerable<lacres> vol_Lacres { get; set; }

    }
}
