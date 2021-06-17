using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObavestenjeController : Controller
    {
        private PushPretplataService _pushPretplataService;
        private MailingService _mailingService;
        private PredstavaService _predstavaService;
        private KorisnikService _korisnikService;

        public ObavestenjeController(PushPretplataService pushPretplataService, MailingService mailingService, PredstavaService predstavaService, KorisnikService korisnikService)
        {
            _pushPretplataService = pushPretplataService;
            _mailingService = mailingService;
            _predstavaService = predstavaService;
            _korisnikService = korisnikService;
        }
        
        [HttpPost("push/subscribe")]
        [Authorize]
        public ActionResult<PushPretplata> Subscribe([FromBody] PushPretplata sub)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            sub.Username = username;
            return _pushPretplataService.Create(sub);
        }
        
        [HttpDelete("push/unsubscribe/{id}")]
        [Authorize]
        public IActionResult Unsubscribe(string id)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            if (_pushPretplataService.Get(id).Username == username)
            {
                _pushPretplataService.Remove(id);
            }
            return new NoContentResult();
        }

        [HttpGet("push/subscriptions")]
        [Authorize]
        public ActionResult<PushPretplataPayloadDTO> Izlistaj()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            var tmp = _pushPretplataService.GetForUser(username);
            var pretplate = new List<PushPretplataDTO>();
            foreach (var pretplata in tmp)
            {
                var predstava = _predstavaService.Get(pretplata.IdPredstave);
                pretplate.Add(new PushPretplataDTO(pretplata.Id, predstava.NazivPredstave));
            }
            var korisnik = _korisnikService.GetByUsername(username);
            return new PushPretplataPayloadDTO(pretplate, korisnik.EmailObavestenja);
        }
        
        [HttpPut("email/subscribe")]
        [Authorize]
        public void Subscribe()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            var korisnik = _korisnikService.GetByUsername(username);
            korisnik.EmailObavestenja = true;
            _korisnikService.Update(korisnik.Id, korisnik);
        }
        
        [HttpPut("email/unsubscribe")]
        [Authorize]
        public void Unsubscribe()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            var korisnik = _korisnikService.GetByUsername(username);
            korisnik.EmailObavestenja = false;
            _korisnikService.Update(korisnik.Id, korisnik);
        }

        [HttpGet("posaljikartu/{idRezervacije}")]
        public IActionResult PosaljiPlacenuKartu(string idRezervacije)
        {
            _mailingService.PosaljiPlacenuKartu(idRezervacije);
            return new NoContentResult();
        }
    }
}