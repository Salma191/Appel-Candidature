import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../../app.routes';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { Breadcrumb, TableAction, TableColumn, TableComponent, TableData } from '../../layout/table/table.component';
import { CommonModule } from '@angular/common';
import { PvService } from '../../services/pv.service';
import { PosteService } from '../../services/poste.service';
// import { OrgchartModule } from '@dabeng/ng-orgchart';

@Component({
  selector: 'app-organigramme',
  standalone: true,
  imports: [
        SidebarComponent,
        TableComponent,
        CommonModule,
  ],
  templateUrl: './organigramme.component.html',
  styleUrl: './organigramme.component.scss'
})
export class OrganigrammeComponent {
  // ds = {
  //   id: '1',
  //   name: 'Lao Lao',
  //   title: 'general manager',
  //   children: [
  //     { id: '2', name: 'Bo Miao', title: 'department manager' },
  //     {
  //       id: '3',
  //       name: 'Su Miao',
  //       title: 'department manager',
  //       children: [
  //         { id: '4', name: 'Tie Hua', title: 'senior engineer' },
  //         {
  //           id: '5',
  //           name: 'Hei Hei',
  //           title: 'senior engineer',
  //           children: [
  //             { id: '6', name: 'Dan Zai', title: 'engineer' },
  //             { id: '7', name: 'Dan Dan', title: 'engineer' },
  //             { id: '8', name: 'Xiang Xiang', title: 'engineer' },
  //             { id: '9', name: 'Ke Xin', title: 'engineer' },
  //             { id: '10', name: 'Xiao Dan', title: 'engineer' },
  //             { id: '11', name: 'Dan Dan Zai', title: 'engineer' }
  //           ]
  //         },
  //         { id: '12', name: 'Pang Pang', title: 'senior engineer' },
  //         { id: '13', name: 'Er Pang', title: 'senior engineer' },
  //         { id: '14', name: 'San Pang', title: 'senior engineer' },
  //         { id: '15', name: 'Si Pang', title: 'senior engineer' }
  //       ]
  //     },
  //     { id: '16', name: 'Hong Miao', title: 'department manager' },
  //     { id: '17', name: 'Chun Miao', title: 'department manager' },
  //     { id: '18', name: 'Yu Li', title: 'department manager' },
  //     { id: '19', name: 'Yu Jie', title: 'department manager' },
  //     { id: '20', name: 'Yu Wei', title: 'department manager' },
  //     { id: '21', name: 'Yu Tie', title: 'department manager' }
  //   ]
  // };


  isComiteOpen: { [rowIndex: number]: { [colKey: string]: boolean } } = {};
  data: TableData[] = [];
  isModalOpen: boolean = false;
  isModalSignOpen: boolean = false;
  selectedId: number = 0;
  @Input() confirmText: string = 'Confirmer';
  @Output() closed = new EventEmitter<void>();
  ref: string = '';


  columns: TableColumn[] = [
    { key: 'sigle', label: 'Sigle' },
    { key: 'rattachement', label: 'Rattachement' },
    { key: 'poste', label: 'Poste' },
    { key: 'type', label: 'Type Poste' },
    { key: 'localite', label: 'Localité' },
    { key: 'titulaire', label: 'Titulaire' },
  ];

  breadcrumb: Breadcrumb = {
    title: 'Organigramme',
    subTitle: 'Organigramme',
    name: 'PV',
  };

  constructor(private posteService: PosteService) { }

  ngOnInit(): void {
    this.getOrg();

    // this.posteService.Refreshrequired.subscribe(() => {
    //   this.getOrg();
    // });
  }


  getOrg(): any {
    this.posteService.getOrg().subscribe(
      (response: any[]) => {
        if (response && response.length > 0) {
          this.data = response.map(org => ({
            id: org.id,
            sigle: org.sigle,
            rattachement: org.rattachementHierarchique,
            poste: org.poste,
            type: org.typePoste,
            localite: org.localite,
            titulaire: org.titulaire
          }));
        }
      },
      error => {
        console.error('Erreur lors du chargement de l Organigramme:', error);
      }
    );
  }

  getActions(item: TableData): TableAction[] {
    const actions = [
      { name: 'add_attachment', icon: 'fa fa-paperclip', color: '#007bff' },
      { name: 'delete', icon: 'fa fa-trash', color: '#dc3545' },
    ];  

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


  closeModal() {
    this.isModalOpen = false;
    this.isModalSignOpen = false;
  }

  close() {
    this.closed.emit();
  }
}
