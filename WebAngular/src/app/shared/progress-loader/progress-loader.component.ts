import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-progress-loader',
  standalone: true,
  imports: [],
  templateUrl: './progress-loader.component.html',
  styleUrl: './progress-loader.component.scss'
})
export class ProgressLoaderComponent {
  @Input() minHeight = 60;
}
