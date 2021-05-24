using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers 
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SalaController : ControllerBase
    {

        private SalaService _salaService;

        public SalaController(SalaService salaService)
        {
            _salaService = salaService;
        }

        [HttpGet]
        public ActionResult<List<Sala>> Get() =>
            _salaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSala")]
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
        public ActionResult<Sala> Create(Sala sala)
        {
            _salaService.Create(sala);

            
            return CreatedAtRoute("GetSala", new { id = sala.Id.ToString() }, sala);
        }

        [HttpPut("{id:length(24)}")]
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