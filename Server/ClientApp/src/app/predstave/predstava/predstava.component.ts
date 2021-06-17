import { Component, Input, OnInit } from '@angular/core';
import { Predstava } from '../../../models/predstava';
import { SwPush } from '@angular/service-worker';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-predstava',
  templateUrl: './predstava.component.html',
  styleUrls: ['./predstava.component.scss']
})
export class PredstavaComponent {
  @Input() predstava: Predstava;

  constructor(private swPush: SwPush, private http: HttpClient) { }

  subscribeToNotifications(): void {
    if (!confirm('Da li želite da budete obavešteni kada predstava bude najavljena za izvođenje?')) return;
    this.swPush.requestSubscription({
      serverPublicKey: 'BMtMn89komUmRhkvdRxsf_w54bHPlwcqBNxg6HX4cWzpi9OPiOzAnP4jT8WiH8VmdikQGOM-F_rDJxpBaxpjSRs'
    })
      .then((r) => {
        const payload = {
          endpoint: r.endpoint,
          p256dh: r.toJSON().keys.p256dh,
          auth: r.toJSON().keys.auth,
          idPredstave: this.predstava.Id,
        };
        this.http.post<object[]>('/api/obavestenje/push/subscribe', payload).subscribe(result => {
          console.log(result);
          alert('Bićete obavešteni kada predstava bude spremna za izvođenje');
        }, error => console.error(error));
      }).catch(console.error);
  }
}
