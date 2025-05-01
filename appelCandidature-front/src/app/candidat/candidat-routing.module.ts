import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PosteComponent } from './poste/poste.component';
import { CandidatureComponent } from './candidature/candidature.component';
import { SuiviCandidatureComponent } from './suivi-candidature/suivi-candidature.component';
import { ProfilComponent } from './profil/profil.component';
import { SauvegardeComponent } from './candidature/sauvegarde/sauvegarde.component';

const routes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'postes', component: PosteComponent },
    { path: 'candidature', component: CandidatureComponent },
    { path: 'suivi-candidature', component: SuiviCandidatureComponent },
    { path: 'profil', component: ProfilComponent },
    { path: 'candidature/sauvegarde', component: SauvegardeComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CandidatRoutingModule { }
