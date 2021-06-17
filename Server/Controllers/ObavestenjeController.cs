using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObavestenjeController
    {
        private PushPretplataService _pushPretplataService;
        private MailingService _mailingService;

        public ObavestenjeController(PushPretplataService pushPretplataService, MailingService mailingService)
        {
            _pushPretplataService = pushPretplataService;
            _mailingService = mailingService;
        }
        
        [HttpPost("subscribe")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Authorize]
        public ActionResult<PushPretplata> Subscribe([FromBody] PushPretplata sub)
        {
            return _pushPretplataService.Create(sub);
        }

        [HttpGet("posaljikartu/{idRezervacije}")]
        public IActionResult PosaljiPlacenuKartu(string idRezervacije)
        {
            _mailingService.PosaljiPlacenuKartu(idRezervacije);
            return new NoContentResult();
        }

        [HttpDelete("unsubscribe/{username}/{idPredstave}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Authorize]
        public void Unsubscribe(string username, string idPredstave)
        {

            _pushPretplataService.Remove(username, idPredstave);
        }
    }
}