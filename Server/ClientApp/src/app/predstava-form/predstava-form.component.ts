import { Component, OnInit } from '@angular/core';
import { Predstava } from '../../models/predstava';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Izvodjenje } from '../../models/izvodjenje';
import { Sala } from '../../models/sala';

@Component({
  selector: 'app-predstava-form',
  templateUrl: './predstava-form.component.html',
  styleUrls: ['./predstava-form.component.scss']
})
export class PredstavaFormComponent implements OnInit {
  statuses = ['u pripremi', 'aktivna', 'arhivirana'];
  sale: Sala[] = [];
  model = new Predstava('', '', this.statuses[0], 0);
  id = '';

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.http.get<Sala[]>(`/api/sala/`).subscribe(result => {
      this.sale = result;

      this.id = this.route.snapshot.paramMap.get('id');
      if (this.id) {
        this.http.get<any>(`/api/predstava/${this.id}`).subscribe(result => {
          const { predstava, izvodjenja } = result;
          const _izvodjenja = izvodjenja.map(i => new Izvodjenje(new Date(i.datum + ' ' + i.vreme), i.idPredstave, this.sale.find(s => s.brojSale === i.brojSale), i.Id));
          this.model = new Predstava(predstava.nazivPredstave, predstava.opis, predstava.status, predstava.trajanje, _izvodjenja, this.id);
        }, error => console.error(error));
      } else {
        this.model = new Predstava('Nova predstava', '', this.statuses[0], 120);
      }

    }, error => console.error(error));
  }

  dodajIzvodjenje(): void {
    const datum = new Date(Date.now() + 24 * 60 * 60 * 1000);
    datum.setHours(20);
    datum.setMinutes(0);
    datum.setSeconds(0);
    this.model.izvodjenja.push(new Izvodjenje(datum, this.model.Id, this.sale[0]));
  }

  obrisiIzvodjenje(i: number): void {
    this.model.izvodjenja.splice(i, 1);
  }

  onSubmit(): void {
    console.log(this.model);
    console.log(this.model.toJSON());
    const payload = this.model.toJSON();
    (this.id
      ? this.http.put<object[]>(`/api/predstava/${this.id}`, payload)
      : this.http.post<object[]>('/api/predstava', payload)
    ).subscribe(result => {
      console.log(result);
      this.router.navigate(['/predstave']);
    }, error => console.error(error));
  }

  log(): void {
    console.log(arguments);
  }
}
