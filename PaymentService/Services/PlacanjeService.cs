using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using PaymentService.Models;
using PaymentService.DTO;

namespace PaymentService.Services
{
    /// Klasa koja reguliše proces online naplate karata.
    public class PlacanjeService
    {
        private KartaService _kartaService; 
        private DomainSettings domains;

        public PlacanjeService(KartaService kartaService, DomainSettings domains)
        {
            _kartaService = kartaService;
            this.domains = domains;
        }

        /// Kreira se sesija za online plaćanje. 
        /// Pri uspešnoj kupovini se upisuju nove karte u bazu.
        public Session CreateSession(KupovinaKarteInternalPayloadDTO payload, string domain = "localhost:5001")
        {
            var lineItems = new List<SessionLineItemOptions> { };
            foreach (KupovinaKarteInternalDTO kupovina in payload.kupovine)
            {
                Predstava predstava = kupovina.predstava;
                IzvodjenjePredstave izvodjenje = kupovina.izvodjenje;
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
                SuccessUrl = domains.ExternalDomain + "/gotova-kupovina?success=true",
                CancelUrl = domains.ExternalDomain + "/gotova-kupovina?canceled=true",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            foreach (KupovinaKarteInternalDTO kupovina in payload.kupovine)
            {
                for (int i = 0; i < kupovina.Kolicina; i++)
                {
                    _kartaService.Create(new Karta(
                        kupovina.izvodjenje.Cena,
                        "Rezervisana",
                        kupovina.predstava.Id,
                        kupovina.izvodjenje.Id,
                        session.Id,
                        payload.username
                    ));
                }
            }

            return session;
        }

        public void PotvrdiPlacanje(string idRezervacije)
        {
            var karte = _kartaService.GetAllForReservation(idRezervacije);
            foreach (var karta in karte)
            {
                karta.Status = "Potvrdjena";
                _kartaService.Update(karta.Id, karta);
            }

            (new HttpClient()).GetAsync(domains.Server + "/api/obavestenje/posaljikartu/" + idRezervacije);
        }

        /// Otkazivanje plaćanja.
        public void OtkaziPlacanje(string idRezervacije)
        {
            var karte = _kartaService.GetAllForReservation(idRezervacije);
            foreach (var karta in karte)
            {
                karta.Status = "Otkazana";
                _kartaService.Update(karta.Id, karta);
            }
        }
    }
}