import { Component, OnInit, Input } from '@angular/core';
import { Currency } from '../app.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-exchange-rate',
  templateUrl: './exchange-rate.component.html',
  styleUrls: ['./exchange-rate.component.scss']
})
export class ExchangeRateComponent implements OnInit {

  @Input() currencies$: Observable<Currency[]>;
  @Input() multiplier: number;
  constructor() { }

  ngOnInit() {
  }

}
