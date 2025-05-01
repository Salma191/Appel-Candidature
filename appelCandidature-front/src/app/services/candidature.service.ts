import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CandidatureService {

  apiUrl = 'http://localhost:5253/api/candidatures';
    
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
        return this.http.get<any[]>(this.apiUrl, { headers });
      }

      getByPoste(id: number): Observable<any[]> {
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.get<any[]>(`${this.apiUrl}/poste/${id}`, { headers });
      }

      getByCandidat(): Observable<any> {
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.get<any>(`${this.apiUrl}/mes-candidatures`, { headers });
      }

      getPJ(id: number): Observable<any[]> {
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.get<any[]>(`${this.apiUrl}/${id}/pj`, { headers });
      }

      previewPJ(id: number): Observable<Blob> {
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.get(`${this.apiUrl}/pj/preview/${id}`, {
          responseType: 'blob'
        });
      }
      

      demarrer(data: { posteId: number; decisionId: number }): Observable<any>{
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.post(`${this.apiUrl}/demarrer`, data, { headers }).pipe(
          tap(() => {
            this.Refreshrequired.next();  
          })
        );
      }

      finaliser(id: number): Observable<any>{
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.put(`${this.apiUrl}/${id}/finaliser`, {}, { headers }).pipe(
          tap(() => {
            this.Refreshrequired.next();  
          })
        );
      }

      addPJ(id: number, pj: any ): Observable<any> {
        const token = this.getToken();
        const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
        return this.http.post(`${this.apiUrl}/${id}/pj`, pj, { headers }).pipe(
          tap(() => {
            this.Refreshrequired.next();  
          })
        );
        }

}
