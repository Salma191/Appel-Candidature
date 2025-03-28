import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PvService {

  apiUrl = 'http://localhost:5253/api';
  
    
    constructor(private http: HttpClient) {}
  
    getAll(): Observable<any[]> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(`${this.apiUrl}/pvs`, { headers });
    }

    getPV(id: number): Observable<any> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any>(`${this.apiUrl}/pvs/${id}`, { headers });
    }

    getPVPJ(id: number): Observable<any> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any>(`${this.apiUrl}/pvs/pj/${id}`, { headers });
    }

    getDownloadUrl(id: number): Observable<Blob> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<Blob>(`${this.apiUrl}/pvs/generate/${id}`, { responseType: 'blob' as 'json', headers });
    }

    getAllEntites(): Observable<any[]> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(`${this.apiUrl}/entites`);
      }

      getAllTypes(): Observable<any[]> {
        const token = localStorage.getItem('token'); // Récupérer le token
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return this.http.get<any[]>(`${this.apiUrl}/typePostes`);
        }

        getPostesByType(id: number): Observable<any[]> {
          const token = localStorage.getItem('token'); // Récupérer le token
          const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
          console.log(`Appel API pour postes avec typeId: ${id}`);
          return this.http.get<any[]>(`${this.apiUrl}/postes/type/${id}`);
          }

          createPV(pv: any): Observable<any> {
            const token = localStorage.getItem('token'); // Récupérer le token
            const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
            return this.http.post<any>(`${this.apiUrl}/pvs`, pv, { headers });
          
        }

        pvSigne(id: number, pj: any): Observable<any> {
          const token = localStorage.getItem('token'); // Récupérer le token
          const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
          return this.http.put<any>(`${this.apiUrl}/pvs/${id}`, pj, { headers });
          }

        getRoleCommission(): Observable<any[]> {
          const token = localStorage.getItem('token'); // Récupérer le token
          const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
          return this.http.get<any[]>(`${this.apiUrl}/membreCommissions/role`);
          }
}
