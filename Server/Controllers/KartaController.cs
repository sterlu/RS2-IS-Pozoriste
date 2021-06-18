using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Helpers;

namespace Server.Controllers
{
    // https://localhost:44317/api/karta
    /*
        {
            "Id": "123456789012345678901234",
            "Kod": "Kod1",
            "Cena": 250,
            "Tip": "tip1",
            "sifraPredstave": "Predstava1",
            "brojRezervacije": 12,
            "Username": "user1"
        }
     */
    [Route("api/[controller]")]
    [ApiController] 
    /// Kontroler za model Karta. 
    /// Sadrži implementaciju http zahteva vezanih za pozorišne karte.

    public class KartaController : ControllerBase
    {
        private KartaService _kartaService;

        public KartaController(KartaService kartaService)
        {
            _kartaService = kartaService;
        }

        [HttpGet]
        [AdminOnly]
        /// Dohvata sve karte iz baze.
        public ActionResult<List<Karta>> Get() =>
            _kartaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetKarta")]
        [AdminOnly]
        /// Dohvata odredjenu kartu na osnovu njenog id-ja.
        public ActionResult<Karta> Get(string id)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            return karta;
        }

        [HttpPost]
        [AdminOnly]
        /// Beleži novu kartu u bazi.
        public ActionResult<Karta> Create(Karta karta)
        {
           var k =  _kartaService.Create(karta);
           if (k != null)
                return CreatedAtRoute("GetKarta", new { id = karta.Id.ToString() }, karta);
            // nema slobodnih karata
            return null;
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        /// Menja postojeću vrednost u bazi.
        /// @param newValForKarta - nova vrednost koja treba da se nadje u bazi. 
        /// @param id - id postojeće vrednosti koja se menja.
        public IActionResult Update(string id, Karta newValForKarta)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            _kartaService.Update(id, newValForKarta);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        /// Briše postojeće izvodjenje iz baze na osnovu id-ja.
        public IActionResult Delete(string id)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            _kartaService.Remove(karta.Id);

            return NoContent();
        }
    }
}
