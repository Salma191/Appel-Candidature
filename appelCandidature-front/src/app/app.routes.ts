import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

import { LoginComponent } from './pages/login/login.component';
import { AccueilComponent } from './pages/accueil/accueil.component';
import { SignupComponent } from './pages/signup/signup.component';
import { DaoRoutingModule } from './dao/dao-routing.module';
import { EmailVerificationComponent } from './pages/email-verification/email-verification.component';
import { VerificationSuccessComponent } from './pages/verification-success/verification-success.component';
import { CandidatRoutingModule } from './candidat/candidat-routing.module';
import { OrgComponent } from './pages/accueil/org/org.component';
import { AdminRoutingModule } from './admin/admin-routing.module';


export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: '', component: AccueilComponent },
    { path: 'org', component: OrgComponent },
    { path: 'sign-up', component: SignupComponent},
    { path: 'verify-email', component: EmailVerificationComponent },
    { path: 'success', component: VerificationSuccessComponent},

    { path: 'dao', loadChildren: () => import('./dao/dao.module').then(m => m.DaoModule) },
    { path: 'candidat', loadChildren: () => import('./candidat/candidat.module').then(m => m.CandidatModule) },
    { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) },



];

@NgModule({
    imports: [RouterModule.forRoot(routes),
        DaoRoutingModule,
        CandidatRoutingModule,
        AdminRoutingModule
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }