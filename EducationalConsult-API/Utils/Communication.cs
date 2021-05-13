using EducationalConsultAPI.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System;

namespace EducationalConsultAPI.Utils {
    public class Communication : ICommunication {
        public Communication(IOptions<Secret> appSettings) {
            _appSecret = appSettings.Value;
        }

        public bool SendEmail(string to, string subject, string body, bool isHtml = true) {
            try {

                using (var mailMessage = new MailMessage(_appSecret.Email, to)) {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = false;
                    mailMessage.IsBodyHtml = isHtml;

                    var networkCredential = new NetworkCredential(_appSecret.Email, _appSecret.Password);
                    var smtp = new SmtpClient {
                        Host = "smtp.gmail.com",
                        EnableSsl = true,
                        Credentials = networkCredential,
                        Port = 587,
                    };
                    smtp.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception e){
            }
            return false;
        }

        private readonly Secret _appSecret;
    }
}
