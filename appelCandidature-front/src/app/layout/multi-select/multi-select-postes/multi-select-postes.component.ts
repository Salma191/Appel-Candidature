import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, HostListener, inject, Input, Output } from '@angular/core';

interface SelectItem {
  id: number;
  description: string;
  exigence: string;
  datePublication: string;
  numeroUnique: string;
  critere: string;
}

@Component({
  selector: 'app-multi-select-postes',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './multi-select-postes.component.html',
  styleUrls: ['./multi-select-postes.component.scss'],
})

export class MultiSelectPostesComponent {

  @Input() items: SelectItem[] = [];
  @Output() selectionChange = new EventEmitter<SelectItem[]>();

  isDropdownVisible = false;
  filteredItems: SelectItem[] = [];
  selectedItems: Set<number> = new Set(); // Utilisez un Set pour suivre les IDs sélectionnés

  private elementRef = inject(ElementRef);

  ngOnInit() {
    this.filteredItems = this.items;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (this.elementRef.nativeElement.contains(event.target)) {
      this.isDropdownVisible = !this.isDropdownVisible;
    } else {
      this.isDropdownVisible = false;
    }
  }

  filterItems(event: Event) {
    const searchItem = (event.target as HTMLInputElement).value.toLocaleLowerCase();
    this.filteredItems = this.items.filter(item => item.description.toLowerCase().includes(searchItem));
  }

  toggleItem(item: SelectItem, event: MouseEvent) {
    event.stopPropagation();
    if (this.selectedItems.has(item.id)) {
      this.selectedItems.delete(item.id); // Désélectionner l'élément
    } else {
      this.selectedItems.add(item.id); // Sélectionner l'élément
    }
    this.selectionChange.emit(this.getSelectedItems());
  }

  removeItem(item: SelectItem, event: MouseEvent) {
    event.stopPropagation();
    this.selectedItems.delete(item.id); // Désélectionner l'élément
    this.selectionChange.emit(this.getSelectedItems());
  }

  getSelectedItems(): SelectItem[] {
    return this.items.filter(item => this.selectedItems.has(item.id)); // Retourne les éléments sélectionnés
  }

  trackById(index: number, item: SelectItem): number {
    return item.id; // Utilisez l'id comme clé unique
  }

}