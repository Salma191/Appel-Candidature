import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { CardComponent } from '../../layout/card/card.component';
import { TableComponent } from '../../layout/table/table.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DecisionDraftComponent } from '../decision/decision-draft/decision-draft.component';
import { EntretienComponent } from './entretien/entretien.component';
import { EvaluationComponent } from './evaluation/evaluation.component';
import { NominationComponent } from './nomination/nomination.component';
import { VerificationCandidaturesComponent } from './verification-candidatures/verification-candidatures.component';

@Component({
  selector: 'app-suivi-candidatures',
  imports: [
    SidebarComponent,
    TableComponent,
    RouterModule,
    CommonModule,
    EntretienComponent,
    EvaluationComponent,
    NominationComponent,
    VerificationCandidaturesComponent
  ],
  templateUrl: './suivi-candidatures.component.html',
  styleUrls: ['./suivi-candidatures.component.scss']
})
export class SuiviCandidaturesComponent {

  activeStep: string = 'Vérification des candidatures';

  steps = [
    { label: 'Vérification des candidatures', active: true, count: 12 },
    { label: 'Entretiens', active: false, count: 5 },
    { label: 'Évaluation', active: false, count: 3 },
    { label: 'Nomination', active: false }
  ];

  // Gérer l'activation dynamique des étapes
  setActiveStep(step: any) {
    this.activeStep = step.label;
    this.steps.forEach(s => s.active = false);  // Réinitialiser l'état des étapes
    step.active = true;  // Activer l'étape courante
  }

}
