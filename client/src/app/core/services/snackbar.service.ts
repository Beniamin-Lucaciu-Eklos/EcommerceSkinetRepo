import { inject, Injectable, signal } from '@angular/core';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  private snackbar = inject(MatSnackBar);
  private duration: number = 2000;

  private horizontalPosition = signal<MatSnackBarHorizontalPosition>('right');
  private verticalPosition = signal<MatSnackBarVerticalPosition>('top');

  error(message: string) {
    this.snackbar.open(message, 'Close', {
      horizontalPosition: this.horizontalPosition(),
      verticalPosition: this.verticalPosition(),
      duration: this.duration,
      panelClass: ['snack-error']
    })
  }

  success(message: string) {
    this.snackbar.open(message, 'Close', {
      horizontalPosition: this.horizontalPosition(),
      verticalPosition: this.verticalPosition(),
      duration: this.duration,
      panelClass: ['snack-success']
    })
  }
}
