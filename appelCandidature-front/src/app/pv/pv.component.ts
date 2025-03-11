import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../layout/table/table.component';
import { PvService } from './pv.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pv',
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule
  ],
  templateUrl: './pv.component.html',
  styleUrl: './pv.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PvComponent implements OnInit {
  columns: TableColumn[] = [
    { key: 'entite', label: 'Entité' },
    { key: 'datePv', label: 'Date PV' },
    { key: 'refDecision', label: 'Ref. Décision' },
    { key: 'poste', label: 'Poste à pourvoir' },
    { key: 'comite', label: 'Comité nominations' },
    { key: 'statut', label: 'Statut PV' },
    { key: 'generer', label: 'Generer PV' },
    { key: 'signees', label: 'P.J signees' },
    { key: 'supp', label: 'P.J supp' },
    
  ];

  actions: TableAction[] = [
    { name: 'edit', icon: 'create', color: 'blue' },
    { name: 'delete', icon: 'trash', color: 'red' },
  ];

  breadcrumb: Breadcrumb = { 
    title: 'Gestion des PV et Décisions',
    subTitle: 'PV de lancement',
    name: 'PV',
  };

  data: TableData[] = [];

  constructor(private pvService: PvService) {}

  ngOnInit(): void {
    this.pvService.getAll().subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);  // Affiche les données brutes dans la console
        if (response && response.length > 0) {
          this.data = response.map(pv => ({
            entite: pv.entite,
            datePv: pv.dateCreation,
            refDecision: pv.id,  // ou la bonne propriété pour la ref décision
            poste: pv.typePoste,
            comite: pv.commission,
            statut: pv.statut,
          }));
        } else {
          console.log('Aucune donnée retournée par l\'API');
        }
      },
      error => {
        console.error('Erreur lors du chargement des PV:', error);
      }
    );
  }
  

  onActionClicked(event: { action: string; item: TableData }) {
    if (event.action === 'edit') {
      console.log('Edit:', event.item);
    } else if (event.action === 'delete') {
      console.log('Delete:', event.item);
    }
  }

  // In your component class
get generateColumn() {
  return this.columns.find(col => col.key === 'generer');
}

get statutColumn() {
  return this.columns.find(col => col.key === 'statut');
}

  isComiteOpen: boolean = false;

  toggleMenu() {
      this.isComiteOpen = !this.isComiteOpen;
  }

}
