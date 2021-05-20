using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        const string Secret = "whsec_fu9cXIXq8nd5g0N9Vb9UBHTUARlSiBrn";

        [HttpPost("create")]
        public ActionResult Create()
        {
            var domain = "https://localhost:5001/predstava/1";
            var options = new SessionCreateOptions
            {
                // CustomerEmail = "customer@example.com", // TODO
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                      PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = 50000,
                      Currency = "RSD",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Karta TODO",
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "?success=true",
                CancelUrl = domain + "?canceled=true",
            };
            var service = new SessionService();
            Session session = service.Create(options);
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
                    this.FulfillOrder(session);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        private void FulfillOrder(Stripe.Checkout.Session session) {
            // TODO
        }
    }
}