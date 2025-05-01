import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, Output } from '@angular/core';
import { SidebarService } from '../../services/sidebar.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-table',
  imports: [
    CommonModule,
    RouterModule,
  ],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TableComponent {
  @Input() columns: TableColumn[] = []; // Colonnes de la table
  @Input() columns2: TableColum<any>[] = []; // Colonnes de la table
  @Input() data: TableData[] = []; // Données de la table
  @Input() actions: TableAction[] = []; // Actions disponibles (éditer, supprimer, etc.)
  @Input() breadcrumb: Breadcrumb = { title: '', subTitle: '', name: '' }; // Fil d'Ariane
  @Output() actionClicked = new EventEmitter<{ action: string; item: TableData }>();
  @Input() add : boolean = false;
  @Input() bread : boolean = false;
  @Output() addClicked = new EventEmitter<void>();  


  constructor(private sidebarService: SidebarService) { }

  isSidebarOpen: boolean = false;


  ngOnInit() {
    this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
      this.isSidebarOpen = isOpen;
    });
  }

  onAddButton() {
    this.addClicked.emit();
  }



}

export interface TableColumn {
  key: string;
  label: string;
}

export interface TableColum<T> {
  key: keyof T;
  label: string;
}


export interface TableAction {
  name: string;
  icon: string;
  color?: string;
}


export interface TableData {
  [key: string]: any; // Données dynamiques
}

export interface Breadcrumb {
  title: string;
  subTitle: string;
  name: string;
  icon?: string;
}