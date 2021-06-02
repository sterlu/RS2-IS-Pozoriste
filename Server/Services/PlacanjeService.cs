using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;
using Stripe.Checkout;
using System.Net.Mail;
using System.Net;

namespace Server.Services
{
    public class PlacanjeService
    {
        private PredstavaService predstavaService;
        private KartaService kartaService;
        private KorisnikService _korisnikService;

        private IzvodjenjePredstaveService _izvodjenjePredstaveService;

        public PlacanjeService(PredstavaService predstavaService, KartaService kartaService, KorisnikService korisnikService, IzvodjenjePredstaveService izvodjenjePredstaveService)
        {
            this.predstavaService = predstavaService;
            this.kartaService = kartaService;
            _korisnikService = korisnikService;
            _izvodjenjePredstaveService = izvodjenjePredstaveService;
        }

        public Session CreateSession(KupovinaKarteDto[] kupovine, string domain = "localhost:5001")
        {
            var lineItems = new List<SessionLineItemOptions>{};
            foreach (KupovinaKarteDto kupovina in kupovine)
            {
                Predstava predstava = predstavaService.Get(kupovina.IdPredstave);
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 50000, // predstava.Cena * 100
                        Currency = "RSD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Karta za predstavu \"" + predstava.NazivPredstave + "\""
                        },
                    },
                    Quantity = kupovina.Kolicina,
                });
            }
            var options = new SessionCreateOptions
            {
                // CustomerEmail = "customer@example.com", // TODO get from auth
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://" + domain + "/gotova-kupovina?success=true",
                CancelUrl = "https://" + domain + "/gotova-kupovina?canceled=true",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            
            foreach (KupovinaKarteDto kupovina in kupovine)
            {
                // Predstava predstava = predstavaService.Get(kupovina.IdPredstave);
                for (int i = 0; i < kupovina.Kolicina; i++)
                {
                    kartaService.Create(new Karta(
                        500, // predstava.Cena
                        "Rezervisana",
                        kupovina.IdPredstave,
                        session.Id,
                        ""
                    ));
                }
            }

            return session;
        }

        public void PotvrdiPlacanje(string idRezervacije)
        {
            var karte = kartaService.GetAllForReservation(idRezervacije);
            foreach (var karta in karte)
            {
                karta.Status = "Potvrdjena";
                kartaService.Update(karta.Id, karta);
            }

            string kartaUsername = kartaService.GetByRezervacijaId(idRezervacije).Username;
            string email = _korisnikService.GetByUsername(kartaUsername).Email;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("matfpozoriste@gmail.com", "pozoriste123"),
                EnableSsl = true
            };

            string body = "";
            foreach(Karta karta in karte)
            {
                var izvodjenje = _izvodjenjePredstaveService.Get(karta.IdIzvodjenja);
                var predstava = predstavaService.Get(izvodjenje.IdPredstave);
                body += "Naziv predstave: " + predstava.NazivPredstave +"\n"
                        + "datum: " + izvodjenje.Datum +"\n"
                        + "vreme: " + izvodjenje.Vreme + "\n"
                        + "sala: " + izvodjenje.BrojSale + "\n"
                        + "cena: " + karta.Cena.ToString() + "\n";
            }
            var mailMessage = new MailMessage
            {
                From = new MailAddress("matfpozoriste@gmail.com"),
                Subject = "Vasa karta",
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);

        }
    }
}