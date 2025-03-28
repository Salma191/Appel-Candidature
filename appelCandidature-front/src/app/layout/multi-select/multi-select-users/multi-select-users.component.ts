import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, ElementRef, EventEmitter, HostListener, Input, Output, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface SelectItem {
  id: number;
  nom: string;
  prenom: string;
  role?: string;
}

@Component({
  selector: 'app-multi-select-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './multi-select-users.component.html',
  styleUrls: ['./multi-select-users.component.scss'],
})

export class MultiSelectUsersComponent {

  @Input() items: SelectItem[] = [];
  @Output() selectionChange = new EventEmitter<SelectItem[]>();

  isDropdownVisible = false;
  filteredItems: SelectItem[] = [];
  selectedItems: Set<number> = new Set();
  selectedRole: string = '';

  private elementRef = inject(ElementRef);

  ngOnInit() {
    this.filteredItems = [...this.items];
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
    const searchItem = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredItems = this.items.filter(item =>
      item.nom.toLowerCase().includes(searchItem) || item.prenom.toLowerCase().includes(searchItem)
    );
  }

  toggleItem(item: SelectItem, event: MouseEvent) {
    event.stopPropagation();
    if (this.selectedItems.has(item.id)) {
      this.selectedItems.delete(item.id);
    } else {
      this.selectedItems.add(item.id);
    }
    this.emitSelectionChange();
  }

  removeItem(item: SelectItem, event: MouseEvent) {
    event.stopPropagation();
    this.selectedItems.delete(item.id);
    this.emitSelectionChange();
  }

  emitSelectionChange() {
    this.selectionChange.emit(this.items.filter(item => this.selectedItems.has(item.id)));
  }

  trackById(index: number, item: SelectItem): number {
    return item.id;
  }

}