import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { Sala } from "../../models/sala";
import { AccountService } from "../../services/account.service";

@Component({
  selector: 'app-pretplate',
  templateUrl: './pretplate.component.html',
  styleUrls: ['./pretplate.component.scss']
})
export class PretplateComponent implements OnInit {
  pretplate: any = { push: [], email: false };
  displayedColumns = ['predstava', 'akcije'];

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute, private accountService: AccountService) {}

  ngOnInit(): void {
    this.dohvatiPretplate();
  }

  /**
   * Dohvatanje pretplata.
   */
  dohvatiPretplate() {
    this.http.get<Sala[]>(`/api/obavestenje/push/subscriptions/`).subscribe(result => {
      this.pretplate = result;
    }, error => console.error(error))
  }

  /**
   * Brisanje pretplate.
   * @param id Id pretplate.
   */
  obrisiPretplatu(id: string): void {
    if (!confirm('Da li želite da otkažete izabranu pretplatu?')) return;
    this.http.delete<Sala[]>(`/api/obavestenje/push/unsubscribe/${id}`).subscribe(result => {
      this.dohvatiPretplate()
    }, error => console.error(error))
  }

  /**
   * Uključivanje email obaveštenja o predstavama.
   */
  ukljuciEmail() {
    this.http.put<void>(`/api/obavestenje/email/subscribe/`, null).subscribe(result => {
      this.dohvatiPretplate()
    }, error => console.error(error))
  }

  /**
   *  Isključivanje email obaveštenja o predstavama.
   */
  iskljuciEmail() {
    this.http.put<void>(`/api/obavestenje/email/unsubscribe/`, null).subscribe(result => {
      this.dohvatiPretplate()
    }, error => console.error(error))
  }
}
