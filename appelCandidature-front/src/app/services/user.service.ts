import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiUrl = 'http://localhost:5253/api/utilisateurs';
    
  private _refreshrequired = new Subject<void>();
  
    get Refreshrequired() {
      return this._refreshrequired;
    }
  
    constructor(private http: HttpClient) {};

    getAllUsers(): Observable<any[]> {
      const token = localStorage.getItem('token');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any[]>(`${this.apiUrl}`, { headers });
    }
    
  getUsersMbre(): Observable<any[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any[]>(`${this.apiUrl}/commission`, { headers });
  }

  getUserById(id: number): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any>(`${this.apiUrl}/${id}`, { headers });
  }

  getRoles(): Observable<any[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any[]>(`http://localhost:5253/api/roles`, { headers });
  }

  createUser(user: any): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(`${this.apiUrl}`, user, { headers }).pipe(
      tap(() => {
        this._refreshrequired.next();
      })
    );
  }
}
