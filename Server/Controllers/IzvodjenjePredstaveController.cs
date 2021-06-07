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

    public class IzvodjenjePredstaveController : ControllerBase
    {
        private IzvodjenjePredstaveService _izvodjenjeService;

        public IzvodjenjePredstaveController(IzvodjenjePredstaveService izvodjenjeService)
        {
            _izvodjenjeService = izvodjenjeService;
        }

        [HttpGet]
        [AdminOnly]
        public ActionResult<List<IzvodjenjePredstave>> Get() =>
            _izvodjenjeService.Get();

        [HttpGet("{id:length(24)}", Name = "GetIzvodjenje")]
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
        public ActionResult<List<IzvodjenjePredstave>> DnevniRepertoar(string datum)
        {
            var repertoar = _izvodjenjeService.GetByDate(datum);
            return repertoar;
        }

        [HttpGet("{idPredstave}")] 
        public ActionResult<List<IzvodjenjePredstave>> GetIzvodjenja(string idPredstave)
        {
            var izvodjenja = _izvodjenjeService.GetIzvodjenjaByIdPredstave(idPredstave);
            return izvodjenja;
        }

        [HttpPost]
        [AdminOnly]
        public ActionResult<IzvodjenjePredstave> Create(IzvodjenjePredstave izvodjenje)
        {
            _izvodjenjeService.Create(izvodjenje);

            
            return CreatedAtRoute("GetIzvodjenje", new { id = izvodjenje.Id.ToString() }, izvodjenje);
        }

        [HttpPut("{id:length(24)}")]
        [AdminOnly]
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