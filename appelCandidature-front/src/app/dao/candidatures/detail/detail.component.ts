// src/app/dao/candidatures/detail/detail.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DecisionService } from '../../../services/decision.service';
import { PosteService } from '../../../services/poste.service';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../../layout/sidebar/sidebar.component';
import { TableComponent } from '../../../layout/table/table.component';

interface Decision {
  id: number;
  reference: string;
  entite: string;       
  statut: string; 
  dateCreation: string;
  dateLimite: string;
}

interface ApiPoste {
  id: number;
  numeroUnique: string;
  typePosteNom: string;
  entiteNom: string;
  candidatures: number;
}

interface Poste {
  id: number;
  intitule: string;
  type: string;
  candidatsTotal: number;
  candidatsValides: number;
}

@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [ CommonModule, SidebarComponent, TableComponent ],
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  decision!: Decision;
  postes: Poste[] = [];
  stats = { total: 0, incomplete: 0, validated: 0 };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private decisionService: DecisionService,
    private posteService: PosteService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadDecision(id);
  }

  private loadDecision(id: number): void {
    this.decisionService.getDecision(id).subscribe(dec => {
      console.log('Decision API:', dec);
      // map exactly the API fields:
      this.decision = {
        id:           dec.id,
        reference:    dec.reference,
        entite:     dec.entite.nom,
        statut:       dec.statut,
        dateCreation: dec.dateCreation,
        dateLimite:   dec.dateLimite,
      };
      console.log('Decision:', this.decision);
      this.loadPostes(id);
    });
  }

  private loadPostes(id: number): void {
    this.posteService.getPostesByDecision(id).subscribe((rows: ApiPoste[]) => {
      this.postes = rows.map(r => ({
        id:               r.id,
        intitule:         r.numeroUnique,
        type:             r.typePosteNom,
        candidatsTotal:   r.candidatures,
        candidatsValides: 0      // no validated count in this API—default to 0 or wire up another call
      }));
      this.stats.total = this.postes.length;
    });
  }

  goBack(): void {
    this.router.navigate(['/dao/candidature']);
  }

  onTraiter(poste: Poste): void {
    this.router.navigate([
      '/dao/candidature',
      this.decision.id,
      'postes',
      poste.id,
      'verify'
    ]);
  }

  phaseIndex(): number {
    const phases = ['Lancement','Dépôt','Vérification','Entretiens','Publication'];
    return phases.indexOf('Vérification');
  }
}
