import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { CommonModule } from '@angular/common';
import { DecisionService } from '../../services/decision.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-candidatures',
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule,
  ],
  templateUrl: './candidatures.component.html',
  styleUrl: './candidatures.component.scss'
})
export class CandidaturesComponent {
isModalOpen: boolean = false;
  isModalSignOpen: boolean = false;
  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  phase : string = '';
  selectedId: number = 0;
  ref : string = '';

  columns: TableColumn[] = [
    { key: 'entite', label: 'Entité' },
    { key: 'dateDecision', label: 'Date Décision' },
    { key: 'refDecision', label: 'Ref. Décision' },
    { key: 'dateSig', label: 'Date Signature' },
    { key: 'cand', label: 'Max Candidats' },
    { key: 'datePub', label: 'Date Publication' },
    { key: 'dateLim', label: 'Limite Candidature' },
    { key: 'signees', label: 'Pièces Signées' },
    { key: 'supp', label: 'P.J. Supp.' },
  ];

  actions: TableAction[] = [
    { name: 'view', icon: 'fa fa-eye', color: '#2563EB' }


  ];

  breadcrumb: Breadcrumb = {
    title: 'Gestion des PV et Décisions',
    subTitle: 'Décisions',
    name: 'Decision',
  };


  constructor(private decisionService: DecisionService, private router: Router) {  }

  ngOnInit(): void {
    this.getDecisions();

    this.decisionService.Refreshrequired.subscribe(() => {
      this.getDecisions();
    }
    )
  }

  getDecisions(): any {
    this.decisionService.getByPhase(2).subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);
        if (response && response.length > 0) {
          this.data = response.map(({ decision }) => {
            const { entite, dateCreation, dateSignature, datePublication, dateLimite, reference, nbreMaxPosteCandidat, statut } = decision;
            
            return {
              id:decision.id,
              entite: entite.nom,
              dateDecision: new Date(dateCreation).toISOString().split('T')[0],
              dateSig: new Date(dateSignature).toISOString().split('T')[0],
              datePub: new Date(datePublication).toISOString().split('T')[0],
              dateLim: new Date(dateLimite).toISOString().split('T')[0],
              refDecision: reference,
              cand: nbreMaxPosteCandidat,
              statut,
              postes: [],
              generer: '',
              signees: [],
              supp: [],
            };
          });   
          this.data.forEach(decision => {
            this.getPJ(decision['id']);
            this.getPhase(decision['id']);
          });
          
          console.log('Decisions chargés:', this.data);
        }
      },
      error => {
        console.error('Erreur lors du chargement des decisions:', error);
      }
    );
  }

  onActionClicked(event: { action: string; item: TableData }) {
    const id = event.item['id'];
    if (event.action === 'view') {
      this.router.navigate(['/dao/candidature', id]);

    } else {
      console.log('Action non gérée:', event.action);
    }
  }

  toggleMenu(rowIndex: number, colKey: string) {
    const currentState = this.isComiteOpen[rowIndex]?.[colKey] ?? false;

    // Si le menu est déjà ouvert, on le ferme simplement
    if (currentState) {
      this.isComiteOpen[rowIndex][colKey] = false;
      return;
    }

    console.log('Clicked on row:', rowIndex, 'and column:', colKey);
    // Fermez tous les menus dans toutes les lignes pour la colonne spécifiée
    for (const index in this.isComiteOpen) {
      for (const colKey in this.isComiteOpen[index]) {
        if (this.isComiteOpen[index][colKey]) {
          console.log(`Closing menu for row: ${index}, column: ${colKey}`);
          this.isComiteOpen[index][colKey] = false; // Ferme le menu pour toutes les lignes
        }
      }
    }
    // Assurez-vous que l'objet pour la ligne actuelle est initialisé
    if (!this.isComiteOpen[rowIndex]) {
      this.isComiteOpen[rowIndex] = {};
      console.log('Initializing isComiteOpen for row:', rowIndex);
    }
    // Basculez l'état du menu pour la ligne actuelle
    this.isComiteOpen[rowIndex][colKey] = !currentState;
    console.log(`Toggling menu for row: ${rowIndex}, column: ${colKey}. New state: ${!currentState}`);
    // Affichez l'état actuel de isComiteOpen
    console.log('Current isComiteOpen state:', this.isComiteOpen);
  }

  getPJ(id: number) {
    this.decisionService.getDecisionPJ(id).subscribe(
      (response: any) => {  
        if (response) {
          const signeesNoms = response.signees.map((pj: any) => pj);
          const suppNoms = response.supp.map((pj: any) => pj);
  
          // Trouver la Decision correspondant dans this.data et mettre à jour ses valeurs
          const decisionIndex = this.data.findIndex(decision => decision['id'] === id);
          if (decisionIndex !== -1) {
            this.data[decisionIndex]['signees'] = signeesNoms;
            this.data[decisionIndex]['supp'] = suppNoms;
          }
        }
      },
      error => {
        console.error(`Erreur lors du chargement des PJ pour Decision ${id}:`, error);
      }
    );
  }

  getPhase(id: number) {
    this.decisionService.getDecisionPhase(id).subscribe(
      (response: any) => {  
        // Mettre à jour la phase dans l'objet correspondant de la table
        const decisionIndex = this.data.findIndex(decision => decision['id'] === id);
        if (decisionIndex !== -1) {
          this.data[decisionIndex]['phase'] = response.phase;
        }
        console.log('Phase de la décision:', response.phase);
      },
      (error) => {
        console.error('Erreur API:', error);
      }
    );
  }



  closeModal() {
    this.isModalOpen = false;
    this.isModalSignOpen = false;
  }
}
