import { Component, OnInit } from '@angular/core';
import { KorpaService } from "../../services/korpa.service";
import { Rezervacija } from "../../models/rezervacija";
import { loadStripe } from "@stripe/stripe-js";
import { HttpClient } from "@angular/common/http";
import { AccountService } from "../../services/account.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-korpa',
  templateUrl: './korpa.component.html',
  styleUrls: ['./korpa.component.scss']
})

export class KorpaComponent {
  rezervacije: Rezervacija[] = [];
  displayedColumns = ['predstava', 'izvodjenje', 'kolicina', 'cena', 'akcije'];

  constructor(public korpaService: KorpaService, private http: HttpClient, private accountService: AccountService, private router: Router) {
    korpaService.stanje$.subscribe(r => this.rezervacije = r);
  }

/**
 * Izbacivanje rezervacije iz korpe.
 * @param rez Rezervacija
 */
  izbaciRez(rez: any): void {
    this.rezervacije.splice(this.rezervacije.indexOf(rez), 1);
    console.log(this.rezervacije);
    this.korpaService.azurirajStanje([...this.rezervacije]);
  }

/**
 * Promena kolicine karata u rezervaciji.
 * @param rez Rezervacija
 * @param promena
 */
  promeniKolicinu(rez: any, promena: number) {
    this.rezervacije.find(r => r === rez).kolicina += promena;
    this.korpaService.azurirajStanje([...this.rezervacije]);
  }
  /**
   * Zapocinje se proces placanja.
   */

  async zapocniPlacanje(): Promise<void> {
    if (!this.accountService.currentUser) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: '/korpa' } });
      return;
    }
    const stripePromise = loadStripe('pk_test_51ItK33FhQvjpePiWfer0YV2JHbKFALtPiTpqm2oOc1hy4Nzuk9vAOV0gtF9kra7oAkgDG9NTC5dV8mcHuHlhGNxY00DmC5kWBf');
    const stripe = await stripePromise;
    const payload = this.rezervacije.map(r => ({
      predstavaId: r.predstava.Id,
      izvodjenjeId: r.izvodjenje.Id,
      kolicina: r.kolicina,
    }));
    this.http.post<object[]>('/api/placanje/create', payload).subscribe(async (session: any) => {
      console.log(session);
      const result = await stripe.redirectToCheckout({
        sessionId: session.id,
      });
      if (result.error) {
      }
    }, error => console.error(error));
  }

}
