import {Component, OnInit} from '@angular/core';
import {Predstava} from '../../models/predstava';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute, Router} from '@angular/router';
import {Izvodjenje} from '../../models/izvodjenje';
import {Sala} from '../../models/sala';

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
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.http.get<Predstava>(`/api/predstava/${this.id}`).subscribe(result => {
        this.model = new Predstava(result.nazivPredstave, result.opis, result.status, result.trajanje, [], this.id);
      }, error => console.error(error));
    } else {
      this.model = new Predstava('Nova predstava', '', this.statuses[0], 120);
    }
    this.http.get<Sala[]>(`/api/sala/`).subscribe(result => {
      this.sale = result;
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
    const payload = {
      ...this.model,
      sifraPredstave: 1,
    };
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
