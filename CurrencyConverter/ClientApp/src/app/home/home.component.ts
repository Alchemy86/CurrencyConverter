import { Component, Inject, ViewChild, OnInit, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BrowserLocalStorage, IStorage, Currency, CurrencyAuditEntry } from '../app.component';
import { map, debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { Subject, Observable } from 'rxjs';
import * as moment from 'moment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  currencies$: Observable<Currency[]>;
  currentValue: number = 1;
  @ViewChild('input', { static: true }) input: ElementRef;
  txtQueryChanged: Subject<number> = new Subject<number>();

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    @Inject(BrowserLocalStorage) private _localStorage: IStorage) {
      
      this.txtQueryChanged
          .pipe(debounceTime(1000), distinctUntilChanged())
          .subscribe(value => {
            console.log('Boom', value);
            this.saveValue(value)
            this.currencies$.pipe(map(x => x.map(y => this.AddAuditEntry(<CurrencyAuditEntry>{
              baseCurrency: 'GBP',
              convertedCurrency: y.name,
              conversionRate: y.value,
              multiplier: value,
              conversionValue: value * y.value,
              dateTime: moment()
            })))).subscribe()
          });
  }
  ngOnInit(): void {
    this.currencies$ = this.getCurrenyValues();
    this.currentValue = Number(this._localStorage.getItem('currentValue'));
  }

  getCurrenyValues(): Observable<Currency[]> {
    console.log('Getting now');
    const headers = new HttpHeaders().set('X-Requested-With', 'XMLHttpRequest');
    return this.http.get<Currency[]>(`${this.baseUrl}currency/gbp`, {
      headers
    }).pipe(map(project => project.filter(x => ['USD', 'AUD', 'EUR'].find(y=> y == x.name)))); // Restricted to 3 chosen currencies
  }

  AddAuditEntry(data: any): void{
    const headers = new HttpHeaders().set('X-Requested-With', 'XMLHttpRequest');
    this.http.post(`${this.baseUrl}currency/audit/add`, data).subscribe()
  }

  onFieldChange(query:number){
    this.txtQueryChanged.next(query);
  }

  saveValue(value: number){
    this._localStorage.setItem('currentValue', value.toString());
  }
}