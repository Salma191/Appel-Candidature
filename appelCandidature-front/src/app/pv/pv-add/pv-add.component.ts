import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PvService } from '../pv.service';
import { MultiSelectPostesComponent } from '../../layout/multi-select/multi-select-postes/multi-select-postes.component';
import { UserService } from '../../user.service';
import { MultiSelectUsersComponent } from '../../layout/multi-select/multi-select-users/multi-select-users.component';


@Component({
  selector: 'app-pv-add',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MultiSelectPostesComponent,
    MultiSelectUsersComponent
    ],
  templateUrl: './pv-add.component.html',
  styleUrl: './pv-add.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PvAddComponent {

  @Input() isOpen = false;
  @Input() title = '';
  @Output() closed = new EventEmitter<void>();
  @Output() confirmed = new EventEmitter<void>();
  @Input() confirmText: string = '';

  types: any[] = [];
  ref: any = "";
  // selectedType: any = "";
  selectedType: any;
  entites: any[] = [];
  selectedEntite: any;
  postes: any[] = [];
  selectedPostes: any[] = [];
  users: any[] = [];
  selectedUsers: any[] = [];
  roles: any[] = [];


  constructor(
    private pvService: PvService, 
    private userService: UserService
  ) { }



  ngOnInit(): void {
    this.isOpen = true;
  this.getEntites();
  this.getTypePostes();
  this.getRoleCommission();
  this.getUsers();
}


  getEntites() {
    this.pvService.getAllEntites().subscribe(
      (data: any[]) => {
        this.entites = data;
        console.log("Entités chargées :", this.entites);
      },
      (error) => {
        console.error("Erreur lors du chargement des entités :", error);
      }
    );
  }

  getTypePostes() {
    this.pvService.getAllTypes().subscribe(
      (data: any[]) => {
        this.types = data;
        console.log("Types chargés :", this.types);
      },
      (error) => {
        console.error("Erreur lors du chargement des types :", error);
      }
    );
  }

  submitForm() {
    const newPV = {
      Reference: this.ref,
      EntiteId: this.selectedEntite,
      TypePosteId: this.selectedType,
      Postes: this.selectedPostes.map(poste => ({ Id: poste.id })),
      Commission: {
        MembreCommissions: this.selectedUsers.map(user => ({
          UtilisateurId: user.id,
          RoleCommission: user.role
        }))
      }
    };
  
    console.log("PV à envoyer :", newPV);
  
    this.pvService.createPV(newPV).subscribe(
      response => {
        console.log("PV créé avec succès :", response);
        this.close();
        this.confirmed.emit();
        this.refreshPvs();
      },
      error => {
        console.error("Erreur lors de la création du PV :", error);
      }
    );
  }
  

  onTypeChange() {
    if (this.selectedType) {
      this.getPostesByType();
    }
  }

  getPostesByType() {
    if (!this.selectedType) {
      console.warn("Aucun type sélectionné !");
      return;
    }
    console.log("Type sélectionné :", this.selectedType);
    this.pvService.getPostesByType(this.selectedType).subscribe(
      (data: any[]) => {
        this.postes = data;
        console.log("Postes chargés :", this.postes);
      },
      (error) => {
        console.error("Erreur lors du chargement des postes :", error);
      }
    );
  }

  refreshPvs() {
    this.pvService.getAll().subscribe(
      (data: any[]) => {
        console.log("PVs actualisés :", data);
        // Mettre à jour l'état de tes PVs
      },
      (error) => {
        console.error("Erreur lors de l'actualisation des PVs", error);
      }
    );
  }
  

  getUsers() {
    this.userService.getUsersMbre().subscribe(
      (data: any[]) => {
        this.users = data;
        console.log("Utilisateurs chargés :", this.users);
      }
    );
  }

  getRoleCommission() {
    this.pvService.getRoleCommission().subscribe(
      (data: any[]) => {
        this.roles = data;
        console.log("Role Commission chargés :", this.roles);
      }
    );
  }

  onSelectionChange(selectedItems: any[]) {
    this.selectedPostes = selectedItems;
    console.log('selected')
  }

  onSelectionChangeU(selectedItems: any[]) {
    this.selectedUsers = selectedItems;
    console.log('selected')
  }

  close() {
    this.closed.emit();
  }

  confirm() {
    this.confirmed.emit();
  }


}


