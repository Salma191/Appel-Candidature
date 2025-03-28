import { Component } from '@angular/core';
import { forkJoin } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { DecisionService } from '../decision.service';

@Component({
  selector: 'app-decision-draft',
  imports: [
    TableComponent,
    SidebarComponent,
    CommonModule
  ],
  templateUrl: './decision-draft.component.html',
  styleUrl: './decision-draft.component.scss'
})
export class DecisionDraftComponent {
  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  phase : string = '';
  columns: TableColumn[] = [
    { key: 'entite', label: 'Entité' },
    { key: 'dateDecision', label: 'Date Décision' },
    { key: 'refDecision', label: 'Ref. Décision' },
    { key: 'generer', label: 'Générer Décision' },
    { key: 'statut', label: 'Statut' },
    { key: 'dateSig', label: 'Date Signature' },
    { key: 'cand', label: 'Max Candidats' },
    { key: 'datePub', label: 'Date Publication' },
    { key: 'dateLim', label: 'Limite Candidature' },
    { key: 'signees', label: 'Pièces Signées' },
    { key: 'supp', label: 'P.J. Supp.' },
  ];

  actions: TableAction[] = [
    { name: 'edit', icon: 'fa fa-edit', color: 'orange' },
    { name: 'delete', icon: 'fa fa-window-close', color: 'red' },
    { name: 'add_attachement', icon: 'fa fa-paperclip', color: 'blue' },

  ];

  breadcrumb: Breadcrumb = {
    title: 'Gestion des PV et Décisions',
    subTitle: 'Décisions',
    name: 'Decision',
  };


  data: TableData[] = [];

  constructor(private decisionService: DecisionService) {  }

  ngOnInit(): void {
    this.decisionService.getByPhase(2).subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);
        if (response && response.length > 0) {
          this.data = response.map(({ id, decision }) => {
            const { entite, dateCreation, dateSignature, datePublication, dateLimite, reference, nbreMaxPosteCandidat, typePoste, statut } = decision;
            
            return {
              id:decision.id,
              entite: entite.nom,
              dateDecision: new Date(dateCreation).toISOString().split('T')[0],
              dateSig: new Date(dateSignature).toISOString().split('T')[0],
              datePub: new Date(datePublication).toISOString().split('T')[0],
              dateLim: new Date(dateLimite).toISOString().split('T')[0],
              refDecision: reference,
              cand: nbreMaxPosteCandidat,
              type: typePoste,
              statut,
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
          const signeesNoms = response.signees.map((pj: any) => pj.nom).filter((nom: any) => nom);
          const suppNoms = response.supp.map((pj: any) => pj.nom).filter((nom: any) => nom);
  
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
  

  downloadPdf(id: number) {
        forkJoin({
          pdfBlob: this.decisionService.getDownloadUrl(id),
          decRef: this.decisionService.getDecision(id)
        }).subscribe(({ pdfBlob, decRef }) => {
          const blob = new Blob([pdfBlob], { type: 'application/pdf' });
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(blob);
          link.download = `Decision_${decRef.reference}.pdf`; // Personnaliser le nom du fichier ici
          link.click();
          window.URL.revokeObjectURL(link.href); // Nettoyer l'objet URL
        }, error => {
          console.error('Download error:', error);
          // Gérer l'erreur de manière appropriée
        });
      }
}
