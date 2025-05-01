import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  ngOnInit() {
  }


  formControls(controlName: string) {
    return this.loginForm.get(controlName);
  }  

    onSubmit() {
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Connexion réussie', response);
          // Exemple : stocker le token
          localStorage.setItem('token', response.token);
          alert('Connexion réussie !');
          const role = this.authService.getRole(); 
          setTimeout(() => {
            if(role === 'DAO') {
              this.router.navigate(['/dao/dashboard']);
            } else if(role === 'Candidat') {
              this.router.navigate(['/candidat/home']);
            } else {
              this.router.navigate(['/admin/users']);
            }
          }, 1000); // Le délai de 1000ms (1 seconde) est optionnel, pour afficher l'alerte avant la redirection
        },
        error: (error) => {
          console.error('Erreur de connexion', error);
          alert('Erreur de connexion !');},
      });
    }
    
}
    
