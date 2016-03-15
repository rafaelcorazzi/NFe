using GoLive.NFe.RequestServices;
using GoLive.NFe.Certificados;
using GoLive.NFe.SOAP.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GoLive.NFe.SOAP
{
    public class SefazHomologacao : ISefazOperation
    {
        public string NFeRetAutorizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            string Retorno = string.Empty;
            try
            {
                if (certificado == null)
                    throw new SefazException("Certificado Inexistente");
                else if (Certificados.NFeCertificadoDigital.GetExpirationDate(certificado))
                {
                    throw new SefazException("Certificado Expirado não e possível comunicação com o Sefaz");
                }
                else
                {
                    string action = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao";

                    SefazWebRequest request = new SefazWebRequest();

                    if(Modelo == 55)
                    {
                        #region "Enderecos Servicos Padrao"
                        string am = Properties.Settings.Default.am_hom_NFeRetAutorizacao;
                        string ba = Properties.Settings.Default.ba_hom_NFeRetAutorizacao;
                        string ce = Properties.Settings.Default.ce_hom_NFeRetAutorizacao;
                        string go = Properties.Settings.Default.go_hom_NFeRetAutorizacao;
                        string mg = Properties.Settings.Default.mg_hom_NFeRetAutorizacao;
                        string mt = Properties.Settings.Default.mt_hom_NFeRetAutorizacao;
                        string ms = Properties.Settings.Default.ms_hom_NFeRetAutorizacao;
                        string pe = Properties.Settings.Default.pe_hom_NFeRetAutorizacao;
                        string pr = Properties.Settings.Default.pr_hom_NFeRetAutorizacao;
                        string rs = Properties.Settings.Default.rs_hom_NFeRetAutorizacao;
                        string sp = Properties.Settings.Default.sp_hom_NFeRetAutorizacao;
                        string svan = Properties.Settings.Default.svan_hom_NFeRetAutorizacao;
                        string svrs = Properties.Settings.Default.svrs_hom_NFeRetAutorizacao;

                        #endregion

                        #region "Enderecos Servicos de Contingencia"

                        string svc_an_retAutorizacao = Properties.Settings.Default.svcan_hom_NFeRetAutorizacao;
                        string svc_rs_retAutorizacao = Properties.Settings.Default.svcrs_hom_NFeRetAutorizacao;

                        #endregion

                        #region "UF AUTORIZADORAS"
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(am, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.BA:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ba, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.CE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ce, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.GO:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(go, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MG:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mg, xml, action, certificado);
                                }
                                break;

                            case (int)Estados.MS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ms, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mt, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pe, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pr, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.RS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(rs, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(sp, xml, action, certificado);
                                }
                                break;
                        }
                        #endregion

                        #region "SVRS - SEFAZ VIRTUAL RIO GRANDE DO SUL = AC, AL, AP, DF, ES, PB, RJ, RN, RO, RR, SC, SE, TO "

                        if (cUF == (int)Estados.AC ||
                            cUF == (int)Estados.AL ||
                            cUF == (int)Estados.AP ||
                            cUF == (int)Estados.DF ||
                            cUF == (int)Estados.ES ||
                            cUF == (int)Estados.PB ||
                            cUF == (int)Estados.RJ ||
                            cUF == (int)Estados.RN ||
                            cUF == (int)Estados.RO ||
                            cUF == (int)Estados.RR ||
                            cUF == (int)Estados.SC ||
                            cUF == (int)Estados.SE ||
                            cUF == (int)Estados.TO)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_an_retAutorizacao, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svrs, xml, action, certificado);
                            }
                        }


                        #endregion

                        #region "SEFAZ VIRTUAL AMBIENTE NACIONAL = MA, PA, PI"

                        if (cUF == (int)Estados.MA ||
                            cUF == (int)Estados.PA ||
                            cUF == (int)Estados.PI)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svan, xml, action, certificado);
                            }
                        }


                        #endregion
                    }
                    if (Modelo == 65)
                    {
                        string sp_nfce = Properties.Settings.Default.sp_hom_NFCeRetAutorizacao;
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if(contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    //Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(sp_nfce, xml, action, certificado);
                                }
                                break;
                        }

                        if (cUF == (int)Estados.AC ||
                         cUF == (int)Estados.AL ||
                         cUF == (int)Estados.AP ||
                         cUF == (int)Estados.DF ||
                         cUF == (int)Estados.ES ||
                         cUF == (int)Estados.PB ||
                         cUF == (int)Estados.RJ ||
                         cUF == (int)Estados.RN ||
                         cUF == (int)Estados.RO ||
                         cUF == (int)Estados.RR ||
                         cUF == (int)Estados.SC ||
                         cUF == (int)Estados.SE ||
                         cUF == (int)Estados.TO)
                        {

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new SefazException("Erro Retorno Autorização: " + ex.Message);
            }
            return Retorno;
        }

        public string NFeAutorizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int modelo)
        {
            string Retorno = string.Empty;
            try
            {
                if (certificado == null)
                    throw new SefazException("Certificado Inexistente");
                else if (Certificados.NFeCertificadoDigital.GetExpirationDate(certificado))
                {
                    throw new SefazException("Certificado Expirado não e possível comunicação com o Sefaz");
                }
                else
                {
                    SefazWebRequest request = new SefazWebRequest();
                    string action = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote";
                  
                    #region "NF-e mod=55"
                    if (modelo == 55)
                    {
                      


                        string am = Properties.Settings.Default.am_hom_NFeAutorizacao;
                        string ba = Properties.Settings.Default.ba_hom_NFeAutorizacao;
                        string ce = Properties.Settings.Default.ce_hom_NFeAutorizacao;
                        string go = Properties.Settings.Default.go_hom_NFeAutorizacao;
                        string mg = Properties.Settings.Default.mg_hom_NFeAutorizacao;
                        string mt = Properties.Settings.Default.mt_hom_NFeAutorizacao;
                        string ms = Properties.Settings.Default.ms_hom_NFeAutorizacao;
                        string pe = Properties.Settings.Default.pe_hom_NFeAutorizacao;
                        string pr = Properties.Settings.Default.pr_hom_NFeAutorizacao;
                        string rs = Properties.Settings.Default.rs_hom_NFeAutorizacao;
                        string sp = Properties.Settings.Default.sp_hom_NFeAutorizacao;
                        string svan = Properties.Settings.Default.svan_hom_NFeAutorizacao;
                        string svrs = Properties.Settings.Default.svrs_hom_NFeAutorizacao;


                        string svc_an_Autorizacao = Properties.Settings.Default.svcan_hom_NFeAutorizacao;
                        string svc_rs_Autorizacao = Properties.Settings.Default.svcrs_hom_NFeAutorizacao;

                        #region "UF AUTORIZADORAS"
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(am, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.BA:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ba, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.CE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ce, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.GO:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(go, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MG:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mg, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ms, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mt, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pe, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pr, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.RS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(rs, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_Autorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(sp, xml, action, certificado);
                                }
                                break;
                        }
                        #endregion

                        #region "SVRS - SEFAZ VIRTUAL RIO GRANDE DO SUL = AC, AL, AP, DF, ES, PB, RJ, RN, RO, RR, SC, SE, TO "

                        if (cUF == (int)Estados.AC ||
                            cUF == (int)Estados.AL ||
                            cUF == (int)Estados.AP ||
                            cUF == (int)Estados.DF ||
                            cUF == (int)Estados.ES ||
                            cUF == (int)Estados.PB ||
                            cUF == (int)Estados.RJ ||
                            cUF == (int)Estados.RN ||
                            cUF == (int)Estados.RO ||
                            cUF == (int)Estados.RR ||
                            cUF == (int)Estados.SC ||
                            cUF == (int)Estados.SE ||
                            cUF == (int)Estados.TO)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_an_Autorizacao, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svrs, xml, action, certificado);
                            }
                        }


                        #endregion

                        #region "SEFAZ VIRTUAL AMBIENTE NACIONAL = MA, PA, PI"

                        if (cUF == (int)Estados.MA ||
                           cUF == (int)Estados.PA ||
                           cUF == (int)Estados.PI)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_rs_Autorizacao, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svan, xml, action, certificado);
                            }
                        }


                        #endregion
                    }
                    #endregion

                    #region "NFC-e mod=65"

                    if(modelo == 65)
                    {
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    //Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    //Retorno = request.RequestWebService(sp_nfce, xml, action, certificado);
                                }
                                break;
                        }

                        if (cUF == (int)Estados.AC ||
                         cUF == (int)Estados.AL ||
                         cUF == (int)Estados.AP ||
                         cUF == (int)Estados.DF ||
                         cUF == (int)Estados.ES ||
                         cUF == (int)Estados.PB ||
                         cUF == (int)Estados.RJ ||
                         cUF == (int)Estados.RN ||
                         cUF == (int)Estados.RO ||
                         cUF == (int)Estados.RR ||
                         cUF == (int)Estados.SC ||
                         cUF == (int)Estados.SE ||
                         cUF == (int)Estados.TO)
                        {

                        }

                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                throw new SefazException("Erro Autorização: " + ex.Message);
            }
            return Retorno;
        }

        public string NFeEvento(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            string Retorno = string.Empty;
            try
            {
                if (certificado == null)
                    throw new SefazException("Certificado Inexistente");
                else if (Certificados.NFeCertificadoDigital.GetExpirationDate(certificado))
                {
                    throw new SefazException("Certificado Expirado não e possível comunicação com o Sefaz");
                }
                else
                {
                    if(Modelo == 55)
                    {
                        string action = "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento";
                        SefazWebRequest request = new SefazWebRequest();


                        string am = Properties.Settings.Default.am_hom_RecepcaoEvento;
                        string ba = Properties.Settings.Default.ba_hom_RecepcaoEvento;
                        string ce = Properties.Settings.Default.ce_hom_RecepcaoEvento;
                        string go = Properties.Settings.Default.go_hom_RecepcaoEvento;
                        string mg = Properties.Settings.Default.mg_hom_RecepcaoEvento;
                        string mt = Properties.Settings.Default.mt_hom_RecepcaoEvento;
                        string ms = Properties.Settings.Default.ms_hom_RecepcaoEvento;
                        string pe = Properties.Settings.Default.pe_hom_RecepcaoEvento;
                        string pr = Properties.Settings.Default.pr_hom_RecepcaoEvento;
                        string rs = Properties.Settings.Default.rs_hom_RecepcaoEvento;
                        string sp = Properties.Settings.Default.sp_hom_RecepcaoEvento;
                        string svan = Properties.Settings.Default.svan_hom_RecepcaoEvento;
                        string svrs = Properties.Settings.Default.svrs_hom_RecepcaoEvento;

                        string svc_an_RecepcaoEvento = Properties.Settings.Default.svcan_hom_RecepcaoEvento;
                        string svc_rs_RecepcaoEvento = Properties.Settings.Default.svcrs_hom_RecepcaoEvento;


                        #region "UF AUTORIZADORAS"
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(am, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.BA:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ba, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.CE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ce, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.GO:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(go, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MG:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mg, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ms, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mt, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PE:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pe, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pr, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.RS:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(rs, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    Retorno = request.RequestWebService(svc_an_RecepcaoEvento, xml, action, certificado);
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(sp, xml, action, certificado);
                                }
                                break;
                        }
                        #endregion

                        #region "SVRS - SEFAZ VIRTUAL RIO GRANDE DO SUL = AC, AL, AP, DF, ES, PB, RJ, RN, RO, RR, SC, SE, TO "

                        if (cUF == (int)Estados.AC ||
                            cUF == (int)Estados.AL ||
                            cUF == (int)Estados.AP ||
                            cUF == (int)Estados.DF ||
                            cUF == (int)Estados.ES ||
                            cUF == (int)Estados.PB ||
                            cUF == (int)Estados.RJ ||
                            cUF == (int)Estados.RN ||
                            cUF == (int)Estados.RO ||
                            cUF == (int)Estados.RR ||
                            cUF == (int)Estados.SC ||
                            cUF == (int)Estados.SE ||
                            cUF == (int)Estados.TO)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_an_RecepcaoEvento, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svrs, xml, action, certificado);
                            }
                        }


                        #endregion

                        #region "SEFAZ VIRTUAL AMBIENTE NACIONAL = MA, PA, PI"

                        if (cUF == (int)Estados.MA ||
                           cUF == (int)Estados.PA ||
                           cUF == (int)Estados.PI)
                        {
                            if (contingencia > 0)
                            {
                                Retorno = request.RequestWebService(svc_rs_RecepcaoEvento, xml, action, certificado);
                            }
                            else
                            {
                                Retorno = request.RequestWebService(svan, xml, action, certificado);
                            }
                        }


                        #endregion
                    }
                    if(Modelo == 65)
                    {
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    //Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                   // Retorno = request.RequestWebService(sp_nfce, xml, action, certificado);
                                }
                                break;
                        }

                        if (cUF == (int)Estados.AC ||
                         cUF == (int)Estados.AL ||
                         cUF == (int)Estados.AP ||
                         cUF == (int)Estados.DF ||
                         cUF == (int)Estados.ES ||
                         cUF == (int)Estados.PB ||
                         cUF == (int)Estados.RJ ||
                         cUF == (int)Estados.RN ||
                         cUF == (int)Estados.RO ||
                         cUF == (int)Estados.RR ||
                         cUF == (int)Estados.SC ||
                         cUF == (int)Estados.SE ||
                         cUF == (int)Estados.TO)
                        {

                        }

                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new SefazException("Erro Retorno Evento: " + ex.Message);
            }
            return Retorno;
        }

        public string NFeConsultaCadastro(string xml, int cUF, int contingencia, X509Certificate2 certificado)
        {
            string Retorno = string.Empty;
            try
            {
                if (certificado == null)
                    throw new SefazException("Certificado Inexistente");
                else if(Certificados.NFeCertificadoDigital.GetExpirationDate(certificado))
                {
                    throw new SefazException("Certificado Expirado não e possível comunicação com o Sefaz");
                }
                else
                {
                    string action = "http://www.portalfiscal.inf.br/nfe/wsdl/CadConsultaCadastro2";
                    SefazWebRequest request = new SefazWebRequest();



                    string am = Properties.Settings.Default.ma_hom_ConsultaCadastro;
                    string ba = Properties.Settings.Default.ba_hom_ConsultaCadastro;
                    string ce = Properties.Settings.Default.ce_hom_ConsultaCadastro;
                    string go = Properties.Settings.Default.go_hom_ConsultaCadastro;
                    string mg = Properties.Settings.Default.mg_hom_ConsultaCadastro;

                    string ms = Properties.Settings.Default.ms_hom_ConsultaCadastro;
                    string pe = Properties.Settings.Default.pe_hom_ConsultaProtocolo;
                    string pr = Properties.Settings.Default.pr_hom_ConsultaCadastro;
                    string rs = Properties.Settings.Default.rs_hom_ConsultaCadastro;
                    string sp = Properties.Settings.Default.sp_hom_ConsultaCadastro;
                    string ma = Properties.Settings.Default.ma_hom_ConsultaCadastro;
                    string svrs = Properties.Settings.Default.svrs_hom_ConsultaCadastro;

                    #region "UF AUTORIZADORAS"
                    switch (cUF)
                    {
                        case (int)Estados.AM:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(am, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.BA:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(ba, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.CE:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(ce, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.GO:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(go, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.MG:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(mg, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.MA:
                            if (contingencia > 0)
                            {

                            }
                            else
                            {
                                Retorno = request.RequestWebService(ma, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.MS:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(ms, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.MT:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                //Retorno = request.RequestWebService(mt, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.PE:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(pe, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.PR:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(pr, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.RS:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(rs, xml, action, certificado);
                            }
                            break;
                        case (int)Estados.SP:
                            if (contingencia > 0)
                            {
                                //Envio Emissao Previa de Entrada em contingencia
                            }
                            else
                            {
                                Retorno = request.RequestWebService(sp, xml, action, certificado);
                            }
                            break;
                    }
                    #endregion

                    #region "SVRS - SEFAZ VIRTUAL RIO GRANDE DO SUL = AC, AL, AP, DF, ES, PB, RJ, RN, RO, RR, SC, SE, TO "

                    if (cUF == (int)Estados.AC ||
                        cUF == (int)Estados.AL ||
                        cUF == (int)Estados.AP ||
                        cUF == (int)Estados.DF ||
                        cUF == (int)Estados.ES ||
                        cUF == (int)Estados.PB ||
                        cUF == (int)Estados.RJ ||
                        cUF == (int)Estados.RN ||
                        cUF == (int)Estados.RO ||
                        cUF == (int)Estados.RR ||
                        cUF == (int)Estados.SC ||
                        cUF == (int)Estados.SE ||
                        cUF == (int)Estados.TO)
                    {
                        if (contingencia > 0)
                        {

                        }
                        else
                        {
                            Retorno = request.RequestWebService(svrs, xml, action, certificado);
                        }
                    }


                    #endregion

                  
                }
            }
            catch (Exception ex)
            {
                throw new SefazException("Erro Retorno Consulta Cadastro: " + ex.Message);
            }
            return Retorno;
        }

        public string NFeStatusServico(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeConsultaProtocolo(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            string Retorno = string.Empty;
            try
            {
                if (certificado == null)
                    throw new SefazException("Certificado Inexistente");
                else if (Certificados.NFeCertificadoDigital.GetExpirationDate(certificado))
                {
                    throw new SefazException("Certificado Expirado não e possível comunicação com o Sefaz");
                }
                else
                {
                    string action = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2";
                    SefazWebRequest request = new SefazWebRequest();

                    if(Modelo == 55)
                    {
                        string am = Properties.Settings.Default.am_hom_ConsultaProtocolo;
                        string ba = Properties.Settings.Default.ba_hom_ConsultaProtocolo;
                        string ce = Properties.Settings.Default.ce_hom_ConsultaProtocolo;
                        string go = Properties.Settings.Default.go_hom_ConsultaProtocolo;
                        string mg = Properties.Settings.Default.mg_hom_ConsultaProtocolo;

                        string ms = Properties.Settings.Default.ms_hom_ConsultaProtocolo;
                        string pe = Properties.Settings.Default.pe_hom_ConsultaProtocolo;
                        string pr = Properties.Settings.Default.pr_hom_ConsultaProtocolo;
                        string rs = Properties.Settings.Default.rs_hom_ConsultaProtocolo;
                        string sp = Properties.Settings.Default.sp_hom_ConsultaProtocolo;

                        string svrs = Properties.Settings.Default.svrs_hom_ConsultaProtocolo;
                        string svan = Properties.Settings.Default.svan_hom_ConsultaProtocolo;


                        #region "UF AUTORIZADORAS"
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(am, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.BA:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ba, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.CE:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ce, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.GO:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(go, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MG:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(mg, xml, action, certificado);
                                }
                                break;

                            case (int)Estados.MS:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(ms, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    //Retorno = request.RequestWebService(mt, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PE:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pe, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(pr, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.RS:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(rs, xml, action, certificado);
                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    //Envio Emissao Previa de Entrada em contingencia
                                }
                                else
                                {
                                    Retorno = request.RequestWebService(sp, xml, action, certificado);
                                }
                                break;
                        }
                        #endregion

                        #region "SVRS - SEFAZ VIRTUAL RIO GRANDE DO SUL = AC, AL, AP, DF, ES, PB, RJ, RN, RO, RR, SC, SE, TO "

                        if (cUF == (int)Estados.AC ||
                            cUF == (int)Estados.AL ||
                            cUF == (int)Estados.AP ||
                            cUF == (int)Estados.DF ||
                            cUF == (int)Estados.ES ||
                            cUF == (int)Estados.PB ||
                            cUF == (int)Estados.RJ ||
                            cUF == (int)Estados.RN ||
                            cUF == (int)Estados.RO ||
                            cUF == (int)Estados.RR ||
                            cUF == (int)Estados.SC ||
                            cUF == (int)Estados.SE ||
                            cUF == (int)Estados.TO)
                        {
                            if (contingencia > 0)
                            {

                            }
                            else
                            {
                                Retorno = request.RequestWebService(svrs, xml, action, certificado);
                            }
                        }


                        #endregion

                        #region "SEFAZ VIRTUAL AMBIENTE NACIONAL = MA, PA, PI"

                        if (cUF == (int)Estados.MA ||
                            cUF == (int)Estados.PA ||
                            cUF == (int)Estados.PI)
                        {
                            if (contingencia > 0)
                            {

                            }
                            else
                            {
                                Retorno = request.RequestWebService(svan, xml, action, certificado);
                            }
                        }


                        #endregion


                    }

                    if(Modelo == 65)
                    {
                        switch (cUF)
                        {
                            case (int)Estados.AM:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.PR:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.MT:
                                if (contingencia > 0)
                                {

                                }
                                else
                                {

                                }
                                break;
                            case (int)Estados.SP:
                                if (contingencia > 0)
                                {
                                    //Retorno = request.RequestWebService(svc_rs_retAutorizacao, xml, action, certificado);
                                }
                                else
                                {
                                    //Retorno = request.RequestWebService(sp_nfce, xml, action, certificado);
                                }
                                break;
                        }

                        if (cUF == (int)Estados.AC ||
                         cUF == (int)Estados.AL ||
                         cUF == (int)Estados.AP ||
                         cUF == (int)Estados.DF ||
                         cUF == (int)Estados.ES ||
                         cUF == (int)Estados.PB ||
                         cUF == (int)Estados.RJ ||
                         cUF == (int)Estados.RN ||
                         cUF == (int)Estados.RO ||
                         cUF == (int)Estados.RR ||
                         cUF == (int)Estados.SC ||
                         cUF == (int)Estados.SE ||
                         cUF == (int)Estados.TO)
                        {

                        }
                    }

                 
                }
            }
            catch(Exception ex)
            {
                throw new SefazException("Erro Retorno Consulta Cadastro: " + ex.Message);
            }
            return Retorno;
        }

        public string NFeInutilizacao(string xml, int cUF, int contingencia, X509Certificate2 certificado, int Modelo)
        {
            throw new NotImplementedException();
        }

        public string NFeDownload(string xml, int cUF, int contingencia, X509Certificate2 certificado)
        {
            throw new NotImplementedException();
        }
    }
}
