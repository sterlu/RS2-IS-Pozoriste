using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervacijaController : ControllerBase
    {
        private RezervacijaService _rezervacijaService;

        public RezervacijaController(RezervacijaService rezervacijaService)
        {
            _rezervacijaService = rezervacijaService;
        }

        [HttpGet]
        [AdminOnly]
        public ActionResult<List<Rezervacija>> Get() =>
            _rezervacijaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetRezervacija")]
        [AdminOnly]
        public ActionResult<Rezervacija> Get(string id)
        {
            var rezervacija = _rezervacijaService.Get(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return rezervacija;
        }

        [HttpPost]
        [AdminOnly]
        public ActionResult<Rezervacija> Create(Rezervacija rezervacija)
        {
            _rezervacijaService.Create(rezervacija);

            
            return CreatedAtRoute("GetRezervacija", new { id = rezervacija.Id.ToString() }, rezervacija);
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
        public IActionResult Update(string id, Rezervacija newValForRezervacija)
        {
            var rezervacija = _rezervacijaService.Get(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            _rezervacijaService.Update(id, newValForRezervacija);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [AdminOnly]
        public IActionResult Delete(string id)
        {
            var rezervacija = _rezervacijaService.Get(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            _rezervacijaService.Remove(rezervacija.Id);

            return NoContent();
        } 
    }
}