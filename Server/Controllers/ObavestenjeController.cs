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
    /// Kontroler za model PushPretplata. 
    /// Sadrzi implementaciju http zahteva vezanih za obavestenja korisnika o spremnim predstavama.
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
        /// Beleži novu pretplatu korisnika na neku predstavu, kako bi koirsnik bio obavešten kada bude spremna za izvodjenje.
        public ActionResult<PushPretplata> Subscribe([FromBody] PushPretplata sub)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            sub.Username = username;
            return _pushPretplataService.Create(sub);
        }
        
        [HttpDelete("push/unsubscribe/{id}")]
        [Authorize]
        /// Uklanja pretplatu korisnika na odredjenu predstavu, kako ne bi dobio obaveštenje za istu.
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
        /// Izlistava pretplate korisnika.
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
        /// Omogucava korisniku da prima obaveštenja elektronskom poštom.
        public void Subscribe()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            var korisnik = _korisnikService.GetByUsername(username);
            korisnik.EmailObavestenja = true;
            _korisnikService.Update(korisnik.Id, korisnik);
        }
        
        [HttpPut("email/unsubscribe")]
        [Authorize]
        /// Ukida primanje obaveštenja korisnika elektronskom poštom.
        public void Unsubscribe()
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            var korisnik = _korisnikService.GetByUsername(username);
            korisnik.EmailObavestenja = false;
            _korisnikService.Update(korisnik.Id, korisnik);
        }

        [HttpGet("posaljikartu/{idRezervacije}")]
        /// Šalje plaćene karte elektronskom poštom.
        public IActionResult PosaljiPlacenuKartu(string idRezervacije)
        {
            _mailingService.PosaljiPlacenuKartu(idRezervacije);
            return new NoContentResult();
        }
    }
}