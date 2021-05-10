import { Component, OnInit } from '@angular/core';
import {Predstava} from '../../models/predstava';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-predstava-form',
  templateUrl: './predstava-form.component.html',
  styleUrls: ['./predstava-form.component.scss']
})
export class PredstavaFormComponent implements OnInit {

  statuses = ['u pripremi', 'aktivna', 'arhivirana'];
  model = new Predstava('', '', this.statuses[0], 0);
  id = '';

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    console.log(this.id);
    if (this.id) {
      this.http.get<Predstava>(`/api/predstava/${this.id}`).subscribe(result => {
        this.model = new Predstava(result.nazivPredstave, result.opis, result.status, result.trajanje, this.id);
      }, error => console.error(error));
    } else {
      this.model = new Predstava('Nova predstava', '', this.statuses[0], 120);
    }
  }

  onSubmit() {
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

}
