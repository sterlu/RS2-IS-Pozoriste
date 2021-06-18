using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using Server.Helpers;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 

    /// Kontroler za model IzvodjenjePredstave. 
    /// Sadrži implementaciju http zahteva vezanih za izvodjenje predstave.
    public class IzvodjenjePredstaveController : ControllerBase
    {
        private IzvodjenjePredstaveService _izvodjenjeService;

        public IzvodjenjePredstaveController(IzvodjenjePredstaveService izvodjenjeService)
        {
            _izvodjenjeService = izvodjenjeService;
        }

        [HttpGet]
        [AdminOnly]
        /// Vraća listu svih izvodjenja predstava.
        public ActionResult<List<IzvodjenjePredstave>> Get() =>
            _izvodjenjeService.Get();

        [HttpGet("{id:length(24)}", Name = "GetIzvodjenje")]
        /// Dohvata izvodjenje predstave sa odredjenim id-jem.
        public ActionResult<IzvodjenjePredstave> Get(string id)
        {
            var izvodjenje = _izvodjenjeService.Get(id);

            if (izvodjenje == null)
            {
                return NotFound();
            }

            return izvodjenje;
        }
        
        [HttpGet("dnevniRepertoar/{datum}")]
        /// Dohvata sva izvodjenja koja se odrzavaju odredjenog datuma.
        public ActionResult<List<IzvodjenjePredstave>> DnevniRepertoar(string datum)
        {
            var repertoar = _izvodjenjeService.GetByDate(datum);
            return repertoar;
        }

        [HttpGet("{idPredstave}")] 
        /// Dohvata sva izvodjenja odredjene predstave.
        public ActionResult<List<IzvodjenjePredstave>> GetIzvodjenja(string idPredstave)
        {
            var izvodjenja = _izvodjenjeService.GetIzvodjenjaByIdPredstave(idPredstave);
            return izvodjenja;
        }

        [HttpPost]
        [AdminOnly]
        /// Beleži novo izvodjenje u bazi.
        public ActionResult<IzvodjenjePredstave> Create(IzvodjenjePredstave izvodjenje)
        {
            _izvodjenjeService.Create(izvodjenje);

            
            return CreatedAtRoute("GetIzvodjenje", new { id = izvodjenje.Id.ToString() }, izvodjenje);
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        /// Menja postojecu vrednost u bazi.
        /// @param newValForIzvodjenje - nova vrednost koja treba da se nadje u bazi. 
        /// @param id - id postojeće vrednosti koja se menja. 
        public IActionResult Update(string id, IzvodjenjePredstave newValForIzvodjenje)
        {
            var izvodjenje = _izvodjenjeService.Get(id);

            if (izvodjenje == null)
            {
                return NotFound();
            }

            _izvodjenjeService.Update(id, newValForIzvodjenje);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        /// Briše postojeće izvodjenje iz baze na osnovu id-ja.
        public IActionResult Delete(string id)
        {
            var izvodjenje = _izvodjenjeService.Get(id);

            if (izvodjenje == null)
            {
                return NotFound();
            }

            _izvodjenjeService.Remove(izvodjenje.Id);

            return NoContent();
        } 
    }
}