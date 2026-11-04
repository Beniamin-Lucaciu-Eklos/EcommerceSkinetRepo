import { Component, inject, NgModule } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss',
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA) as { selectBrands: string[], selectTypes: string[] };

  selectedBrands: string[] = this.data.selectBrands;
  selectedTypes: string[] = this.data.selectTypes;

  applyFilters() {
    this.dialogRef.close({
      selectBrands: this.selectedBrands,
      selectTypes: this.selectedTypes
    });
  }
}
