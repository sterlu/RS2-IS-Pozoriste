using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredstavaController : ControllerBase
    {
        private PredstavaService _predstavaService;

        public PredstavaController(PredstavaService predstavaService)
        {
            _predstavaService = predstavaService;
        }

        [HttpGet]
        public ActionResult<List<Predstava>> Get() =>
            _predstavaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPredstava")]
        public ActionResult<Predstava> Get(string id)
        {
            var predstava = _predstavaService.Get(id);

            if (predstava == null)
            {
                return NotFound();
            }

            return predstava;
        }

        [HttpPost]
        public ActionResult<Predstava> Create(Predstava predstava)
        {
            _predstavaService.Create(predstava);

            
            return CreatedAtRoute("GetPredstava", new { id = predstava.Id.ToString() }, predstava);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Predstava newValForPredstava)
        {
            var predstava = _predstavaService.Get(id);

            if (predstava == null)
            {
                return NotFound();
            }

            _predstavaService.Update(id, newValForPredstava);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
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