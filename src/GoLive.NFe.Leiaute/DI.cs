using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.NFe.Leiaute
{
   public sealed class DI
    {
       public string DI_nDI { get; set; }
       public DateTime DI_dDI { get; set; }
       public string DI_xLocDesemb { get; set; }
       public string DI_UFDesemb { get; set; }
       public DateTime DI_dDesemb { get; set; }
       public int DI_tpViaTransp { get; set; }
       public decimal DI_vAFRMM { get; set; }
       public int DI_tpIntermedio { get; set; }
       public string DI_CNPJ { get; set; }
       public string DI_UFTerceiro { get; set; }
       public string DI_cExportador { get; set; }
       public IEnumerable<Adi> DI_Adi { get; set; }
    }
}
