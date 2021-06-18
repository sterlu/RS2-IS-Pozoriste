using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;
using Stripe.Checkout;

namespace Server.Services
{
    /// Klasa koja reguliše proces online naplate karata.
    public class PlacanjeService
    {
        private PredstavaService predstavaService;
        private KartaService kartaService;
        private KorisnikService _korisnikService;

        private IzvodjenjePredstaveService _izvodjenjePredstaveService;

        private MailingService _mailingService;

        public PlacanjeService(PredstavaService predstavaService, KartaService kartaService,
            KorisnikService korisnikService, IzvodjenjePredstaveService izvodjenjePredstaveService, MailingService mailingService)
        {
            this.predstavaService = predstavaService;
            this.kartaService = kartaService;
            _korisnikService = korisnikService;
            _izvodjenjePredstaveService = izvodjenjePredstaveService;
            _mailingService = mailingService;
        }

        /// Kreira se sesija za online plaćanje. 
        /// Pri uspešnoj kupovini se upisuju nove karte u bazu.

        public Session CreateSession(KupovinaKarteDto[] kupovine, string username, string domain = "localhost:5001")
        {
            var lineItems = new List<SessionLineItemOptions> { };
            foreach (KupovinaKarteDto kupovina in kupovine)
            {
                Predstava predstava = predstavaService.Get(kupovina.PredstavaId);
                IzvodjenjePredstave izvodjenje = _izvodjenjePredstaveService.Get(kupovina.IzvodjenjeId);
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = izvodjenje.Cena * 100,
                        Currency = "RSD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Karta za predstavu \"" + predstava.NazivPredstave + "\"",
                            Description = "Izvođenje " + izvodjenje.Datum + " " + izvodjenje.Vreme
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
                IzvodjenjePredstave izvodjenje = _izvodjenjePredstaveService.Get(kupovina.IzvodjenjeId);
                for (int i = 0; i < kupovina.Kolicina; i++)
                {
                    kartaService.Create(new Karta(
                        izvodjenje.Cena,
                        "Rezervisana",
                        kupovina.PredstavaId,
                        kupovina.IzvodjenjeId,
                        session.Id,
                        username
                    ));
                }
            }

            return session;
        }

        /// Potvrda plaćanja i slanje karata korisniku.
        // TODO: proveriti da li li sadrzaj mail-a treba da bude ovde jer postoji funkcija u servisu za to
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

            string emailSadrzaj = "Vaša kupovina je uspešna. Kupili ste karte:\n";
            foreach (Karta karta in karte)
            {
                var izvodjenje = _izvodjenjePredstaveService.Get(karta.IdIzvodjenja);
                var predstava = predstavaService.Get(izvodjenje.IdPredstave);
                emailSadrzaj += "Naziv predstave: " + predstava.NazivPredstave + "\n"
                        + "datum: " + izvodjenje.Datum + "\n"
                        + "vreme: " + izvodjenje.Vreme + "\n"
                        + "sala: " + izvodjenje.BrojSale + "\n"
                        + "cena: " + karta.Cena + "\n"
                        + "karta: " + karta.Id + "\n"
                        + "---\n";
            }

            _mailingService.PosaljiKartu(emailSadrzaj, email);
            
        }

        /// Otkazivanje plaćanja
        public void OtkaziPlacanje(string idRezervacije)
        {
            var karte = kartaService.GetAllForReservation(idRezervacije);
            foreach (var karta in karte)
            {
                karta.Status = "Otkazana";
                kartaService.Update(karta.Id, karta);
            }
        }
    }
}