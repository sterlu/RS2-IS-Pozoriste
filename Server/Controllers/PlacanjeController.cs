using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Services;
using Stripe;
using Stripe.Checkout;

public class StripeOptions
{
    public string option { get; set; }
}

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacanjeController : Controller
    {
        private PlacanjeService placanjeService;

        public PlacanjeController(PlacanjeService placanjeService)
        {
            this.placanjeService = placanjeService;
        }

        const string Secret = "whsec_fu9cXIXq8nd5g0N9Vb9UBHTUARlSiBrn";

        [HttpPost("create")]
        [Authorize]
        public ActionResult Create(KupovinaKarteDto[] kupovine)
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
            Session session = placanjeService.CreateSession(kupovine, username, HttpContext.Request.Host.ToString());
            return Json(new { id = session.Id });
        }
        
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Console.Write(json);

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    Secret
                );

                // checkout.session.completed
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    placanjeService.PotvrdiPlacanje(session.Id);
                } else if (stripeEvent.Type == Events.PaymentIntentCanceled)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    placanjeService.OtkaziPlacanje(session.Id);
                }

                return Ok();
            }
            catch (StripeException)
            {
                return BadRequest();
            }
        }
    }
}