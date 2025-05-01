import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PvComponent } from './pv/pv.component';
import { DecisionComponent } from './decision/decision.component';
import { OrganigrammeComponent } from './organigramme/organigramme.component';
import { PvAddComponent } from './pv/pv-add/pv-add.component';
import { DecisionDraftComponent } from './decision/decision-draft/decision-draft.component';
import { SuiviCandidaturesComponent } from './suivi-candidatures/suivi-candidatures.component';
import { PosteComponent } from './poste/poste.component';
import { EntretienComponent } from './suivi-candidatures/entretien/entretien.component';
import { ArchiveComponent } from './archive/archive.component';
import { CandidaturesComponent } from './candidatures/candidatures.component';
import { DetailComponent } from './candidatures/detail/detail.component';
import { TraitementComponent } from './candidatures/traitement/traitement.component';
import { ReclamationComponent } from './reclamation/reclamation.component';

const routes: Routes = [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'pv', component: PvComponent },
      { path: 'decision', component: DecisionComponent },
      { path: 'org', component: OrganigrammeComponent },
      { path: 'pv-add', component: PvAddComponent },
      { path: 'decision-draft', component: DecisionDraftComponent },
      { path: 'suivi-candidatures', component: SuiviCandidaturesComponent},
      { path: 'postes', component: PosteComponent },
      { path: 'entretien', component: EntretienComponent },
      { path: 'archive', component: ArchiveComponent},
      { path: 'candidature', component: CandidaturesComponent},
      { path: 'candidature/:id', component: DetailComponent},
      { path: 'candidature/:id/postes/:posteId/verify', component: TraitementComponent},
      { path: 'reclamation', component: ReclamationComponent},



      
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DaoRoutingModule { }
