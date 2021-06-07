using System.Net;
using System.Net.Mail;
using Server.Models;

namespace Server.Services
{
    public class MailingService
    {
        private KorisnikService _korisnikService;
        public MailingService(KorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        public void PosaljiObavestenje(string obavestenje)
        {
            var mailingLista = _korisnikService.MailingList();
            
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("matfpozoriste@gmail.com", "pozoriste123"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("email"),
                Subject = "Nova predstava",
                Body = obavestenje,
                IsBodyHtml = false
            };
            foreach(Korisnik korisnik in mailingLista)
            {
                mailMessage.To.Add(korisnik.Email);  
            }
            smtpClient.Send(mailMessage);

        }

        public void PosaljiKartu(string sadrzaj, string email)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("matfpozoriste@gmail.com", "pozoriste123"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("email"),
                Subject = "Vase karte",
                Body = sadrzaj,
                IsBodyHtml = false
            };

            mailMessage.To.Add(email);
        }
        
    }
}