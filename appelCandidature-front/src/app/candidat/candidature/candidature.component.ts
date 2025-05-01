import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { CandidatureService } from '../../services/candidature.service';
import { CommonModule } from '@angular/common';
import { TableComponent } from '../../layout/table/table.component';

@Component({
  selector: 'app-candidature',
  standalone: true,
  imports: [
    SidebarComponent,
    CommonModule,
    TableComponent
  ],
  templateUrl: './candidature.component.html',
  styleUrl: './candidature.component.scss'
})
export class CandidatureComponent {

  constructor(private candidatureService: CandidatureService) { }

  candidatures: any[] = [];

  ngOnInit(): void {
    this.candidatureService.getByCandidat().subscribe((data) => {
      this.candidatures = data.map((candidature: any) => ({
        id: candidature.id,
        num: candidature.numeroEnregistrement,
        dateCandidature: candidature.datePostulation,
        poste: candidature.poste?.description || 'Non spécifié',
        etat: candidature.statut,
        decision: candidature.decision?.reference || 'Non spécifiée',
        isOpen: false
      }));
    });
  }
}
