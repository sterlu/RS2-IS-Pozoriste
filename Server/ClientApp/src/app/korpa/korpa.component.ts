import { Component, OnInit } from '@angular/core';
import { KorpaService } from "../../services/korpa.service";
import { Rezervacija } from "../../models/rezervacija";

@Component({
  selector: 'app-korpa',
  templateUrl: './korpa.component.html',
  styleUrls: ['./korpa.component.scss']
})
export class KorpaComponent {
  rezervacije: Rezervacija[] = [];
  displayedColumns = ['predstava', 'izvodjenje', 'kolicina', 'cena', 'akcije'];

  constructor(private korpaService: KorpaService) {
    korpaService.stanje$.subscribe(r => this.rezervacije = r);
  }

  izbaciRez(rez: any): void {
    this.rezervacije.splice(this.rezervacije.indexOf(rez), 1);
    console.log(this.rezervacije);
    this.korpaService.azurirajStanje([...this.rezervacije]);
  }

  promeniKolicinu(rez: any, promena: number) {
    this.rezervacije.find(r => r === rez).kolicina += promena;
    this.korpaService.azurirajStanje([...this.rezervacije]);
  }
}
