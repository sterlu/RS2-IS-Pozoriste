import { Component, OnInit } from '@angular/core';
import {Predstava} from "../../models/predstava";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-predstava-form',
  templateUrl: './predstava-form.component.html',
  styleUrls: ['./predstava-form.component.scss']
})
export class PredstavaFormComponent implements OnInit {

  statuses = ['u pripremi', 'aktivna'];

  model = new Predstava('', '', this.statuses[0], 0);

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.model = new Predstava('Nova predstava', '...', this.statuses[0], 0);
  }

  onSubmit() {
    console.log(this.model);
    const payload = {
      ...this.model,
      sifraPredstave: 1,
    }
    this.http.post<object[]>('/api/predstava', payload).subscribe(result => {
      console.log(result);
      this.router.navigate(['/predstave']);
    }, error => console.error(error));
  }

}
