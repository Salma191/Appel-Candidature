import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { PvService } from '../../services/pv.service';
import { DecisionService } from '../../services/decision.service';
import { TableComponent } from '../../layout/table/table.component';
import { SidebarService } from '../../services/sidebar.service';
import { PosteService } from '../../services/poste.service';
import { CandidatService } from '../../services/candidat.service';
import { CandidatureService } from '../../services/candidature.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    SidebarComponent,
    TableComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  isSidebarOpen: boolean = false;

  pvDraft: number = 0;
  pvApprouve: number = 0;
  decDraft: number = 0;
  decSigne: number = 0;
  decLance: number = 0;
  dtPv: number = 0;
  dtDec: number = 0;
  poste: number = 0;
  posteAppel: number = 0;
  posteAppelOuvert: number = 0;
  posteAppelCloture: number = 0;
  posteAppelCours: number = 0;
  cand: number = 0;
  


  constructor(private pvService: PvService, private decService: DecisionService, private sidebarService: SidebarService, private posteService: PosteService, private candidatureService: CandidatureService){}

  ngOnInit(): void {
    this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
        this.isSidebarOpen = isOpen;
      });

    this.pvService.getAll().subscribe(
      (data) => {
        this.dtPv = data.length;
        this.pvDraft = data.filter((item) => item.statut === 'Draft').length;
        this.pvApprouve = data.filter((item) => item.statut === 'Approuvé').length;
        console.log('Data:', this.dtPv, this.pvDraft, this.pvApprouve);
      },
      (error) => {
        console.error('Error:', error);
      }
    )

    this.decService.getAll().subscribe(
      (data) => {
        this.dtDec = data.length;
        this.decDraft = data.filter((item) => item.statut === 'Draft').length;
        this.decLance = data.filter((item) => item.statut === 'Lancé').length;
        this.decSigne = data.filter((item) => item.statut === 'Signée').length;
        
        console.log('Data:', this.dtDec, this.decDraft, this.decLance);
      },
      (error) => {
        console.error('Error:', error);
      }
    )

    this.posteService.getActivePostes().subscribe(
      (data) => {
        this.posteAppel = data.length;
        this.posteAppelOuvert = data.filter((item) => new Date(item.dateLimite) > new Date()).length;
        this.posteAppelCloture = data.filter((item) => new Date(item.dateLimite) < new Date()).length;
        this.posteAppelCours = this.posteAppel - this.posteAppelOuvert - this.posteAppelCloture;
        console.log('Data:', this.poste);
      },
      (error) => {
        console.error('Error:', error);
      }
    )

    this.candidatureService.getAll().subscribe(
      (data) => {
        this.cand = data.length;
        console.log('Data:', this.posteAppel);
      },
      (error) => {  
          console.error('Error:', error);
        }
      )

    this.posteService.getAll().subscribe(
      (data) => {
        this.poste = data.length;
        console.log('Data:', this.poste);
      },
      (error) => {
        console.error('Error:', error);
      }
    )
  }
}
