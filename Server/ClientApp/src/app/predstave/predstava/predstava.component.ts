import {Component, Input, OnInit} from '@angular/core';
import {Predstava} from '../../../models/predstava';

@Component({
  selector: 'app-predstava',
  templateUrl: './predstava.component.html',
  styleUrls: ['./predstava.component.scss']
})
export class PredstavaComponent implements OnInit {
  @Input() predstava: Predstava;

  constructor() { }

  ngOnInit(): void {
  }
}
