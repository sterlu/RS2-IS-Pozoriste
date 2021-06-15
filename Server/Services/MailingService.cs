using System.Net;
using System.Net.Mail;
using Server.Models;

namespace Server.Services
{
    public class MailingService
    {
        private KorisnikService _korisnikService;
        private PredstavaService _predstavaService;
        private KartaService _kartaService;
        private IzvodjenjePredstaveService _izvodjenjePredstaveService;
        public MailingService(KorisnikService korisnikService, PredstavaService predstavaService, IzvodjenjePredstaveService izvodjenjePredstaveService, KartaService kartaService)
        {
            _korisnikService = korisnikService;
            _predstavaService = predstavaService;
            _izvodjenjePredstaveService = izvodjenjePredstaveService;
            _kartaService = kartaService;
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
                From = new MailAddress("matfpozoriste@gmail.com"),
                Subject = "Vase karte",
                Body = sadrzaj,
                IsBodyHtml = false
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);
        }
        
        public void PosaljiPlacenuKartu(string idRezervacije)
        {
            string kartaUsername = _kartaService.GetByRezervacijaId(idRezervacije).Username;
            string email = _korisnikService.GetByUsername(kartaUsername).Email;
            var karte = _kartaService.GetAllForReservation(idRezervacije);
            
            string emailSadrzaj = "Vaša kupovina je uspešna. Kupili ste karte:\n";

            foreach (Karta karta in karte)
            {
                var izvodjenje = _izvodjenjePredstaveService.Get(karta.IdIzvodjenja);
                var predstava = _predstavaService.Get(izvodjenje.IdPredstave);
                emailSadrzaj += "Naziv predstave: " + predstava.NazivPredstave + "\n"
                                + "datum: " + izvodjenje.Datum + "\n"
                                + "vreme: " + izvodjenje.Vreme + "\n"
                                + "sala: " + izvodjenje.BrojSale + "\n"
                                + "cena: " + karta.Cena + "\n"
                                + "karta: " + karta.Id + "\n"
                                + "---\n";
            }
            
            this.PosaljiKartu(emailSadrzaj, email);
        }

    }
}