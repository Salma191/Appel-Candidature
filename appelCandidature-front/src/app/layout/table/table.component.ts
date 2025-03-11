import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-table',
  imports: [
    CommonModule
  ],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TableComponent {
  @Input() columns: TableColumn[] = []; // Colonnes de la table
  @Input() data: TableData[] = []; // Données de la table
  @Input() actions: TableAction[] = []; // Actions disponibles (éditer, supprimer, etc.)
  @Input() breadcrumb: Breadcrumb = { title: '', subTitle: '', name: '' }; // Fil d'Ariane
  @Output() actionClicked = new EventEmitter<{ action: string; item: TableData }>();


}

export interface TableColumn {
  key: string;
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