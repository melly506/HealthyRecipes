import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { HeaderComponent } from './header/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  ngOnInit() {
    this.hideLoader();
  }

  private hideLoader(): void {
    const loader = document.getElementById('app-initial-loading');
    if (loader) {
      loader.classList.add('hidden');

      setTimeout(() => {
        loader.remove();
      }, 200);
    }
  }
}
