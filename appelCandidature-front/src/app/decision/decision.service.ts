import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DecisionService {

apiUrl = 'http://localhost:5253/api';
  
    
    constructor(private http: HttpClient) {}
  
    getAll(): Observable<any[]> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(`${this.apiUrl}/decisions`, { headers });
    }

    getByPhase(id: number): Observable<any[]> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(`${this.apiUrl}/decisionphases/parPhase/${id}`, { headers });
    }

    getDecision(id: number): Observable<any> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any>(`${this.apiUrl}/decisions/${id}`, { headers });
    }

    getDecisionPJ(id: number): Observable<any> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any>(`${this.apiUrl}/decisions/pj/${id}`, { headers });
    }

    getDecisionPhase(id: number): Observable<any> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get(`${this.apiUrl}/decisions/phase/${id}`, { headers });
    }

    getDownloadUrl(id: number): Observable<Blob> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<Blob>(`${this.apiUrl}/decisions/generate/${id}`, { responseType: 'blob' as 'json', headers });
    }
}

