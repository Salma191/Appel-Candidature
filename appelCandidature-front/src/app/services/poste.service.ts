import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PosteService {

   apiUrl = 'http://localhost:5253/api/postes';
  
    constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {}
  
    private getToken(): string | null {
      if (isPlatformBrowser(this.platformId)) {
        return localStorage.getItem('token'); // Récupérer le token
      }
      return null;
    }
  
    private _refreshrequired = new Subject<void>();
  
    get Refreshrequired() {
      return this._refreshrequired;
    }
  
  
    getAll(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(this.apiUrl, { headers });
    }

    getById(id: number): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/${id}`, { headers });
    }

    getPostesByDecision(id: number): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/byDecision/${id}`, { headers });
    }

    getActivePostes(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/appel`, { headers });
    }

    getPosteLater(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>('http://localhost:5253/api/PostesÀPostuler', { headers });
    }

    getOrg(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>('http://localhost:5253/api/organigrammes', { headers });
    }

    addPosteLater(posteId: number) : Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.post<any>(`http://localhost:5253/api/PostesÀPostuler/${posteId}`, posteId, { headers }).pipe(
        tap(() => {
          this.Refreshrequired.next();
        })
      );
    }
    
    removePosteLater(posteId: number) : Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.delete<any>(`http://localhost:5253/api/PostesÀPostuler/${posteId}`, { headers }).pipe(
        tap(() => {
          this.Refreshrequired.next();
        })
      );
    }

  }