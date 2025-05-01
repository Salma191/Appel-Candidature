import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-email-verification',
  templateUrl: './email-verification.component.html',
  styleUrls: ['./email-verification.component.scss']
})
export class EmailVerificationComponent implements OnInit {
  message = 'Vérification en cours...';
  email: string = '';
  isLoading = true;  // Indicateur de chargement

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    console.log('ngOnInit: Initialisation du composant');
    
    this.email = localStorage.getItem('email') || '';
    console.log('Email récupéré de localStorage: ', this.email);

    this.route.queryParamMap.subscribe(params => {
      console.log('queryParamMap: Paramètres de requête reçus');
      const token = params.get('token');
      console.log('Token dans l\'URL: ', token);

      if (token) {
        console.log('Token trouvé, envoi de la requête HTTP pour vérifier l\'email...');
        this.http.get('http://localhost:5253/api/auth/verify-email?token=' + token, { responseType: 'text' })
  .subscribe({
    next: (response) => {
      this.message = response; // "Email vérifié avec succès."
      setTimeout(() => this.router.navigate(['/success']), 1000);
    },
    error: (error) => {
      console.error('Erreur HTTP:', error);
      this.message = 'Lien invalide ou expiré.';
    }
  });

      } else {
        console.log('Aucun token trouvé dans l\'URL');
        this.message = 'Token manquant dans l’URL.';
        this.isLoading = false; // Arrêter le chargement si pas de token
      }
    });
  }

  goToLogin() {
    console.log('Redirection vers /login');
    this.router.navigate(['/login']);
  }

  resendVerificationEmail() {
    console.log('Demande de renvoi de l\'email de vérification');
    alert('Email de vérification renvoyé !');
  }
}
