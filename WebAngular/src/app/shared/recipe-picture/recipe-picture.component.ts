import {
  Component,
  DestroyRef,
  ElementRef,
  forwardRef,
  inject,
  Input,
  ViewChild
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ProgressLoaderComponent } from '../progress-loader/progress-loader.component';
import { MatIcon } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { ImagesService } from '../../core/services';

@Component({
  selector: 'app-recipe-picture',
  imports: [
    MatIcon,
    ProgressLoaderComponent
  ],
  standalone: true,
  templateUrl: './recipe-picture.component.html',
  styleUrl: './recipe-picture.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RecipePictureComponent),
      multi: true
    }
  ]
})
export class RecipePictureComponent {
  #imagesService = inject(ImagesService);
  #dr = inject(DestroyRef);

  @ViewChild('fileInput') fileInput!: ElementRef;
  @Input() editable = false;
  @Input() recipePicture = '';

  isUploading = false;
  disabled = false;
  touched = false;

  onChange: (value: string) => void = () => {};
  onTouched: () => void = () => {};

  triggerFileInput() {
    if (this.editable && !this.disabled) {
      this.fileInput.nativeElement.click();
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {
      const file = input.files?.item(0);
      if (file) {
        this.uploadImage(file);
      }
    }
  }

  uploadImage(file: File) {
    this.isUploading = true;

    this.#imagesService.uploadRecipeImage(file)
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe({
        next: (response) => {
          this.isUploading = false;
          this.recipePicture = response.secureUrl;
          this.onChange(this.recipePicture);
          this.onTouched();
        },
        error: (error) => {
          this.isUploading = false;
          console.error('Error uploading image:', error);
        }
      });
  }

  writeValue(url: string): void {
    this.recipePicture = url || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  // Helper method to mark control as touched
  markAsTouched(): void {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }

}
