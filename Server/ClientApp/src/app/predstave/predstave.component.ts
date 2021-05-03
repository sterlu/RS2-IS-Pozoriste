import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-predstave',
  templateUrl: './predstave.component.html',
  styleUrls: ['./predstave.component.scss']
})
export class PredstaveComponent implements OnInit {
  public predstave: object[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<object[]>('/api/predstava').subscribe(result => {
      console.log(result);
      this.predstave = result;
    }, error => console.error(error));
  }

}
