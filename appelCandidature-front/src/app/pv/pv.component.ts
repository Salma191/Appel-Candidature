import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../layout/table/table.component';
import { PvService } from './pv.service';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';
import { PvEditComponent } from './pv-edit/pv-edit.component';

@Component({
  selector: 'app-pv',
  standalone: true,
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule,
    PvEditComponent
    ],
  templateUrl: './pv.component.html',
  styleUrl: './pv.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PvComponent implements OnInit {
  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  isModalOpen: boolean = false;
  isModalSignOpen: boolean = false;
  @Input() confirmText: string = 'Confirmer';
  @Output() closed = new EventEmitter<void>();
  @Input() pv : boolean = true;
  ref : string = '';


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
  
  
  
  

  breadcrumb: Breadcrumb = { 
    title: 'Gestion des PV et Décisions',
    subTitle: 'PV de lancement',
    name: 'PV',
  };


  constructor(private pvService: PvService) {}

  ngOnInit(): void {
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
            comite: pv.commissionPresident,
            statut: pv.statut,
            generer: '<bouton ici>',
            signees: [],
            supp: [],
          }));
          this.data.forEach(pv =>
            this.pjSignee(pv['id'])
          )
          console.log('PV chargés:', this.data);
        }
      },
      error => {
        console.error('Erreur lors du chargement des PV:', error);
      }
    );
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

    pjSignee(id: number) {
      this.pvService.getPVPJ(id).subscribe(
        (response: any) => {
          // console.log(`Réponse API pour PV ${id}:`, response);
    
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

            
  
  onActionClicked(event: { action: string; item: TableData }) {
    if (event.action === 'sign') {
      this.isModalSignOpen = true;
      console.log('Sign:', event.item);
      this.ref = event.item['refDecision'];
    } else if (event.action === 'add_attachment') {
      this.isModalOpen = true;
      console.log('Piece jointe:', event.item);
    }
  }

  toggleMenu(rowIndex: number, colKey: string) {
  if (!this.isComiteOpen[rowIndex]) {
    this.isComiteOpen[rowIndex] = {};
  }
  this.isComiteOpen[rowIndex][colKey] = !this.isComiteOpen[rowIndex][colKey];
}

close() {
  this.closed.emit();
  }

  // onConfirmPV(id: number, pj: any) {
  //   this.pvService.pvSigne(id, pj).subscribe(
  //     (response: any) => {
  //       console.log('Réponse API:', response);
  //       this.pjSignee(id);
  //       this.isModalSignOpen = false;
  //     }
  //   );
  //   this.isModalOpen = false;
  //   }

  onConfirmPV() {

    this.isModalOpen = false;
    }

    onConfirmPJ() {
      this.isModalOpen = false;
    }

closeModal() {
  this.isModalOpen = false;
  this.isModalSignOpen = false;
  }

  


}