import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { KorpaService } from "../../services/korpa.service";
import { Sala } from "../../models/sala";
import { Izvodjenje } from "../../models/izvodjenje";
import { Predstava } from "../../models/predstava";
import { Rezervacija } from "../../models/rezervacija";

@Component({
  selector: 'app-predstava-page',
  templateUrl: './predstava-page.component.html',
  styleUrls: ['./predstava-page.component.scss']
})
export class PredstavaPageComponent implements OnInit {
  id = '';
  predstava: Predstava;
  sale: Sala[];

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute, private korpaService: KorpaService) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.http.get<Sala[]>(`/api/sala/`).subscribe(result => {
      this.sale = result;
      this.http.get<any>(`/api/predstava/${this.id}`).subscribe(result => {
        const { predstava, izvodjenja } = result;
        const _izvodjenja = izvodjenja.map(i => new Izvodjenje(new Date(i.datum + ' ' + i.vreme), i.idPredstave, this.sale.find(s => s.brojSale === i.brojSale), i.Id));
        this.predstava = new Predstava(predstava.nazivPredstave, predstava.opis, predstava.status, predstava.trajanje, _izvodjenja, this.id);
      }, error => console.error(error));
    }, error => console.error(error))
  }

  dodajUKorpu(izvodjenje: Izvodjenje) {
    this.korpaService.dodaj(new Rezervacija(this.predstava, izvodjenje, 1))
  }
}
