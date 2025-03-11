import { Component } from '@angular/core';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../layout/table/table.component';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';

@Component({
  selector: 'app-decision',
  imports: [
    SidebarComponent,
    TableComponent
  ],
  templateUrl: './decision.component.html',
  styleUrl: './decision.component.scss'
})
export class DecisionComponent {
  columns: TableColumn[] = [
    { key: 'entite', label: 'Entité' },
    { key: 'dateDecision', label: 'Date Décision' },
    { key: 'refDecision', label: 'Ref. Décision' },
    { key: 'statut', label: 'Statut' },
  ];

  actions: TableAction[] = [
    { name: 'edit', icon: '✏️', color: 'blue' },
    { name: 'delete', icon: '🗑️', color: 'red' },
  ];

  breadcrumb: Breadcrumb = {
    title: 'Gestion des PV et Décisions',
    subTitle: 'Decisions',
    name: 'Decision',
  };


  data: TableData[] = [
    {
      entite: 'Entité 1',
      dateDecision: '2023-10-01',
      refDecision: 'REF123',
      statut: 'Actif',
    },
    // Ajoutez plus de données ici
  ];

  onActionClicked(event: { action: string; item: TableData }) {
    if (event.action === 'edit') {
      console.log('Edit:', event.item);
    } else if (event.action === 'delete') {
      console.log('Delete:', event.item);
    }
  }
}
