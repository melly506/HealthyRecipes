import { MatSnackBarConfig } from '@angular/material/snack-bar';

export const sbConfig: MatSnackBarConfig = {
  horizontalPosition: 'left',
  panelClass: 'app-snackbar-default',
  duration: 4200
};

export const sbError: MatSnackBarConfig = {
  duration: 4800,
  panelClass: 'app-snackbar-error',
  horizontalPosition: 'left'
};

export const projectName = 'Green spoon';
