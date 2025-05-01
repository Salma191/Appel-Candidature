import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CandidatService {

  private apiUrl = ' http://localhost:5253/api/candidats';
  
  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {}

  private getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('token');
    }
    return null;
  }

  getCandidats() {
    return this.http.get(this.apiUrl);
  }

  getCandidatById(id: number) {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
    return this.http.get(`${this.apiUrl}/${id}`, { headers });
  }

  getUserById(id: number): Observable<any> {
      const token = localStorage.getItem('token');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.http.get<any>(`${this.apiUrl}/user/${id}`, { headers });
    }
}
