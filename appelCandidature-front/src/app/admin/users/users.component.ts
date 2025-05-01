import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { UserService } from '../../services/user.service';
import { Breadcrumb, TableAction, TableColum, TableComponent, TableData } from '../../layout/table/table.component';
import { UserAddComponent } from './user-add/user-add.component';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}


@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    SidebarComponent,
    TableComponent,
    UserAddComponent
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  isModalOpen: boolean = false;
  users: User[] = [];

  columns: TableColum<User>[] = [
      { key: 'name', label: 'Nom' },
      { key: 'email', label: 'Email' },
      { key: 'role', label: 'Rôle' },
    ];

      breadcrumb: Breadcrumb = { 
        title: 'Gestion des Utilisateurs',
        subTitle: 'Utilisateurs',
        name: 'Utilisateurs',
      };

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();

    this.userService.Refreshrequired.subscribe(() => {
      this.loadUsers();
    });
    }


  loadUsers() {
    this.userService.getAllUsers().subscribe(users => 
      this.users = users.map(user => ({
        id: user.id,
        name: `${user.prenom} ${user.nom}`,
        email: user.email,
        role: user.role.nom
      }))
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

      console.log('Piece jointe:', event.item);
    }

    onUserCreated() {

    }


    openModal() {
      this.isModalOpen = true;
    }
  
    closeModalUser() {
      this.isModalOpen = false;
    }
  
}
