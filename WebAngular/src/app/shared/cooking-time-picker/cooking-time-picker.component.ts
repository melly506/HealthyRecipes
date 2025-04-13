import { Component, DestroyRef, forwardRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatSlider, MatSliderThumb } from '@angular/material/slider';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { CookingTimeFormatPipe } from '../pipes/cooking-time-format.pipe';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-cooking-time-picker',
  imports: [
    MatSlider,
    MatSliderThumb,
    FormsModule,
    ReactiveFormsModule,
    CookingTimeFormatPipe
  ],
  standalone: true,
  templateUrl: './cooking-time-picker.component.html',
  styleUrl: './cooking-time-picker.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CookingTimePickerComponent),
      multi: true
    }
  ]
})
export class CookingTimePickerComponent implements OnInit {
  #fb = inject(FormBuilder);
  #dr = inject(DestroyRef);
  form!: FormGroup;

  totalMinutes = 0;

  onChange: any = () => {};
  onTouched: any = () => {};

  ngOnInit(): void {
    this.initForm();
    this.setupFormListeners();
  }

  initForm(): void {
    this.form = this.#fb.group({
      cookingTimeMinutes: [0],
      cookingTimeHours: [0],
      cookingTimeDays: [0]
    });
  }

  setupFormListeners() {
    this.form.valueChanges
      .pipe(
        debounceTime(20),
        takeUntilDestroyed(this.#dr)
      )
      .subscribe(values => {
        const minutes = values.cookingTimeMinutes || 0;
        const hours = values.cookingTimeHours || 0;
        const days = values.cookingTimeDays || 0;
        this.totalMinutes = minutes + (hours * 60) + (days * 24 * 60);
        this.onChange(this.totalMinutes);
        this.onTouched();
      });
  }

  writeValue(value: unknown): void {
    if (value !== undefined && value !== null) {
      this.totalMinutes = value as number;
      this.updateSliders(value as number);
    }
  }

  private updateSliders(totalMinutes: number): void {
    const days = Math.floor(totalMinutes / (24 * 60));
    const hours = Math.floor((totalMinutes % (24 * 60)) / 60);
    const minutes = totalMinutes % 60;

    this.form.patchValue({
      cookingTimeMinutes: minutes,
      cookingTimeHours: hours,
      cookingTimeDays: days
    }, { emitEvent: false }); // Предотвращаем циклические вызовы
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) {
      this.form.disable();
    } else {
      this.form.enable();
    }
  }
}
