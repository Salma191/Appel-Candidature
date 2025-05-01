  import { Component } from '@angular/core';
  import { CommonModule } from '@angular/common';
  import { Breadcrumb, TableAction, TableColumn, TableData } from '../../../layout/table/table.component';
  import { DecisionService } from '../../../services/decision.service';
  import { PosteService } from '../../../services/poste.service';
  import { Router } from '@angular/router';

  @Component({
    selector: 'app-verification-candidatures',
    imports: [
      CommonModule
    ],
    templateUrl: './verification-candidatures.component.html',
    styleUrl: './verification-candidatures.component.scss'
  })
  export class VerificationCandidaturesComponent {
    hoverStates: { [actionName: string]: boolean } = {};

  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  decisionId: number = 0;
    phase : string = '';
    columns: TableColumn[] = [
      { key: 'postes', label: 'Postes'},
      { key: 'entite', label: 'Entité' },
      { key: 'dateDecision', label: 'Date Décision' },
      { key: 'refDecision', label: 'Ref. Décision' },
      { key: 'statut', label: 'Statut' },
      { key: 'dateSig', label: 'Date Signature' },
      { key: 'cand', label: 'Max Candidats' },
      { key: 'datePub', label: 'Date Publication' },
      { key: 'dateLim', label: 'Limite Candidature' },
      { key: 'signees', label: 'Pièces Signées' },
      { key: 'supp', label: 'P.J. Supp.' },
    ];

    columnsPostes: TableColumn[] = [
      { key: 'entite', label: 'Entité' },
      { key: 'type', label: 'Type de poste' },
      { key: 'numero', label: 'Numero Unique' },
      { key: 'cand', label: 'Candidature' },
    ];

    actions: TableAction[] = [
      { name: 'add_attachment', icon: 'fa fa-paperclip', color: 'blue' },
    ];

    actionsPoste: TableAction[] = [
      { name: 'traite', icon: 'fa fa-edit', color: 'orange' },
    ];

    breadcrumb: Breadcrumb = {
      title: 'Gestion des PV et Décisions',
      subTitle: 'Décisions',
      name: 'Decision',
    };


    data: TableData[] = [];
    items: TableData[] = [];

    constructor(private decisionService: DecisionService, private posteService: PosteService, private router: Router) {  }

    ngOnInit(): void {
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
              this.pjSignee(decision['id']);
            });
            console.log('Decisions chargés:', this.data);
          }
        },
        error => {
          console.error('Erreur lors du chargement des Decision:', error);
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


    // Fonction pour gérer l'ouverture/fermeture du dropdown
    toggleDropdown(event: {rowIndex: number, item: TableData}): void {
      // Fermer tous les autres dropdowns d'abord
      this.data.forEach((item, index) => {
        if (index !== event.rowIndex) {
            item['isOpen'] = false;
        }
    });

      this.data[event.rowIndex]['isOpen'] = !this.data[event.rowIndex]['isOpen'];
      this.decisionId = event.item['id'];
      console.log("Decision", event.item['id']);

      this.posteService.getPostesByDecision(this.decisionId).subscribe(
        (response: any[]) => {
          console.log('Réponse API:', response);
          if (response && response.length > 0) {
            this.items = response.map( poste => {
              return {
                id: poste.id,
                entite: poste.entiteNom,
                type: poste.typePosteNom,
                numero: poste.numeroUnique,
                cand: poste.candidatures
              };
              });
            }
          },
          error => {
            console.error('Erreur lors du chargement des Postes:', error);
          }
        );
    }
    
    

    toggleMenu(rowIndex: number, colKey: string) {
      if (!this.isComiteOpen[rowIndex]) {
        this.isComiteOpen[rowIndex] = {};
      }
      this.isComiteOpen[rowIndex][colKey] = !this.isComiteOpen[rowIndex][colKey];
    }

    pjSignee(id: number) {
      this.decisionService.getDecisionPJ(id).subscribe(
        (response: any) => {
          console.log(`Réponse API pour Decision ${id}:`, response);
    
          if (response) {
            const signeesNoms = response.signees.map((pj: any) => pj);
            const suppNoms = response.supp.map((pj: any) => pj);
    
            // Trouver le PV correspondant dans this.data et mettre à jour ses valeurs
            const pvIndex = this.data.findIndex(pv => pv['id'] === id);
            if (pvIndex !== -1) {
              this.data[pvIndex]['signees'] = signeesNoms;
              this.data[pvIndex]['supp'] = suppNoms;
            }
          }
        },
        error => {
          console.error(`Erreur lors du chargement des PJ pour PV ${id}:`, error);
        }
      );
    }

    onTraiter(poste: TableData): void {
      this.router.navigate([
        '/dao/candidature',
        this.decisionId,
        'postes',
        poste['id'],
        'verify'
      ]);
    }
    

  }
