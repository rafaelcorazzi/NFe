using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GoLive.NFe.Mail
{
    public class SendEmail
    {
        public void EnviarEmail(string Smtp, int Port, string usuario, string senha, string Assunto, string Corpo, string From, string To, bool SSL)
        {
            MailMessage mMailMessage = new MailMessage();
            mMailMessage.From = new MailAddress(From);
            if (To.Length > 0)
            {
                //AppLog(CC, "DESTINATARIO", "CCe");
                mMailMessage.To.Add(To);
            }
            mMailMessage.Subject = Assunto;
            mMailMessage.Body = Corpo;

            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.High;

            SmtpClient smtpCliente = new SmtpClient();
            smtpCliente.Host = Smtp;
            smtpCliente.EnableSsl = SSL;
            smtpCliente.Port = Port;


            if (!String.IsNullOrEmpty(usuario) && !String.IsNullOrEmpty(senha))
                smtpCliente.Credentials = new NetworkCredential(usuario, senha);

            smtpCliente.Send(mMailMessage);

        }
        public void EnviarEmail(string Smtp, int Port, string usuario, string senha, string Assunto, string Corpo, string From, string To, bool SSL, String AnexoProcNFe)
        {
            MailMessage mMailMessage = new MailMessage();
            mMailMessage.From = new MailAddress(From);
            if (To.Length > 0)
            {
                //AppLog(CC, "DESTINATARIO", "CCe");
                mMailMessage.To.Add(To);
            }
            mMailMessage.Subject = Assunto;
            mMailMessage.Body = Corpo;

            if (File.Exists(AnexoProcNFe))
            {
                Attachment attcProcNFe = new Attachment(AnexoProcNFe);
                mMailMessage.Attachments.Add(attcProcNFe);
            }


            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.High;

            SmtpClient smtpCliente = new SmtpClient();
            smtpCliente.Host = Smtp;
            smtpCliente.EnableSsl = SSL;
            smtpCliente.Port = Port;


            if (!String.IsNullOrEmpty(usuario) && !String.IsNullOrEmpty(senha))
                smtpCliente.Credentials = new NetworkCredential(usuario, senha);

            smtpCliente.Send(mMailMessage);
        }
        public void EnviarEmail(string Smtp, int Port, string usuario, string senha, string Assunto, string Corpo, string From, string To, bool SSL, String AnexoProcNFe, String AnexoPDF)
        {
            MailMessage mMailMessage = new MailMessage();
            mMailMessage.From = new MailAddress(From);
            if (To.Length > 0)
            {
                //AppLog(CC, "DESTINATARIO", "CCe");
                mMailMessage.To.Add(To);
            }
            mMailMessage.Subject = Assunto;
            mMailMessage.Body = Corpo;

            if (File.Exists(AnexoProcNFe))
            {
                Attachment attcProcNFe = new Attachment(AnexoProcNFe);
                mMailMessage.Attachments.Add(attcProcNFe);
            }

            if (File.Exists(AnexoPDF))
            {
                Attachment attcPdf = new Attachment(AnexoPDF);
                mMailMessage.Attachments.Add(attcPdf);
            }

            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.High;

            SmtpClient smtpCliente = new SmtpClient();
            smtpCliente.Host = Smtp;
            smtpCliente.EnableSsl = SSL;
            smtpCliente.Port = Port;


            if (!String.IsNullOrEmpty(usuario) && !String.IsNullOrEmpty(senha))
                smtpCliente.Credentials = new NetworkCredential(usuario, senha);

            smtpCliente.Send(mMailMessage);
        }
    }
}
