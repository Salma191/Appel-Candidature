import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { CommonModule } from '@angular/common';
import { forkJoin, Observable, timer } from 'rxjs';
import { PvEditComponent } from './pv-edit/pv-edit.component';
import { PvService } from '../../services/pv.service';
import { SafeUrlPipe } from '../../safe-url.pipe';
import { PvAddComponent } from './pv-add/pv-add.component';

@Component({
  selector: 'app-pv',
  standalone: true,
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule,
    PvEditComponent,
    SafeUrlPipe,
    PvAddComponent
  ],
  templateUrl: './pv.component.html',
  styleUrl: './pv.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PvComponent implements OnInit {

  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  isModalOpen: boolean = false;
  isModalOpenAdd: boolean = false;
  isModalSignOpen: boolean = false;
  selectedId: number = 0;
  @Input() confirmText: string = 'Confirmer';
  @Output() closed = new EventEmitter<void>();
  ref: string = '';
  pdfUrl: string | null = null;
  isViewerOpen = false;



  columns: TableColumn[] = [
    { key: 'entite', label: 'Entité' },
    { key: 'datePv', label: 'Date de Création' },
    { key: 'refDecision', label: 'Ref. Décision' },
    { key: 'type', label: 'Postes à pourvoir' },
    { key: 'comite', label: 'Comité nomination' },
    { key: 'statut', label: 'Statut PV' },
    { key: 'generer', label: 'Générer PV' },
    { key: 'signees', label: 'PV signées' },
    { key: 'supp', label: 'P.J supp' },
  ];

  breadcrumb: Breadcrumb = {
    title: 'Gestion des PV et Décisions',
    subTitle: 'PV de lancement',
    name: 'PV',
  };

  constructor(private pvService: PvService) { }

  ngOnInit(): void {
    this.getPVs();

    this.pvService.Refreshrequired.subscribe(() => {
      this.getPVs();
    });
  }


  getPVs(): any {
    this.pvService.getAll().subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);
        if (response && response.length > 0) {
          this.data = response.map(pv => ({
            id: pv.id,
            entite: pv.entite,
            datePv: new Date(pv.dateCreation).toISOString().split('T')[0],
            refDecision: pv.reference,
            type: pv.typePoste,
            comite: pv.commission,
            statut: pv.statut,
            generer: 'Générer PV',
            signees: [],
            supp: [],
          }));
          this.data.forEach(pv =>
            this.getPJ(pv['id'])
          )
          console.log('PV chargés:', this.data);
        }
      },
      error => {
        console.error('Erreur lors du chargement des PV:', error);
      }
    );
  }

  getActions(item: TableData): TableAction[] {
    const actions = [
      { name: 'add_attachment', icon: 'fa fa-paperclip', color: '#007bff' },
      { name: 'delete', icon: 'fa fa-trash', color: '#dc3545' },
      { name: 'archive', icon: 'fa fa-archive', color: '#6c757d' }
    ];

    if (item['statut'] !== 'Approuvé') {
      actions.unshift({ name: 'sign', icon: 'fa fa-check', color: '#28a745' });
    }

    return actions;
  }

  previewDocument(pjId: number): void {
    this.pvService.previewPJ(pjId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      this.pdfUrl = url;
      this.isViewerOpen = true;

      // window.open(url);               // opens in new tab for consultation
      // setTimeout(() => URL.revokeObjectURL(url), 10000);
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
      pdfBlob: this.pvService.getDownloadUrl(id),
      pvRef: this.pvService.getPV(id)
    }).subscribe(({ pdfBlob, pvRef }) => {
      const blob = new Blob([pdfBlob], { type: 'application/pdf' });
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = `PV_Lancement_${pvRef.reference}.pdf`;
      link.click();
      window.URL.revokeObjectURL(link.href); // Nettoyer l'objet URL
    }, error => {
      console.error('Download error:', error);
    });
  }

  getPJ(id: number) {
    this.pvService.getPVPJ(id).subscribe(
      (response: any) => {
        if (response) {
          const signeesNoms = response.signees.map((pj: any) => pj);
          const suppNoms = response.supp.map((pj: any) => pj);

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



  onActionClicked(event: { action: string; item: TableData }) {
    if (event.action === 'sign') {
      this.isModalSignOpen = true;
      console.log('Open:', this.isModalSignOpen);
      console.log('Sign:', event.item);
      this.ref = event.item['refDecision'];
      this.selectedId = event.item['id']; // Stockez l'ID du PV
    } else if (event.action === 'add_attachment') {
      this.isModalOpen = true;
      this.selectedId = event.item['id'];
      console.log('Piece jointe:', event.item);
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

  onConfirmPV(fileData: any) {
    if (!this.selectedId) {
      console.error('Aucun ID sélectionné pour la mise à jour.');
      return;
    }

    try {
      const fileDataObj = JSON.parse(fileData);
      console.log('File data object:', fileDataObj);
      this.pvService.putpvSigne(this.selectedId, fileDataObj).subscribe(
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

      this.pvService.putpvPJ(this.selectedId, fileDataObj).subscribe(
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

  openModal() {
    this.isModalOpenAdd= true;
    console.log('Open:', this.isModalOpenAdd);
  }

  closeModal() {
    this.isModalOpenAdd = false;
    this.isModalOpen = false;
    this.isModalSignOpen = false;
  }

  close() {
    this.closed.emit();
  }

  onConfirm() {
    // Logique de confirmation ici
    console.log('Action confirmée');
    this.isModalOpen = false;
  }


}