using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    // https://localhost:44317/api/karta
    /*
        {
            "Id": "123456789012345678901234",
            "Kod": "Kod1",
            "Cena": 250,
            "Tip": "tip1",
            "sifraPredstave": "Predstava1",
            "brojRezervacije": 12,
            "Username": "user1"
        }
     */
    [Route("api/[controller]")]
    [ApiController]
    public class KartaController : ControllerBase
    {
        private KartaService _kartaService;

        public KartaController(KartaService kartaService)
        {
            _kartaService = kartaService;
        }

        [HttpGet]
        public ActionResult<List<Karta>> Get() =>
            _kartaService.Get();

        [HttpGet("{id:length(24)}", Name = "GetKarta")]
        public ActionResult<Karta> Get(string id)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            return karta;
        }

        [HttpPost]
        public ActionResult<Karta> Create(Karta karta)
        {
            _kartaService.Create(karta);

            // https://docs.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller.createdatroute?view=aspnetcore-2.2
            //  CreatedAtRoute in the Create action method returns an HTTP 201 response.
            //  Status code 201 is the standard response for an HTTP POST method that creates a new resource on the server.
            //  CreatedAtRoute also adds a Location header to the response. The Location header specifies the URI of the newly created book.
            return CreatedAtRoute("GetKarta", new { id = karta.Id.ToString() }, karta);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Karta newValForKarta)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            _kartaService.Update(id, newValForKarta);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var karta = _kartaService.Get(id);

            if (karta == null)
            {
                return NotFound();
            }

            _kartaService.Remove(karta.Id);

            return NoContent();
        }
    }
}
