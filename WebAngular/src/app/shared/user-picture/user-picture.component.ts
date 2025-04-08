import {
  Component,
  DestroyRef,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { User } from '../../core/interfaces';
import { ImagesService } from '../../core/services/images.service';
import { ProgressLoaderComponent } from '../progress-loader/progress-loader.component';
import { UsersService } from '../../core/services';

@Component({
  selector: 'app-user-picture',
  standalone: true,
  imports: [
    ProgressLoaderComponent,
    MatIcon
  ],
  templateUrl: './user-picture.component.html',
  styleUrl: './user-picture.component.scss'
})
export class UserPictureComponent implements OnInit {
  #imagesService = inject(ImagesService);
  #usersService = inject(UsersService);
  #dr = inject(DestroyRef);

  @Input() user?: User | null = null;
  @Input() size = 40;
  @Input() editable = false;
  @Output() pictureUpdated = new EventEmitter<string>();

  @ViewChild('fileInput') fileInput!: ElementRef;

  isHovered = false;
  isUploading = false;

  ngOnInit() {
    this.#usersService.userCache
      .pipe(takeUntilDestroyed(this.#dr))
      .subscribe(userCache => {
        if (userCache?.id === this.user?.id) {
          this.user = userCache;
        }
      })
  }

  onMouseEnter() {
    if (this.editable) {
      this.isHovered = true;
    }
  }

  onMouseLeave() {
    this.isHovered = false;
  }

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

    this.#imagesService.uploadProfileImage(file)
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
