using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.Collections.Generic;


namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlijentController : ControllerBase
    {
        private KlijentService _klijentService;

        public KlijentController(KlijentService klijentService)
        {
            _klijentService = klijentService;
        }

        [HttpGet]
        public ActionResult<List<Klijent>> Get() =>
            _klijentService.Get();

        [HttpGet("{id:length(24)}", Name = "GetKlijent")]
        public ActionResult<Klijent> Get(string id)
        {
            var klijent = _klijentService.Get(id);

            if (klijent == null)
            {
                return NotFound();
            }

            return klijent;
        }

        [HttpPost]
        public ActionResult<Klijent> Create(Klijent klijent)
        {
            _klijentService.Create(klijent);

            
            return CreatedAtRoute("GetKlijent", new { id = klijent.Id.ToString() }, klijent);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Klijent newValForKlijent)
        {
            var klijent = _klijentService.Get(id);

            if (klijent == null)
            {
                return NotFound();
            }

            _klijentService.Update(id, newValForKlijent);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var klijent = _klijentService.Get(id);

            if (klijent == null)
            {
                return NotFound();
            }

            _klijentService.Remove(klijent.Id);

            return NoContent();
        } 
    }
}