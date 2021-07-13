import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Predstava } from '../../models/predstava';
import { AccountService } from "../../services/account.service";

@Component({
  selector: 'app-predstave',
  templateUrl: './predstave.component.html',
  styleUrls: ['./predstave.component.scss']
})
export class PredstaveComponent implements OnInit {
  public predstave: Predstava[] = [];

  constructor(private http: HttpClient, private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.http.get<object[]>('/api/predstava').subscribe((result: Predstava[]) => {
      console.log(result);
      const isAdmin = this.accountService.currentUser?.Tip === 'admin';
      console.log(isAdmin, this.accountService.currentUser?.Tip);
      result.sort((a, b) => a.Id.localeCompare(b.Id));
      if (!isAdmin) result = result.filter(p => p.status !== 'arhivirana');
      this.predstave = result;
    }, error => console.error(error));
  }

}
