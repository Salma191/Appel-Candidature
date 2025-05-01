import { Component } from '@angular/core';
import { DecisionService } from '../../services/decision.service';
import { forkJoin } from 'rxjs';
import { CommonModule } from '@angular/common';
import { DecisionEditComponent } from './decision-edit/decision-edit.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { DecisionLanceComponent } from './decision-lance/decision-lance.component';
import { SafeUrlPipe } from '../../safe-url.pipe';

@Component({
  selector: 'app-decision',
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule,
    DecisionEditComponent,
    DecisionLanceComponent,
    SafeUrlPipe
  ],
  templateUrl: './decision.component.html',
  styleUrl: './decision.component.scss'
})
export class DecisionComponent {

  isModalOpen: boolean = false;
  isModalSignOpen: boolean = false;
  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  phase : string = '';
  selectedId: number = 0;
  ref : string = '';
  pdfUrl: string | null = null;
  isViewerOpen = false;

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
    { key: 'phase', label: 'Phase' },
    { key: 'signees', label: 'Pièces Signées' },
    { key: 'supp', label: 'P.J. Supp.' },
  ];

  actions: TableAction[] = [
    { name: 'edit', icon: 'fa fa-edit', color: 'orange' },
    { name: 'delete', icon: 'fa fa-window-close', color: 'red' },
    { name: 'add_attachment', icon: 'fa fa-paperclip', color: 'blue' },

  ];

  breadcrumb: Breadcrumb = {
    title: 'Gestion des PV et Décisions',
    subTitle: 'Décisions',
    name: 'Decision',
  };


  constructor(private decisionService: DecisionService) {  }

  ngOnInit(): void {
    this.getDecisions();

    this.decisionService.Refreshrequired.subscribe(() => {
      this.getDecisions();
    }
    )
  }

  getDecisions(): any {
    this.decisionService.getAll().subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);
        if (response && response.length > 0) {
          this.data = response.map(decision => ({
            id: decision.id,
            entite: decision.entite['nom'],
            dateDecision: decision.dateCreation ? new Date(decision.dateCreation).toISOString().split('T')[0] : "Non renseignée",
            dateSig: decision.dateSignature ? new Date(decision.dateSignature).toISOString().split('T')[0] : "Non renseignée",
            datePub: decision.datePublication ? new Date(decision.datePublication).toISOString().split('T')[0] : "Non renseignée",
            dateLim: decision.dateLimite ? new Date(decision.dateLimite).toISOString().split('T')[0] : "Non renseignée",
            refDecision: decision.reference,
            cand: decision.nbreMaxPosteCandidat,
            type: decision.typePoste,
            statut: decision.statut,
            generer: '',
            signees: [],
            supp: [],
            // phase: this.getPhase(decision.id)
            
          }));
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
    if (event.action === 'edit') {
      this.isModalSignOpen = true;
      console.log('Open:', this.isModalSignOpen);
      console.log('Edit:', event.item);
      this.ref = event.item['refDecision'];
      this.selectedId = event.item['id']; // Stockez l'ID du Decision
      console.log('ID de la décision sélectionnée:', this.selectedId);
    } else if (event.action === 'add_attachment') {
      this.isModalOpen = true; 
      this.selectedId = event.item['id']; 
      console.log('Piece jointe:', event.item);
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
          console.log('Pièces jointes:', signeesNoms, suppNoms);
  
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
      },
      (error) => {
        console.error('Erreur API:', error);
      }
    );
  }

  previewDocument(pjId: number): void {
    this.decisionService.previewPJ(pjId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      this.pdfUrl = url;
      this.isViewerOpen = true;

    });
  }

  closeViewer() {
    if (this.pdfUrl) {
      URL.revokeObjectURL(this.pdfUrl);
    }
    this.pdfUrl = null;
    this.isViewerOpen = false;
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
    });
  }

      
  onConfirmDec(fileData: any, dateLimite: any) {
    if (!this.selectedId) {
      console.error('Aucun ID sélectionné pour la mise à jour.');
      return;
    }
  
    try {
      let fileDataObj;
      
      if (typeof fileData === 'string') {
        fileDataObj = JSON.parse(fileData);  // Tente de parser si c'est une chaîne
      } else {
        fileDataObj = fileData;  // Si c'est déjà un objet, on l'utilise directement
      }
  
      console.log('File data object:', fileDataObj);
  
      // Appel à l'API pour mettre à jour la décision signée
      this.decisionService.putDecisionSigne(this.selectedId, fileDataObj).subscribe(
        (response: any) => {
          console.log('Réponse API:', response);
          this.getPJ(this.selectedId);
          this.isModalSignOpen = false;
        },
        (error) => {
          console.error('Erreur API:', error);
          alert('Une erreur est survenue lors de l’envoi.');
        }
      );
  
      // Appel à l'API pour mettre à jour la date limite
      this.decisionService.putDecisionLance(this.selectedId, { DateLimite: dateLimite }).subscribe(
        (response: any) => {
          console.log('Réponse API (putDecisionLance):', response);
          console.log(dateLimite);
        },
        (error) => {
          
          console.log(dateLimite);
          console.error('Erreur API (putDecisionLance):', error);
        }
      );
    } catch (error) {
      console.error('Erreur de parsing JSON:', error);
    }
  }
  


      
  onConfirmPJ(fileData: any) {
    if (!this.selectedId) {
      console.error('Aucun ID sélectionné pour la mise à jour.');
      return;
    }
      
    try {
      const fileDataObj = JSON.parse(fileData);
      console.log('File data object:', fileDataObj);
    
      this.decisionService.putDecisionPJ(this.selectedId, fileDataObj).subscribe(
        (response: any) => {
          console.log('Réponse API:', response);
      
          this.getPJ(this.selectedId);
          this.isModalOpen = false;
        },
        (error) => {
          console.error('Erreur API:', error);
          alert('Une erreur est survenue lors de l’envoi.');
        }
      );
    } catch (error) {
        console.error('Erreur de parsing JSON:', error);
      }
  }

  closeModal() {
    this.isModalOpen = false;
    this.isModalSignOpen = false;
  }
  
}
