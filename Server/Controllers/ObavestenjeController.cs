using System.Net;
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

        public ObavestenjeController(PushPretplataService pushPretplataService)
        {
            _pushPretplataService = pushPretplataService;
        }
        
        [HttpPost("subscribe")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<PushPretplata> Subscribe([FromBody] PushPretplata sub)
        {
            return _pushPretplataService.Create(sub);
        }
    }
}