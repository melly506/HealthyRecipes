import { Component, inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle
} from '@angular/material/dialog';
import { MatButton } from '@angular/material/button';
import { ConfirmDeleteDialogDataInterface } from './confirm-delete-dialog-data.interface';

@Component({
  selector: 'app-confirm-delete-modal',
  standalone: true,
  imports: [
    MatDialogActions,
    MatDialogContent,
    MatDialogTitle,
    MatButton,
    MatDialogClose
  ],
  templateUrl: './confirm-delete-modal.component.html',
  styleUrl: './confirm-delete-modal.component.scss'
})
export class ConfirmDeleteModalComponent {
  readonly data = inject<ConfirmDeleteDialogDataInterface>(MAT_DIALOG_DATA);
}
