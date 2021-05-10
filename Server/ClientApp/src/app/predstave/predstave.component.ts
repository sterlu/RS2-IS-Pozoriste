import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Predstava} from '../../models/predstava';

@Component({
  selector: 'app-predstave',
  templateUrl: './predstave.component.html',
  styleUrls: ['./predstave.component.scss']
})
export class PredstaveComponent implements OnInit {
  public predstave: Predstava[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get<object[]>('/api/predstava').subscribe((result: Predstava[]) => {
      console.log(result);
      this.predstave = result;
    }, error => console.error(error));
  }

}
