import { Component, InjectionToken } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
}

export interface Currency {
  name: string;
  value: number;
}

export interface CurrencyAuditEntry {
  baseCurrency: string;
  convertedCurrency: string;
  conversionRate: number;
  multiplier: number;
  conversionValue: number;
  dateTime: moment.Moment;
}

export interface IStorage {
  readonly length: number;
  key(index: number): string;
  getItem(keyName: string): string;
  setItem(keyName: string, keyValue: string): void;
  removeItem(keyName: string): void;
  clear(): void;
}

export const BrowserLocalStorage = new InjectionToken<IStorage>('BROWSERLOCALSTORAGE', {
  providedIn: 'root',
  factory: () => window.localStorage
});
