import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5253/api/auth/login';

  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {}

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<{ Token: string; prenom: string; nom: string; id: number }>(this.apiUrl, credentials).pipe(
      tap(response => {
        if (isPlatformBrowser(this.platformId)) {
          localStorage.setItem('token', response.Token); // Stockage sécurisé côté client
          localStorage.setItem('prenom', response.prenom); // Optionnel
          localStorage.setItem('role', response.nom); // Optionnel
          localStorage.setItem('id', response.id.toString())
        }
      }),
      catchError(error => {
        console.error('Erreur de connexion:', error);
        throw error;
      })
    );
  }

  signup(userData: { nom: string; prenom: string; email: string; password: string }): Observable<any> {
    return this.http.post<any>('http://localhost:5253/api/auth/register', userData).pipe(
      tap(response => {
        if (isPlatformBrowser(this.platformId)) {
          localStorage.setItem('token', response.Token);
        }
        console.log(response);
        }),
      catchError(error => {
        console.error('Erreur d\'inscription:', error);
        throw error;
      })
    );
  }

  getPrenom(): string | null {
    return isPlatformBrowser(this.platformId) ? localStorage.getItem('prenom') : null;
  }

  getRole(): string | null {
    return isPlatformBrowser(this.platformId) ? localStorage.getItem('role') : null;
  }

  getEmail(): string | null {
    return isPlatformBrowser(this.platformId) ? localStorage.getItem('email') : null;
  }
  
  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
      localStorage.removeItem('prenom');
    }
  }

  isAuthenticated(): boolean {
    return isPlatformBrowser(this.platformId) && !!localStorage.getItem('token');
  }
}
