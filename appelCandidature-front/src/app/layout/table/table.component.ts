import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, Output } from '@angular/core';
import { SidebarService } from '../../sidebar.service';
import { RouterModule } from '@angular/router';
import { PvAddComponent } from '../../pv/pv-add/pv-add.component';

@Component({
  selector: 'app-table',
  imports: [
    CommonModule,
    RouterModule,
    PvAddComponent
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
  @Input() pv : boolean = false;

  constructor(private sidebarService: SidebarService) { }

  isSidebarOpen: boolean = false;
  isModalOpen: boolean = false;


  ngOnInit() {
    this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
      this.isSidebarOpen = isOpen;
    });
  }

  @Output() closed = new EventEmitter<void>();
  @Input() confirmText: string = 'Confirmer';

  close() {
    this.closed.emit();
  }

  openModal() {
  this.isModalOpen = true;
}

closeModal() {
  this.isModalOpen = false;
}

onConfirm() {
  // Logique de confirmation ici
  console.log('Action confirmée');
  this.isModalOpen = false;
}
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