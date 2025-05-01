import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-pj',
  imports: [
    CommonModule
  ],
  templateUrl: './pj.component.html',
  styleUrl: './pj.component.scss'
})
export class PjComponent implements OnInit {

  @Input() entityId!: number;
  @Input() entityType!: 'Candidature' | 'PV' | 'Decision';

  piecesJointes: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadPiecesJointes();
  }

  loadPiecesJointes() {
    const url = this.getApiUrl();
    this.http.get<any[]>(url).subscribe({
      next: data => this.piecesJointes = data,
      error: err => console.error(err)
    });
  }

  getApiUrl(): string {
    switch (this.entityType) {
      case 'Candidature':
        return `/api/Candidatures/${this.entityId}/pj`;
      case 'PV':
        return `/api/Pvs/${this.entityId}/pj`;
      case 'Decision':
        return `/api/Decisions/${this.entityId}/pj`;
      default:
        throw new Error("Type d'entité inconnu");
    }
  }

  // pour ajouter un PJ plus tard...
  addPJ(file: File) {
    // Tu peux utiliser FormData pour upload si tu veux
  }
}
