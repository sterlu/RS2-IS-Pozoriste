import { Injectable, OnInit } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Rezervacija } from "../models/rezervacija";
import { Sala } from "../models/sala";
import { Izvodjenje } from "../models/izvodjenje";
import { Predstava } from "../models/predstava";

@Injectable({
  providedIn: 'root',
})
export class KorpaService {
  private stanjeSource = new BehaviorSubject<Rezervacija[]>([]);

  stanje$ = this.stanjeSource.asObservable();

  constructor(private http: HttpClient) {
    this.ucitajPrethodnuKorpu();
  }

  /**
   * Ucitavanje prethodnog stanja korpe i pravljenje rezervacije.
   */
  async ucitajPrethodnuKorpu() {
    console.log('korpa init');
    if (localStorage.getItem('korpa')) {
      this.http.get<Sala[]>(`/api/sala/`).subscribe(async (result) => {
        const sale = result;
        const prethodnaKorpa = JSON.parse(localStorage.getItem('korpa'));
        const korpa: Rezervacija[] = await Promise.all(prethodnaKorpa.map(r => new Promise((res, rej) => {
          this.http.get<any>(`/api/predstava/${r.predstavaId}`).subscribe(result => {
            const { predstava, izvodjenja } = result;
            const _izvodjenja = izvodjenja.map(i => new Izvodjenje(new Date(i.datum + ' ' + i.vreme), i.idPredstave, sale.find(s => s.brojSale === i.brojSale), i.Id));
            const _predstava = new Predstava(predstava.nazivPredstave, predstava.opis, predstava.status, predstava.trajanje, _izvodjenja, r.predstavaId);
            const _izvodjenje = _izvodjenja.find(i => i.Id === r.izvodjenjeId);
            res(new Rezervacija(_predstava, _izvodjenje, r.kolicina));
          }, rej);
        })));
        this.azurirajStanje(korpa);
      }, error => console.error(error))
    }
  }

  public get stanje(): Rezervacija[] {
    return this.stanjeSource.value;
  }

  /**
   * Dodavanje nove rezervacije u korpu.
   * @param rezervacija
   */
  dodaj(rezervacija: Rezervacija): void {
    let korpa = [...this.stanjeSource.value];
    const postojecaRez = korpa.find(r => r.izvodjenje.Id === rezervacija.izvodjenje.Id);
    if (postojecaRez) postojecaRez.kolicina += rezervacija.kolicina;
    else korpa.push(rezervacija);
    this.azurirajStanje(korpa);
  }

  /**
   * Ayuriranje stanja korpe.
   * @param korpa
   */
  azurirajStanje(korpa: Rezervacija[]): void {
    const _korpa = korpa.filter(r => r.kolicina > 0);
    this.stanjeSource.next(_korpa);
    const payload = _korpa.map(r => ({
      predstavaId: r.predstava.Id,
      izvodjenjeId: r.izvodjenje.Id,
      kolicina: r.kolicina,
    }))
    localStorage.setItem('korpa', JSON.stringify(payload))
  }

  /**
   * Brisanje sadrzaja korpe.
   */
  ocisti(): void {
    localStorage.removeItem('korpa');
    this.azurirajStanje([]);
  }

  /**
   *
   * @returns Broj karata u rezervacijama.
   */
  brojRezervacija(): number {
    return this.stanje.reduce((sum, r) => sum + r.kolicina, 0);
  }

  /**
   *
   * @returns Cena rezervacije.
   */
  ukupnaCena(): number {
    return this.stanje.reduce((sum, r) => sum + (r.izvodjenje.cena * r.kolicina), 0);
  }
}
