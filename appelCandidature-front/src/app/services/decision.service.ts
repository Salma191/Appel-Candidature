import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DecisionService {

  apiUrl = 'http://localhost:5253/api';
  
  private _refreshrequired = new Subject<void>();
    
      get Refreshrequired() {
        return this._refreshrequired;
      }
  
  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {}
  
    private getToken(): string | null {
      if (isPlatformBrowser(this.platformId)) {
        return localStorage.getItem('token'); // Récupérer le token
      }
      return null;
    }
  
    getAll(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/decisions`, { headers });
    }

    getByPhase(id: number): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/decisionphases/parPhase/${id}`, { headers });
    }

    getDecision(id: number): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any>(`${this.apiUrl}/decisions/${id}`, { headers });
    }

    getDecisionPJ(id: number): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any>(`${this.apiUrl}/decisions/pj/${id}`, { headers });
    }

    getDecisionPhase(id: number): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) :  new HttpHeaders();
      return this.http.get(`${this.apiUrl}/decisions/phase/${id}`, { headers });
    }

    getPosteAppel(): Observable<any[]> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<any[]>(`${this.apiUrl}/decisions/appel`, { headers });
    }

    getDownloadUrl(id: number): Observable<Blob> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get<Blob>(`${this.apiUrl}/decisions/generate/${id}`, { responseType: 'blob' as 'json', headers });
    }

    previewPJ(id: number): Observable<Blob> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.get(`${this.apiUrl}/decisions/pj/preview/${id}`, {
        responseType: 'blob', headers
      });
    }

    putDecisionSigne(id: number, pj: any): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.post<any>(`${this.apiUrl}/decisions/signe/${id}`, pj, { headers }).pipe(
        tap(() => {
          this.Refreshrequired.next();  
        })
      );
    }

    putDecisionLance(id: number, decision: any): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.put<any>(`${this.apiUrl}/decisions/${id}`, decision, { headers }).pipe(
        tap(() => {
          this.Refreshrequired.next();  
        })
      );
    }


    putDecisionPJ(id: number, pj: any): Observable<any> {
      const token = this.getToken();
      const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
      return this.http.post<any>(`${this.apiUrl}/decisions/pj/${id}`, pj, { headers }).pipe(
        tap(() => {
          this.Refreshrequired.next();  
        })
      );
    }
}

