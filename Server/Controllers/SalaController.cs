using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers 
{
    
    [Route("api/[controller]")]
    [ApiController]
    /// Kontroler za model Sala. 
    /// Sadrži implementaciju http zahteva vezanih za pozorišne sale.
    public class SalaController : ControllerBase
    {

        private SalaService _salaService;

        public SalaController(SalaService salaService)
        {
            _salaService = salaService;
        }

        [HttpGet]
        /// Dohvata sve sale.
        public ActionResult<List<Sala>> Get() =>
            _salaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSala")]
        /// Dohvata odredjenu salu na osnovu id-ja.
        public ActionResult<Sala> Get(string id)
        {
            var sala = _salaService.Get(id);

            if (sala == null)
            {
                return NotFound();
            }

            return sala;
        }

        [HttpPost]
        [AdminOnly]
        /// Beleži novu salu u bazi.
        public ActionResult<Sala> Create(Sala sala)
        {
            _salaService.Create(sala);

            
            return CreatedAtRoute("GetSala", new { id = sala.Id.ToString() }, sala);
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        /// Menja postojeću vrednost u bazi.
        /// @param newValForSala - nova vrednost koja treba da se nadje u bazi. 
        /// @param id - id postojeće vrednosti koja se menja.
        public IActionResult Update(string id, Sala newValForSala)
        {
            var sala = _salaService.Get(id);

            if (sala == null)
            {
                return NotFound();
            }

            _salaService.Update(id, newValForSala);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        /// Briše postojeće izvodjenje iz baze na osnovu id-ja.
        public IActionResult Delete(string id)
        {
            var sala = _salaService.Get(id);

            if (sala == null)
            {
                return NotFound();
            }

            _salaService.Remove(sala.Id);

            return NoContent();
        } 
        
    }
}