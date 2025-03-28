import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiUrl = 'http://localhost:5253/api/utilisateurs';
    
      
      constructor(private http: HttpClient) {}
    
      getUsersMbre(): Observable<any[]> {
        const token = localStorage.getItem('token'); // Récupérer le token
        const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
        return this.http.get<any[]>(`${this.apiUrl}/commission`, { headers });
      }
}
