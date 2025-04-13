import { Component, DestroyRef, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
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
  styleUrl: './recipe-picture.component.scss'
})
export class RecipePictureComponent {
  #imagesService = inject(ImagesService);
  #dr = inject(DestroyRef);

  @ViewChild('fileInput') fileInput!: ElementRef;
  @Input() editable = false;
  @Input() recipePicture = '';
  @Output() pictureUpdated = new EventEmitter<string>();

  isUploading = false;

  triggerFileInput() {
    if (this.editable) {
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
          this.pictureUpdated.emit(response.secureUrl);
        },
        error: (error) => {
          this.isUploading = false;
          console.error('Error uploading image:', error);
        }
      });
  }

}
