import { Component, OnInit } from '@angular/core';
import { loadStripe } from '@stripe/stripe-js';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-kupovina-karte',
  templateUrl: './kupovina-karte.component.html',
  styleUrls: ['./kupovina-karte.component.scss']
})
export class KupovinaKarteComponent implements OnInit {
  id = '';

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  async zapocniPlacanje(): Promise<void> {
    const stripePromise = loadStripe('pk_test_51ItK33FhQvjpePiWfer0YV2JHbKFALtPiTpqm2oOc1hy4Nzuk9vAOV0gtF9kra7oAkgDG9NTC5dV8mcHuHlhGNxY00DmC5kWBf');
    const stripe = await stripePromise;
    const payload = [{
      IdPredstave: this.id,
      Kolicina: 3
    }];
    this.http.post<object[]>('/api/placanje/create', payload).subscribe(async (session: any) => {
      console.log(session);
      const result = await stripe.redirectToCheckout({
        sessionId: session.id,
      });
      if (result.error) {}
    }, error => console.error(error));
  }
}
