import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  #headerVisible$ = new BehaviorSubject(true);

  get visibility$() {
    return this.#headerVisible$.asObservable();
  }

  hideHeader(): void {
    this.#headerVisible$.next(false);
  }

  showHeader(): void {
    this.#headerVisible$.next(true);
  }
}
