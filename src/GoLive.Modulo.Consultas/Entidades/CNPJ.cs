using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.Modulo.Consultas.Entidades
{
    public class CNPJ
    {
        public string NumeroCNPJ { get; set; }
        public string TipoEmpresa { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNAEPrincipal { get; set; }
        public string AtividadePrincipal { get; set; }
        public DateTime dataAbertura { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string IBGEMunicipio { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string situacao { get; set; }
        public string Email { get; set; }
        public Decimal CapitalSocial {get;set;}
        public DateTime dataSituacao { get; set; }
        public string codNaturezaJuridica { get; set; }
        public string AtividadeJuridica { get; set; }
        public List<string> Telefones { get; set; }
        public Dictionary<string, string> AtividadesEconomicas { get; set; }
        public IEnumerable<SociosAdministradores> Sociedade { get; set; }
        public IEnumerable<RepresentantesLegais> Representantes { get; set; }
    }
}