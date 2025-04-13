import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cookingTimeFormat',
  standalone: true
})
export class CookingTimeFormatPipe implements PipeTransform {
  transform(minutes: number): string {
    if (minutes === 0) {
      return '0хв';
    }

    const days = Math.floor(minutes / (24 * 60));
    const hours = Math.floor((minutes % (24 * 60)) / 60);
    const mins = minutes % 60;

    let result = '';

    if (days > 0) {
      result += `${days}д `;
    }

    if (hours > 0) {
      result += `${hours}г `;
    }

    if (mins > 0) {
      result += `${mins}хв`;
    }

    return result.trim();
  }
}
