import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  standalone: true,
  name: 'gender'
})
export class GenderPipe implements PipeTransform {

  transform(value?: string): string {
    switch (value) {
      case 'male':
        return 'Пан';
      case 'female':
        return 'Пані';
      default:
        return 'Не вказано';
    }
  }

}
