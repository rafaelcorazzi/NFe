using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace GoLive.NFe.Certificados
{
    public class NFeCertificadoDigital
    {
        // Definição de Store Location para pesquisa de certificados.
        private const String _CERTIFICATE_STORE = "My";

        // Retorno de erro.
        public static int intError = 0;


        public static bool GetExpirationDate(X509Certificate2 certificado)
        {
            bool expirado = false;
            try
            {

                if (DateTime.Now == DateTime.Parse(certificado.GetExpirationDateString())
                    || DateTime.Now > DateTime.Parse(certificado.GetExpirationDateString()))
                {
                    expirado = true;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Erro no certificado", ex.InnerException);
            }
            return expirado;
        }

        #region //////////////////////////////// HEX Manipulation OID Certificate //////////////////////////
        // funcao que retorna a Extension do Certificado otherName.
        private static string Get_Data_Hex_OID(string ASN1, string OID)
        {
            string[] items = ASN1.Split('\n');
            string ret;

            foreach (string item in items)
            {
                if (item.IndexOf(OID + "=") > 0)
                {
                    ret = item.Trim();
                    ret = item.Replace(OID + "=", "");
                    ret = ret.Trim();
                    return ret;
                }
            }

            return "";
        }

        private static string Data_Hex_Asc(ref string Data)
        {
            string Data1 = "";
            string sData = "";

            // remove espaços em branco, considerados no OID...
            Data = Data.Replace(" ", "");

            while (Data.Length > 0)
            //first take two hex value using substring.
            //then convert Hex value into ascii.
            //then convert ascii value into character.
            {
                Data1 = System.Convert.ToChar(System.Convert.ToUInt32(Data.Substring(0, 2), 16)).ToString();
                sData = sData + Data1;
                Data = Data.Substring(2, Data.Length - 2);
            }
            return sData;
        }

        private static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        private static string ByteArrayToStr(byte[] arrByte)
        {
            byte[] dBytes = arrByte;
            string str;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return str = enc.GetString(dBytes);
        }
        #endregion

        #region //////////////////////////////// findCertificate ////////////////////////////////
        /// <summary>
        /// private static X509Certificate2 findCertificate(String parStoreLocation, String parCNPJ)
        /// </summary>
        /// <param name="parStoreLocation"></param>
        /// <param name="parCertSubject"></param>
        /// <returns></returns>
        public static X509Certificate2 findCertificate(String parStoreLocation, String parCNPJ)
        {
            // Preparando para navegar pela Store de certificados.
            X509Store storeCertificates = new X509Store(parStoreLocation, StoreLocation.LocalMachine);
            storeCertificates.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            try
            {
                // Obtendo certificados da Store.
                X509Certificate2Collection certCollection = storeCertificates.Certificates;
                foreach (X509Certificate2 certItem in certCollection)
                {
                    if (certItem.Subject.Contains(parCNPJ))
                    {
                        // CNPJ encontrado no campo Subject do Certificado.
                        storeCertificates.Close();
                        return certItem;
                    }

                    // Create a new AsnEncodedDataCollection object.
                    AsnEncodedDataCollection asncoll = new AsnEncodedDataCollection();

                    foreach (X509Extension extension in certItem.Extensions)
                    {
                        //if (extension.Oid.FriendlyName.ToUpper() == "SUBJECT ALTERNATIVE NAME")
                        if (extension.Oid.Value == "2.5.29.17") //SUBJECT ALTERNATIVE NAME
                        {
                            // Create an AsnEncodedData object using the extensions information.
                            AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                            asncoll.Add(asndata);
                        }
                    }

                    if (asncoll.Count > 0)
                    {
                        //Create an enumerator for moving through the collection.
                        AsnEncodedDataEnumerator asne = asncoll.GetEnumerator();

                        //You must execute a MoveNext() to get to the first item in the collection.
                        asne.MoveNext();

                        // Assign the value to HEX string
                        string asn1_SAN = asne.Current.Format(true);

                        //Reset the collection.
                        asne.Reset();

                        //http://www.icpbrasil.gov.br/RES_ICP31.htm
                        /*
                        -OID =2.16.76.1.3.4 e conteúdo = nas primeiras 8 (oito) posições, 
                        a data de nascimento do responsável pelo certificado, 
                        no formato ddmmaaaa; nas (onze) posições subseqüentes,
                        o Cadastro de Pessoa Física (CPF) do responsável; 
                        nas 11 (onze) posições subseqüentes, 
                        o Número de Identificação Social-NIS (PIS,PASEP ou CI); 
                        nas (onze) posições subseqüentes, 
                        o número do RG do responsável;
                        nas 6 (seis) posições subseqüentes, 
                        as siglas do órgão expedidor do RG e respectiva UF; 
                        */
                        string oid_2_16_76_1_3_4 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.4");
                        oid_2_16_76_1_3_4 = Data_Hex_Asc(ref oid_2_16_76_1_3_4);

                        //OID =2.16.76.1.3.2 e conteúdo =nome do responsável 
                        //pelo certificado; 
                        string oid_2_16_76_1_3_2 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.2");
                        oid_2_16_76_1_3_2 = Data_Hex_Asc(ref oid_2_16_76_1_3_2);

                        //-OID =2.16.76.1.3.3 e conteúdo = Cadastro Nacional de Pessoa Jurídica (CNPJ) 
                        //da pessoa jurídica titular do certificado; 
                        string oid_2_16_76_1_3_3 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.3");
                        oid_2_16_76_1_3_3 = Data_Hex_Asc(ref oid_2_16_76_1_3_3);

                        //-OID =2.16.76.1.3.7 e conteúdo =nas 12 (doze)posições o número do Cadastro 
                        //Específico do INSS (CEI)da pessoa jurídica titular 
                        //do certificado. 
                        string oid_2_16_76_1_3_7 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.7");
                        oid_2_16_76_1_3_7 = Data_Hex_Asc(ref oid_2_16_76_1_3_7);

                        if (oid_2_16_76_1_3_3.Contains(parCNPJ))
                        {
                            // CNPJ encontrado no campo Subject Alternative Name (2.16.76.1.3.3) do Certificado.
                            storeCertificates.Close();
                            return certItem;
                        }
                    }
                }
            }
            finally
            {
                // Fechando o objeto de Store.
                storeCertificates.Close();
            }
            return null;
        }
        public static X509Certificate2 findCertificate(String parStoreLocation, String LocalFile, String Password)
        {
            X509Certificate2 cert2 = new X509Certificate2(LocalFile, Password);
            // Preparando para navegar pela Store de certificados.
            X509Store storeCertificates = new X509Store(parStoreLocation, StoreLocation.CurrentUser);

            storeCertificates.Open(OpenFlags.ReadWrite);
            storeCertificates.Add(cert2);
            try
            {
                //// Obtendo certificados da Store.
                //X509Certificate2Collection certCollection = storeCertificates.Certificates;
                //foreach (X509Certificate2 certItem in certCollection)
                //{
                //    //if (certItem.Subject.Contains(parCNPJ))
                //    //{
                //    //    // CNPJ encontrado no campo Subject do Certificado.
                //    //    storeCertificates.Close();
                //    //    return certItem;
                //    //}

                //    // Create a new AsnEncodedDataCollection object.
                //    AsnEncodedDataCollection asncoll = new AsnEncodedDataCollection();

                //    foreach (X509Extension extension in certItem.Extensions)
                //    {
                //        //if (extension.Oid.FriendlyName.ToUpper() == "SUBJECT ALTERNATIVE NAME")
                //        if (extension.Oid.Value == "2.5.29.17") //SUBJECT ALTERNATIVE NAME
                //        {
                //            // Create an AsnEncodedData object using the extensions information.
                //            AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                //            asncoll.Add(asndata);
                //        }
                //    }

                //    if (asncoll.Count > 0)
                //    {
                //        //Create an enumerator for moving through the collection.
                //        AsnEncodedDataEnumerator asne = asncoll.GetEnumerator();

                //        //You must execute a MoveNext() to get to the first item in the collection.
                //        asne.MoveNext();

                //        // Assign the value to HEX string
                //        string asn1_SAN = asne.Current.Format(true);

                //        //Reset the collection.
                //        asne.Reset();

                //        //http://www.icpbrasil.gov.br/RES_ICP31.htm
                //        /*
                //        -OID =2.16.76.1.3.4 e conteúdo = nas primeiras 8 (oito) posições, 
                //        a data de nascimento do responsável pelo certificado, 
                //        no formato ddmmaaaa; nas (onze) posições subseqüentes,
                //        o Cadastro de Pessoa Física (CPF) do responsável; 
                //        nas 11 (onze) posições subseqüentes, 
                //        o Número de Identificação Social-NIS (PIS,PASEP ou CI); 
                //        nas (onze) posições subseqüentes, 
                //        o número do RG do responsável;
                //        nas 6 (seis) posições subseqüentes, 
                //        as siglas do órgão expedidor do RG e respectiva UF; 
                //        */
                //        string oid_2_16_76_1_3_4 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.4");
                //        oid_2_16_76_1_3_4 = Data_Hex_Asc(ref oid_2_16_76_1_3_4);

                //        //OID =2.16.76.1.3.2 e conteúdo =nome do responsável 
                //        //pelo certificado; 
                //        string oid_2_16_76_1_3_2 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.2");
                //        oid_2_16_76_1_3_2 = Data_Hex_Asc(ref oid_2_16_76_1_3_2);

                //        //-OID =2.16.76.1.3.3 e conteúdo = Cadastro Nacional de Pessoa Jurídica (CNPJ) 
                //        //da pessoa jurídica titular do certificado; 
                //        string oid_2_16_76_1_3_3 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.3");
                //        oid_2_16_76_1_3_3 = Data_Hex_Asc(ref oid_2_16_76_1_3_3);

                //        //-OID =2.16.76.1.3.7 e conteúdo =nas 12 (doze)posições o número do Cadastro 
                //        //Específico do INSS (CEI)da pessoa jurídica titular 
                //        //do certificado. 
                //        string oid_2_16_76_1_3_7 = Get_Data_Hex_OID(asn1_SAN, "2.16.76.1.3.7");
                //        oid_2_16_76_1_3_7 = Data_Hex_Asc(ref oid_2_16_76_1_3_7);

                //        //if (oid_2_16_76_1_3_3.Contains(parCNPJ))
                //        //{
                //        //    // CNPJ encontrado no campo Subject Alternative Name (2.16.76.1.3.3) do Certificado.
                //        //    storeCertificates.Close();
                //        //    
                //        //}
                //        return certItem;
                //    }
               // }
                return cert2;
            }
            finally
            {
                // Fechando o objeto de Store.
                storeCertificates.Close();
            }
            return null;
        }
        #endregion


        #region //////////////////////////////// getCertificate ////////////////////////////////
        /// <summary>
        /// private static X509Certificate2 getCertificate(string storeLocation, string certificateSubject)
        /// </summary>
        /// <param name="storeLocation"></param>
        /// <param name="certificateSubject"></param>
        /// <returns></returns>
        public static X509Certificate2 getCertificate(string parStoreLocation, string parCertificateSubject)
        {
            // Declarando o objeto de certificado.
            X509Certificate2 objCertificate = null;

            // Validação de valores de entrada.
            if (parCertificateSubject == null)
            {
                //throw new ArgumentNullException("Error. Method: getCertificate - Param: parCertificateSubject");
                NFeCertificadoDigital.intError = 401;
            }
            if (parStoreLocation == null)
            {
                //throw new ArgumentNullException("Error. Method: getCertificate - Param: parStoreLocation");
                NFeCertificadoDigital.intError = 402;
            }

            // Pesquisando certificados pelo subject.
            objCertificate = findCertificate(parStoreLocation, parCertificateSubject);
            if (objCertificate == null)
            {
                //throw new CryptographicException("Error. The certificate could not be found.");
                NFeCertificadoDigital.intError = 501;
            }

            return objCertificate;
        }
        public static X509Certificate2 getCertificate(string parStoreLocation, string LocalFile, string Password)
        {
            // Declarando o objeto de certificado.
            X509Certificate2 objCertificate = null;

            // Validação de valores de entrada.
            if (LocalFile == null)
            {
                //throw new ArgumentNullException("Error. Method: getCertificate - Param: parCertificateSubject");
                NFeCertificadoDigital.intError = 401;
            }
            if (parStoreLocation == null)
            {
                //throw new ArgumentNullException("Error. Method: getCertificate - Param: parStoreLocation");
                NFeCertificadoDigital.intError = 402;
            }

            // Pesquisando certificados pelo subject.
            objCertificate = findCertificate(parStoreLocation, LocalFile, Password);
            if (objCertificate == null)
            {
                //throw new CryptographicException("Error. The certificate could not be found.");
                NFeCertificadoDigital.intError = 501;
            }

            return objCertificate;
        }
        #endregion


        #region //////////////////////////////// getCertificateKey ////////////////////////////////
        /// <summary>
        /// private static RSACryptoServiceProvider getCertificateKey(X509Certificate2 parCertificate)
        /// </summary>
        /// <param name="parCertificate"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider getCertificateKey(X509Certificate2 parCertificate)
        {
            // Objetos para tratamento da chave privada do certificado.
            CspParameters cspParameter = new CspParameters();
            RSACryptoServiceProvider rsaGetInfo = (RSACryptoServiceProvider)parCertificate.PrivateKey;

            // Parâmetros para definição da chave.
            cspParameter.KeyContainerName = rsaGetInfo.CspKeyContainerInfo.KeyContainerName;
            cspParameter.ProviderName = rsaGetInfo.CspKeyContainerInfo.ProviderName;
            cspParameter.ProviderType = rsaGetInfo.CspKeyContainerInfo.ProviderType;

            //An exchange key is an asymmetric key pair used to encrypt session keys so 
            //that they can be safely stored and exchanged with other users. 
            //You can use the Exchange value (1) to specify an exchange key. 
            //This value corresponds to the AT_KEYEXCHANGE value used in the unmanaged 
            //Microsoft Cryptographic API (CAPI). 

            //A signature key is an asymmetric key pair used for authenticating digitally 
            //signed messages or files. You can use the Signature value (2) to specify a 
            //signature key. This value corresponds to the AT_SIGNATURE value used in CAPI.

            // Verificar o tipo de certificado utilizado pela aplicação.
            // Uma vez indicado o valor errado no keyNumber, o servidor do SEFAZ
            // retorna o erro 242 - Falha no reconhecimento da autoria ou arquivo...
            //cspParameter.KeyNumber = (int)KeyNumber.Signature;
            cspParameter.KeyNumber = (int)rsaGetInfo.CspKeyContainerInfo.KeyNumber;

            // Retornando a chave privada obtida do certificado.
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(cspParameter);
            return key;
        }
        #endregion


        #region //////////////////////////////// SignXML ////////////////////////////////
        /// <summary>
        /// public static XmlDocument SignXML(XmlDocument document, string tagToSign, string idAttribute, string parCNPJ)
        /// </summary>
        /// <param name="document"></param>
        /// <param name="tagToSign"></param>
        /// <param name="idAttribute"></param>
        /// <param name="parCNPJ"></param>
        /// <returns></returns>
        public static XmlDocument SignXML(XmlDocument document, string tagToSign, string idAttribute, X509Certificate2 cert, out int parError)
        {
            bool checkSignatureXML;

            XmlDocument doc2 = new XmlDocument();
            doc2.PreserveWhitespace = true;
            doc2.LoadXml(document.OuterXml);
            document = doc2;

            // Obtendo o certificado a partir do CNPJ informado.
            //X509Certificate2 cert = getCertificate(_CERTIFICATE_STORE, parCNPJ);

            // Verificar o retorno de erro.
            if (NFeCertificadoDigital.intError.Equals(0))
            {
                // Percorre a estrutura da nota para assinar a TAG desejada.
                foreach (XmlNode node in document.GetElementsByTagName(tagToSign, "http://www.portalfiscal.inf.br/nfe"))
                {
                    //CspParameters cspParams = new CspParameters();
                    //cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";

                    //RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                    KeyInfo keyInfo = new KeyInfo();
                    keyInfo.AddClause(new KeyInfoX509Data(cert));

                    SignedXml signedXml = new SignedXml(document);
                    signedXml.KeyInfo = keyInfo;
                    signedXml.SigningKey = getCertificateKey(cert);

                    Reference reference = new Reference();
                    reference.Uri = "#" + (string)node.Attributes[idAttribute].Value;
                    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
                    reference.AddTransform(new XmlDsigC14NTransform(false));

                    signedXml.AddReference(reference);

                    // Realiza a assinatura da mensagem
                    signedXml.ComputeSignature();

                    // Verifica se a assinatura foi efetuada corretamente...
                    checkSignatureXML = signedXml.CheckSignature();

                    if (checkSignatureXML)
                    {
                        XmlElement xmlSignature = signedXml.GetXml();
                        XmlNode currentNode = xmlSignature;
                        XmlAttribute namespaceAtt = document.CreateAttribute("xmlns");
                        namespaceAtt.Value = "http://www.w3.org/2000/09/xmldsig#";
                        xmlSignature.Attributes.Append(namespaceAtt);

                        // Adiciona as TAGs de Signatur para a TAG parent associada.
                        node.ParentNode.AppendChild(document.ImportNode(xmlSignature, true));
                    }
                    else
                    {
                        NFeCertificadoDigital.intError = -999;
                    }
                }
            }

            parError = NFeCertificadoDigital.intError;
            return (document);
        }
        #endregion
    }
}
