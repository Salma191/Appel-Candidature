import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { PosteService } from '../../services/poste.service';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';

@Component({
  selector: 'app-poste',
  imports: [
    CommonModule,
    TableComponent,
    SidebarComponent
  ],
  templateUrl: './poste.component.html',
  styleUrl: './poste.component.scss'
})
export class PosteComponent {
 isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  isModalOpen: boolean = false;
  isModalSignOpen: boolean = false;
  selectedId: number = 0;
  @Input() confirmText: string = 'Confirmer';
  @Output() closed = new EventEmitter<void>();
  ref : string = '';


  columns: TableColumn[] = [
    { key: 'numeroUnique', label: 'Numéro Unique' },
    { key: 'description', label: 'Description' },
    { key: 'entite', label: 'Entité' },
    { key: 'type', label: 'Type de Poste' },
    { key: 'datePublication', label: 'Date de Publication' },
    { key: 'exigence', label: 'Exigence' },
    { key: 'critere', label: 'Critere' }
  ];

  breadcrumb: Breadcrumb = { 
    title: 'Gestion des Postes',
    subTitle: 'Postes',
    name: 'Poste',
  };

  constructor(private posteService: PosteService) {}

  ngOnInit(): void {
    this.getPostes();

    this.posteService.Refreshrequired.subscribe(() => {
      this.getPostes();
    });
  }


  getPostes(): any {
    this.posteService.getAll().subscribe(
      (response: any[]) => {
        console.log('Réponse API:', response);
        if (response && response.length > 0) {
          this.data = response.map(poste => ({
            id: poste.id,
            entite: poste.entite['nom'],
            type: poste.typePoste['nom'],
            datePublication: new Date(poste.datePublication).toISOString().split('T')[0],
            description: poste.description,
            exigence: poste.exigence,
            numeroUnique: poste.numeroUnique,
            critere: poste.critere,
            signees: [],
            supp: [],
          }));
          console.log('Postes chargés:', this.data);
        }
      },
      error => {
        console.error('Erreur lors du chargement des PV:', error);
      }
    );
  }

  getActions(item: TableData): TableAction[] {
    const actions = [
      { name: 'delete', icon: 'fa fa-trash', color: '#dc3545' },
    ];
  
    if (item['statut'] !== 'Approuvé') {
      actions.unshift({ name: 'sign', icon: 'fa fa-check', color: '#28a745' });
    }
  
    return actions;
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
    console.log('Clicked on row:', rowIndex, 'and column:', colKey);
    // Fermez tous les menus dans toutes les lignes pour la colonne spécifiée
    for (const index in this.isComiteOpen) {
      for(const colKey in this.isComiteOpen[index]) {
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
    const currentState = this.isComiteOpen[rowIndex][colKey];
    this.isComiteOpen[rowIndex][colKey] = !currentState;
    console.log(`Toggling menu for row: ${rowIndex}, column: ${colKey}. New state: ${!currentState}`);
    // Affichez l'état actuel de isComiteOpen
    console.log('Current isComiteOpen state:', this.isComiteOpen);
  }
  
  // onConfirmPV(fileData: any) {
  //   if (!this.selectedId) {
  //     console.error('Aucun ID sélectionné pour la mise à jour.');
  //     return;
  //   }

  //   try {
  //     const fileDataObj = JSON.parse(fileData);
  //     console.log('File data object:', fileDataObj);
  //     this.pvService.putpvSigne(this.selectedId, fileDataObj).subscribe(
  //       (response: any) => {
  //         console.log('Réponse API:', response);
  
  //         this.getPJ(this.selectedId);
  //         this.isModalSignOpen = false;
  //       },
  //       (error) => {
  //         console.error('Erreur API:', error);
  //         alert('Une erreur est survenue lors de l’envoi.');
  //       }
  //     );
  //   } catch (error) {
  //     console.error('Erreur de parsing JSON:', error);
  //   }
  // }

  // onConfirmPJ(fileData: any) {
  //   if (!this.selectedId) {
  //     console.error('Aucun ID sélectionné pour la mise à jour.');
  //     return;
  //   }
  
  //   try {
  //     const fileDataObj = JSON.parse(fileData);
  //     console.log('File data object:', fileDataObj);
  
  //     this.pvService.putpvPJ(this.selectedId, fileDataObj).subscribe(
  //       (response: any) => {
  //         console.log('Réponse API:', response);
  
  //         this.getPJ(this.selectedId);
  //         this.isModalOpen = false;
  //       },
  //       (error) => {
  //         console.error('Erreur API:', error);
  //         alert('Une erreur est survenue lors de l’envoi.');
  //       }
  //     );
  //   } catch (error) {
  //     console.error('Erreur de parsing JSON:', error);
  //   }
  // }

  closeModal() {
    this.isModalOpen = false;
    this.isModalSignOpen = false;
  }

  close() {
    this.closed.emit();
  }


}