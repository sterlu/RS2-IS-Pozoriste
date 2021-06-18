using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// Klasa koja reguliše proces online naplate karata.
    public class PlacanjeController : Controller
    {
        private PredstavaService predstavaService;
        private IzvodjenjePredstaveService izvodjenjeService;
        private DomainSettings domains;
        public PlacanjeController(PredstavaService predstavaService, IzvodjenjePredstaveService izvodjenjeService, DomainSettings domains)
        {
            this.predstavaService = predstavaService;
            this.izvodjenjeService = izvodjenjeService;
            this.domains = domains;
        }
        
        [HttpPost("create")]
        [Authorize]
        /// Kreira se sesija za online plaćanje.
        public async Task<ActionResult<string>> Create(KupovinaKarteDto[] kupovine)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            KupovinaKarteInternalPayloadDTO payload = new KupovinaKarteInternalPayloadDTO();
            payload.kupovine = new List<KupovinaKarteInternalDto>(kupovine.Length);
            foreach (KupovinaKarteDto kupovina in kupovine)
            {
                var _kupovina = new KupovinaKarteInternalDto();
                _kupovina.predstava = predstavaService.Get(kupovina.PredstavaId);
                _kupovina.izvodjenje = izvodjenjeService.Get(kupovina.IzvodjenjeId);
                _kupovina.Kolicina = kupovina.Kolicina;
                payload.kupovine.Add(_kupovina);
            }
            payload.username = username;
            var response = await (new HttpClient()).PostAsync(domains.PaymentService + "/placanje/create", new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        
        [HttpPost("webhook")]
        public Task Index()
        {
            return this.HttpProxyAsync(domains.PaymentService + "/placanje/webhook");
        }
    }
}