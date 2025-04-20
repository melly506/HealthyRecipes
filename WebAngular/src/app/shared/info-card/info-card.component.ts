import { Component, Input } from '@angular/core';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-info-card',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    MatIcon
  ],
  templateUrl: './info-card.component.html',
  styleUrl: './info-card.component.scss'
})
export class InfoCardComponent {
  @Input() color = 'green';
  @Input() cardTitle = '';
  @Input() secondaryTitle = '';
  @Input() cardIcon = '';
  @Input() primaryValue: string | null = '';
  @Input() secondaryValue: string | null = '';
  @Input() subValue : string | null = '';
  @Input() secondarySubValue : string | null = '';
  @Input() footerText = '';
}
