using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaposlenController : ControllerBase
    {
        private ZaposlenService _zaposlenService;

        public ZaposlenController(ZaposlenService zaposlenService)
        {
            _zaposlenService = zaposlenService;
        }

        [HttpGet]
        public ActionResult<List<Zaposlen>> Get() =>
            _zaposlenService.Get();

        [HttpGet("{id:length(24)}", Name = "GetZaposlen")]
        public ActionResult<Zaposlen> Get(string id)
        {
            var zaposlen = _zaposlenService.Get(id);

            if (zaposlen == null)
            {
                return NotFound();
            }

            return zaposlen;
        }

        [HttpPost]
        public ActionResult<Zaposlen> Create(Zaposlen zaposlen)
        {
            _zaposlenService.Create(zaposlen);

            
            return CreatedAtRoute("GetZaposlen", new { id = zaposlen.Id.ToString() }, zaposlen);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Zaposlen newValForZaposlen)
        {
            var zaposlen = _zaposlenService.Get(id);

            if (zaposlen == null)
            {
                return NotFound();
            }

            _zaposlenService.Update(id, newValForZaposlen);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var zaposlen = _zaposlenService.Get(id);

            if (zaposlen == null)
            {
                return NotFound();
            }

            _zaposlenService.Remove(zaposlen.Id);

            return NoContent();
        } 
    }
}