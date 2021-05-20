import { Component, OnInit } from '@angular/core';
import { loadStripe } from '@stripe/stripe-js';

@Component({
  selector: 'app-kupovina-karte',
  templateUrl: './kupovina-karte.component.html',
  styleUrls: ['./kupovina-karte.component.scss']
})
export class KupovinaKarteComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  async zapocniPlacanje(): Promise<void> {
    const stripePromise = loadStripe('pk_test_51ItK33FhQvjpePiWfer0YV2JHbKFALtPiTpqm2oOc1hy4Nzuk9vAOV0gtF9kra7oAkgDG9NTC5dV8mcHuHlhGNxY00DmC5kWBf');
    const stripe = await stripePromise;
    const response = await fetch('/api/placanje/create', { method: 'POST' });
    const session = await response.json();
    // When the customer clicks on the button, redirect them to Checkout.
    const result = await stripe.redirectToCheckout({
      sessionId: session.id,
    });
    if (result.error) {
      // If `redirectToCheckout` fails due to a browser or network
      // error, display the localized error message to your customer
      // using `result.error.message`.
    }
  }
}
