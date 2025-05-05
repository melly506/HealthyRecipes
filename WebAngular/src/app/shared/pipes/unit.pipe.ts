import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone: true,
  name: 'unit'
})
export class UnitPipe implements PipeTransform {

  transform(value?: string): string {
    switch (value) {
      case 'g':
        return 'Грам';
      case 'ml':
        return 'Мл';
      default:
        return 'Грам';
    }
  }
}
