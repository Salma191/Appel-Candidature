import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AccueilComponent } from './accueil/accueil.component';
import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PvComponent } from './pv/pv.component';
import { DecisionComponent } from './decision/decision.component';
import { OrganigrammeComponent } from './organigramme/organigramme.component';
import { PvAddComponent } from './pv/pv-add/pv-add.component';
import { DecisionDraftComponent } from './decision/decision-draft/decision-draft.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: '', component: AccueilComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'pv', component: PvComponent },
    { path: 'decision', component: DecisionComponent },
    { path: 'org', component: OrganigrammeComponent },
    { path: 'pv-add', component: PvAddComponent},
    { path: 'decision-draft', component: DecisionDraftComponent},

];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }