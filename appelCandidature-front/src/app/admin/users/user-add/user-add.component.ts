import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../services/user.service';

interface User {
  id?: number;
  nom: string;
  prenom: string;
  email: string;
  roleId: number;
  password: string;
}

@Component({
  selector: 'app-user-add',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.scss']
})
export class UserAddComponent {
  @Input() isOpen = false;
  @Input() title = '';
  @Input() confirmText = 'Ajouter';
  @Output() closed = new EventEmitter<void>();
  @Output() confirmed = new EventEmitter<void>();

  // form model
  nom: string = '';
  prenom: string = '';
  email: string = '';
  selectedRole!: number;
  roles: { id: number; nom: string }[] = [];
  password: string = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  private loadRoles() {
    this.userService.getRoles().subscribe(
      (data: any[]) => this.roles = data,
      err => console.error('Erreur loading roles', err)
    );
  }

  submitForm() {
    if (!this.nom || !this.prenom || !this.email || !this.selectedRole) {
      return; // or show validation error
    }
    const newUser: User = {
      nom: this.nom,
      prenom: this.prenom,
      email: this.email,
      roleId: this.selectedRole,
      password: this.password
    };

    this.userService.createUser(newUser).subscribe(
      res => {
        console.log('User created', res);
        this.confirmed.emit();
        this.close();
      },
      err => console.error('Creation error', err)
    );
  }

  close() {
    this.closed.emit();
  }
}