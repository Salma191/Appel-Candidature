import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PvService {

  apiUrl = 'http://localhost:5253/api/pvs';
    
    constructor(private http: HttpClient) {}
  
    getAll(): Observable<any[]> {
      const token = localStorage.getItem('token'); // Récupérer le token
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(this.apiUrl, { headers });
    }
}
