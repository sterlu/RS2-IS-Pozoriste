import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import {KorpaService} from "../../services/korpa.service";

@Component({
  selector: 'app-gotovo-placanje',
  templateUrl: './gotovo-placanje.component.html',
  styleUrls: ['./gotovo-placanje.component.scss']
})
export class GotovoPlacanjeComponent implements OnInit {
  status = true;

  constructor(private route: ActivatedRoute, private korpaService: KorpaService) {}

  ngOnInit(): void {
    if (this.route.snapshot.queryParamMap.get('canceled')) {
      this.status = false;
    } else {
      this.korpaService.ocisti();
    }
  }

  pokusajPonovo() {
    window.history.back();
  }
}
