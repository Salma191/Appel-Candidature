import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PvService {

  apiUrl = 'http://localhost:5253/api';

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
    return this.http.get<any[]>(`${this.apiUrl}/pvs`, { headers });
  }

  getPV(id: number): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any>(`${this.apiUrl}/pvs/${id}`, { headers });
  }

  getPVPJ(id: number): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any>(`${this.apiUrl}/pvs/pj/${id}`, { headers });
  }

  getDownloadUrl(id: number): Observable<Blob> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<Blob>(`${this.apiUrl}/pvs/generate/${id}`, { responseType: 'blob' as 'json', headers });
  }

  getAllEntites(): Observable<any[]> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/entites`, { headers });
  }

  getAllTypes(): Observable<any[]> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/typePostes`, { headers });
  }

  getPostesByTypeAndEntite(entiteId: number, typeId: number): Observable<any[]> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/postes/entite/${entiteId}/type/${typeId}`, { headers });
  }

  previewPJ(id: number): Observable<Blob> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get(`${this.apiUrl}/pvs/pj/preview/${id}`, {
      responseType: 'blob', headers
    });
  }

  createPV(pv: any): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.post<any>(`${this.apiUrl}/pvs`, pv, { headers }).pipe(
      tap(() => {
        this.Refreshrequired.next();

      })
    );
  }

  putpvSigne(id: number, pj: any): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.put<any>(`${this.apiUrl}/pvs/${id}`, pj, { headers }).pipe(
      tap(() => {
        this.Refreshrequired.next();

      })
    );
  }

  putpvPJ(id: number, pj: any): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.post<any>(`${this.apiUrl}/pvs/pj/${id}`, pj, { headers }).pipe(
      tap(() => {
        this.Refreshrequired.next();

      })
    );
  }

  getRoleCommission(): Observable<any[]> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get<any[]>(`${this.apiUrl}/membreCommissions/role`, { headers });
  }
}
