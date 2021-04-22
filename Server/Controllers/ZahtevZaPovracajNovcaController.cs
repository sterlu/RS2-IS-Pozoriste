using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Models;
using System.Collections.Generic;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZahtevZaPovracajNovcaController : ControllerBase
    {
        private ZahtevZaPovracajNovcaService _zahtevService;

        public ZahtevZaPovracajNovcaController(ZahtevZaPovracajNovcaService zahtevService)
        {
            _zahtevService = zahtevService;
        }

        [HttpGet]
        public ActionResult<List<ZahtevZaPovracajNovca>> Get() =>
            _zahtevService.Get();

        [HttpGet("{id:length(24)}", Name = "GetZahtev")]
        public ActionResult<ZahtevZaPovracajNovca> Get(string id)
        {
            var zahtev = _zahtevService.Get(id);

            if (zahtev == null)
            {
                return NotFound();
            }

            return zahtev;
        }

        [HttpPost]
        public ActionResult<ZahtevZaPovracajNovca> Create(ZahtevZaPovracajNovca zahtev)
        {
            _zahtevService.Create(zahtev);

            
            return CreatedAtRoute("GetZahtev", new { id = zahtev.Id.ToString() }, zahtev);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ZahtevZaPovracajNovca newValForZahtev)
        {
            var zahtev = _zahtevService.Get(id);

            if (zahtev == null)
            {
                return NotFound();
            }

            _zahtevService.Update(id, newValForZahtev);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var zahtev = _zahtevService.Get(id);

            if (zahtev == null)
            {
                return NotFound();
            }

            _zahtevService.Remove(zahtev.Id);

            return NoContent();
        } 
    }
}