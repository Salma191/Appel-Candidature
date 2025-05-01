import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup',
  imports: [
    RouterModule,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent {
  registerForm: FormGroup;
  
    constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
      this.registerForm = this.fb.group({
        nom: ['', [Validators.required]],
        prenom: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]]
      });
    }
  
  
    formControls(controlName: string) {
      return this.registerForm.get(controlName);
    }  
  
      onSubmit() {
        this.authService.signup(this.registerForm.value).subscribe({
          next: (response) => {
            console.log('Connexion réussie', response);
            alert('Connexion réussie !');
            setTimeout(() => {
              this.router.navigate(['/verify-email']);
            }, 1000);
          },
          error: (error) => {
            console.error('Erreur de connexion', error);
            alert('Erreur de connexion !');},
        });
      }
}
