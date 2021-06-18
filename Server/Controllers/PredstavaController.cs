using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// Kontroler za model Predstava. 
    /// Sadrži implementaciju http zahteva vezanih za predstave.
    public class PredstavaController : ControllerBase
    {
        private PredstavaService _predstavaService;
        private PushPretplataService _pushPretplataService;
        private IzvodjenjePredstaveService _izvodjenjePrestaveService;
        private MailingService _mailingService;

        public PredstavaController(PredstavaService predstavaService, PushPretplataService pushPretplataService, IzvodjenjePredstaveService izvodjenjePredstaveService, MailingService mailingService)
        {
            _predstavaService = predstavaService;
            _pushPretplataService = pushPretplataService;
            _izvodjenjePrestaveService = izvodjenjePredstaveService;
            _mailingService = mailingService;
        }

        [HttpGet]
        /// Dohvata sve predstave.
        public ActionResult<List<Predstava>> Get() =>
            _predstavaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPredstava")]
        /// Dohvata odredjenu predstavu na osnovu id-ja.
        public ActionResult<PredstavaDto> Get(string id)
        {
            var predstava = _predstavaService.Get(id);

            if (predstava == null) return NotFound();
                
            var izvodjenja = _izvodjenjePrestaveService.GetIzvodjenjaByIdPredstave(id);

            return new PredstavaDto(predstava, izvodjenja);
        }


        [HttpPost]
        [AdminOnly]
        /// Beleži novu predstavu u bazi.
        /// @param payload - predstava i njena izvodjenja
        public ActionResult<Predstava> Create(PredstavaDto payload)
        {
            var predstava = _predstavaService.Create(payload.predstava);

            foreach (var izvodjenje in payload.izvodjenja)
            {
                izvodjenje.IdPredstave = predstava.Id;
                _izvodjenjePrestaveService.Create(izvodjenje);
            }

            return CreatedAtRoute("GetPredstava", new { id = predstava.Id.ToString() }, predstava);
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        /// Menja status predstave, i obaveštava korisnike o tome.
        public IActionResult Update(string id, PredstavaDto payload)
        {
            var predstava = _predstavaService.Get(id);

            if (predstava == null) return NotFound();

            _predstavaService.Update(id, payload.predstava);
            
            foreach (var izvodjenje in payload.izvodjenja)
            {
                izvodjenje.IdPredstave = predstava.Id;
                if (izvodjenje.Id != null) _izvodjenjePrestaveService.Update(izvodjenje.Id, izvodjenje);
                else _izvodjenjePrestaveService.Create(izvodjenje);
            }

            if (predstava.Status == "u pripremi" && payload.predstava.Status == "aktivna")
            {
                _pushPretplataService.Obavesti(payload.predstava.Id);
                _mailingService.PosaljiObavestenje(payload.predstava.Id);
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        /// Briše postojeće izvodjenje iz baze na osnovu id-ja.
        public IActionResult Delete(string id)
        {
            var predstava = _predstavaService.Get(id);

            if (predstava == null)
            {
                return NotFound();
            }

            _predstavaService.Remove(predstava.Id);

            return NoContent();
        } 
    }
}