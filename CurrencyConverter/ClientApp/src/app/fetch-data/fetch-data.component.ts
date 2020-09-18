import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CurrencyAuditEntry } from '../app.component';
import { Observable } from 'rxjs';
import * as moment from 'moment';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  auditEntries$: Observable<CurrencyAuditEntry[]>;
  moment: any = moment;
  from: moment.Moment = moment();
  to: moment.Moment = moment().add(1, 'days');

  constructor(private http: HttpClient, 
      @Inject('BASE_URL') private baseUrl: string) {
    this.auditEntries$ = this.getAuditEntries(this.from, this.to);
  }

  search() { 
    this.auditEntries$ = this.getAuditEntries(moment(this.from), moment(this.to));
  }

  getAuditEntries(from: moment.Moment, to: moment.Moment): Observable<CurrencyAuditEntry[]> {
    const headers = new HttpHeaders().set('X-Requested-With', 'XMLHttpRequest');
    return this.http.get<CurrencyAuditEntry[]>(`${this.baseUrl}currency/audit/${from.format("MM-DD-yyyy")}/${to.format("MM-DD-yyyy")}`, {
      headers
    });
  }
}
