using GoLive.NFe.ResponseParser.Entidades;
using GoLive.NFe.SOAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GoLive.NFe.Sefaz
{
    public class SefazConsultaCadastro
    {
        SefazHomologacao homNFe = new SefazHomologacao();
        SefazProducao prodNFe = new SefazProducao();

        public RetConsCad ConsultarCadastro(int Ambiente, String CNPJ, string UF, int cUF, int Contingencia, X509Certificate2 certificado)
        {
            RetConsCad cadastro = new RetConsCad();

            string consultar = TSoap.soapXmlConsultaCadastro(cUF,UF,  CNPJ);

            string Resultado = string.Empty;

            switch (Ambiente)
            {
                case 1:
                    Resultado = prodNFe.NFeConsultaCadastro(consultar, cUF, Contingencia, certificado);
                    break;
                case 2:
                    Resultado = homNFe.NFeConsultaCadastro(consultar,cUF, Contingencia, certificado);
                    break;
            }

            //DANFe
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Resultado);


            XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
            namespaces.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
            XmlNodeList consCad = doc.SelectNodes("descendant::nfe:retConsCad/nfe:infCons", namespaces);
            foreach (XmlNode cons in consCad)
            {
                foreach (XmlNode infCons in cons)
                {
                    if ((infCons.Name) == "cStat") cadastro.cStat = int.Parse(infCons.InnerText);

                    if (infCons.Name == "infCad")
                    {
                        foreach (XmlNode infCad in infCons)
                        {

                            if ((infCad.Name) == "IE") cadastro.infCad_IE = infCad.InnerText;
                            if ((infCad.Name) == "CNPJ") cadastro.infCad_CNPJ = infCad.InnerText;
                            if ((infCad.Name) == "cSit") cadastro.infCad_cSit = int.Parse(infCad.InnerText);
                            if ((infCad.Name) == "UF") cadastro.infCad_UF = infCad.InnerText;
                            if ((infCad.Name) == "indCredNFe") cadastro.infCad_indCredNFe = int.Parse(infCad.InnerText);
                            if ((infCad.Name) == "indCredCTe") cadastro.infCad_indCredCTe = int.Parse(infCad.InnerText);
                            if ((infCad.Name) == "xNome") cadastro.infCad_xNome = infCad.InnerText;
                            if ((infCad.Name) == "xRegApur") cadastro.infCad_xRegApur = infCad.InnerText;
                            if ((infCad.Name) == "CNAE") cadastro.infCad_CNAE = infCad.InnerText;
                            if ((infCad.Name) == "dIniAtiv") cadastro.infCad_dIniAtiv = DateTime.Parse(infCad.InnerText);
                            if ((infCad.Name) == "dUltSit") cadastro.infCad_dUltSit = DateTime.Parse(infCad.InnerText);

                            if (infCad.Name == "ender")
                            {
                                foreach (XmlNode ender in infCad)
                                {
                                    if ((ender.Name) == "xLgr") cadastro.infCad_xLgr = ender.InnerText;
                                    if ((ender.Name) == "nro") cadastro.infCad_nro = ender.InnerText;
                                    if ((ender.Name) == "xBairro") cadastro.infCad_xBairro = ender.InnerText;
                                    if ((ender.Name) == "cMun") cadastro.infCad_cMun = ender.InnerText;
                                    if ((ender.Name) == "xMun") cadastro.infCad_xMun = ender.InnerText;
                                    if ((ender.Name) == "CEP") cadastro.infCad_CEP = ender.InnerText;
                                }

                            }



                        }

                    }

                }
            }
            return cadastro;
        }
    }
}
