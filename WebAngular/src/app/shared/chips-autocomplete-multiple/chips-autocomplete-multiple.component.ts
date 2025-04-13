import {
  Component,
  forwardRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
  MatAutocompleteTrigger
} from '@angular/material/autocomplete';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { COMMA, ENTER } from '@angular/cdk/keycodes';

@Component({
  selector: 'app-chips-autocomplete-multiple',
  standalone: true,
  imports: [MatFormFieldModule, MatChipsModule, MatIconModule, MatAutocompleteModule, FormsModule],
  templateUrl: './chips-autocomplete-multiple.component.html',
  styleUrl: './chips-autocomplete-multiple.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ChipsAutocompleteMultipleComponent),
      multi: true
    }
  ]
})
export class ChipsAutocompleteMultipleComponent<T extends { name: string, id: string; }> implements OnInit, OnChanges, ControlValueAccessor {
  @ViewChild(MatAutocompleteTrigger) autocompleteTrigger!: MatAutocompleteTrigger;
  @Input() items: T[] = [];
  @Input() label: string = 'Виберіть варіанти';
  @Input() placeholder: string = 'Додати...';

  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  #selectedItems: T[] = [];
  #selectedIds: string[] = [];
  #currentSearchText: string = '';

  // Фільтруємо айтеми коли користувач вписує щось в інпут
  get filteredItems(): T[] {
    const searchText = this.#currentSearchText.toLowerCase().trim();
    return this.items.filter(item =>
      !this.#selectedIds.includes(item.id) &&
      (!searchText || item.name.toLowerCase().trim().includes(searchText))
    );
  }

  get selectedItems(): T[] {
    return this.#selectedItems;
  }

  get currentSearchText(): string {
    return this.#currentSearchText;
  }

  set currentSearchText(value: string) {
    this.#currentSearchText = value;
  }

  private onChange: any = () => {};
  private onTouched: any = () => {};

  ngOnInit(): void {
  }

  // Якщо айтеми змінились перераховуємо і перевіряємо відображення відносно обраних id
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['items'] && this.items) {
      this.#updateSelectedItemsFromIds();
      this.#currentSearchText = '';
    }
  }

  // Вибрані айтеми базуючиь на існуючих Id
  #updateSelectedItemsFromIds(): void {
    if (this.#selectedIds.length && this.items.length) {
      const newSelectedItems = this.#selectedIds
        .map(id => this.items.find(item => item.id === id))
        .filter(item => item !== undefined);

      this.#selectedItems = newSelectedItems as T[];
    }
  }

  // Додавання нового айтему користувачем
  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    if (!value) {
      return;
    }
    // Вибрати існуючий айтем котрий будемо додавати як обраний користувачем
    const matchingItem = this.items.find(item => {
      return item.name.toLowerCase() === value.toLowerCase() && !this.#selectedIds.includes(item.id)
    });

    // Якщо айтем знайдено додаємо його як обраний в список
    if (matchingItem) {
      this.addItem(matchingItem);
    }

    // Очистка інпута після додавання
    this.#currentSearchText = '';
    event.chipInput?.clear();
  }

  addItem(item: any): void {
    // Додаємо айтем тільки якщо він не був доданий раніше,
    // Щоб уникнути додавання того ж самого значення декілька разів
    if (!this.#selectedIds.includes(item.id)) {
      this.#selectedItems = [...this.#selectedItems, item];
      this.#selectedIds = [...this.#selectedIds, item.id];
      this.onChange(this.#selectedIds);
    }
  }

  // Видалення айтему користувачем
  remove(item: T): void {
    const itemId = item.id;

    // Фільтруємо массив щоб видалити бажаний елемент
    this.#selectedItems = this.#selectedItems.filter(i => i.id !== itemId);

    // Фільтруємо IDs  щоб видалити бажаний id елементу
    this.#selectedIds = this.#selectedIds.filter(id => id !== itemId);

    this.onChange(this.#selectedIds);
  }

  // Обробляємо подію, коли користувач обрав айтем з випадаючого списку
  selected(event: MatAutocompleteSelectedEvent): void {
    const selectedItem = event.option.value;
    this.addItem(selectedItem);
    this.#currentSearchText = '';
    event.option.deselect();
  }

  // Оновлюємо значення форми
  writeValue(value: string[]): void {
    if (value && Array.isArray(value)) {
      this.#selectedIds = value;
      this.#updateSelectedItemsFromIds();
    } else {
      this.#selectedIds = [];
      this.#selectedItems = [];
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}
