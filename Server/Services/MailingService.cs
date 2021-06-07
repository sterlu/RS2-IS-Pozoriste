using System.Net;
using System.Net.Mail;
using Server.Models;

namespace Server.Services
{
    public class MailingService
    {
        private KorisnikService _korisnikService;
        private PredstavaService _predstavaService;
        public MailingService(KorisnikService korisnikService, PredstavaService predstavaService)
        {
            _korisnikService = korisnikService;
            _predstavaService = predstavaService;
        }

        public void PosaljiObavestenje(string idPredstave)
        {
            var predstava = _predstavaService.Get(idPredstave);
            var mailingLista = _korisnikService.MailingList();
            var body = "Predstava \""
                + predstava.NazivPredstave
                + "\" za koju ste zatražili obaveštenja uskoro kreće sa izvođenjem!";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("matfpozoriste@gmail.com", "pozoriste123"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("matfpozoriste@gmail.com"),
                Subject = predstava.NazivPredstave + " kreće sa izvođenjem",
                Body = body,
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

            smtpClient.Send(mailMessage);
        }
        
    }
}