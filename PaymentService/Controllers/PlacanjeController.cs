using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTO;
using PaymentService.Models;
using PaymentService.Services;
using Stripe;
using Stripe.Checkout;
namespace PaymentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    /// Klasa koja reguliše proces online naplate karata.
    public class PlacanjeController : ControllerBase
    {
        private PlacanjeService placanjeService;

        public PlacanjeController(PlacanjeService placanjeService)
        {
            this.placanjeService = placanjeService;
        }

        const string Secret = "whsec_fu9cXIXq8nd5g0N9Vb9UBHTUARlSiBrn";

        [HttpPost("create")]
        public JsonResult Create(KupovinaKarteInternalPayloadDTO payload)
        {
            Session session = placanjeService.CreateSession(payload, HttpContext.Request.Host.ToString());
            return new JsonResult(new { id = session.Id });
        }
        
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            Console.Write("test");
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