import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../../../layout/sidebar/sidebar.component';
import { TableComponent } from '../../../layout/table/table.component';
import { FormsModule } from '@angular/forms';
import { CandidatureService } from '../../../services/candidature.service';
import { PosteService } from '../../../services/poste.service';
import { SafeUrlPipe } from '../../../safe-url.pipe';
import { DecisionService } from '../../../services/decision.service';

interface PosteDetail {
  id: number;
  intitule: string;
  critereClassement: string;
  nombrePostes: number;
  typePoste: {nom: string, noteMinimum: number, classementMinimum: number, nbrExperienceMinimum: number};
}

interface Decision {
  reference: string;
}

interface Candidate {
  id: number;
  numero: string;
  nomPrenom: string;
  statutDossier: 'Complet' | 'Incomplet' | 'En attente';
  note: number;
  nbrExperience: number;
  ranking: number;
  appliedDate?: string;
  origin?: string;
  avatarUrl?: string;
  documents: {id: number, nom: string; typeDoc: 'CV' | 'CL' }[];
  commentaires: string;
  diplomas?: { intitule: string; etablissement: string; annee: number }[];
  experiences?: {
    poste: string;
    societe: string;
    dateDebut: string;
    dateFin: string;
    taches: string;
  }[];
  JoursAbsence?: number;
  DateRetraite?: string;
  conforme: 1 | 2;
}


@Component({
  selector: 'app-traitement',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, TableComponent, SafeUrlPipe],
  templateUrl: './traitement.component.html',
  styleUrls: ['./traitement.component.scss']
})
export class TraitementComponent implements OnInit {
  posteId!: number;
  decisionId!: number;
  candidates: Candidate[] = [];
  filtered: Candidate[] = [];

  activeTab: string = 'application'; // active tab in the sidebar

  posteDetail?: PosteDetail;      
  decision?: Decision;       
  pdfUrl: string | null = null;
  selectedModalCandidate?: Candidate;
  isModalOpen = false;

  traitementDossierCount: number = 0;
  traitementConformiteCount: number = 0;

  // Filters
  searchTerm: string = '';
  filterDossier: string = 'Tous';
  filterSAP: string = 'Tous';

  // Drawer
  selectedCandidate?: Candidate;
  isDrawerOpen: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private candidatureService: CandidatureService,
    private posteService: PosteService,
    private decisionService: DecisionService
  ) {}

  ngOnInit(): void {
    this.decisionId = Number(this.route.snapshot.paramMap.get('id'));
    this.posteId = Number(this.route.snapshot.paramMap.get('posteId'));
    this.loadCandidates();
    this.loadPosteDetail();
    this.loadDecision();
  }

  loadDecision() {
    this.decisionService.getDecision(this.decisionId).subscribe((decision: any) => 
      this.decision = {
        reference: decision.reference
      });
    console.log(this.decision);
  }

  private loadPosteDetail() {
    this.posteService.getById(this.posteId)
      .subscribe((p: any) => this.posteDetail = {
        id: p.id,
        intitule: p.description,
        critereClassement: p.critere,
        nombrePostes: p.nombrePostes,
        typePoste: {
          nom: p.typePoste.nom,
          noteMinimum: p.typePoste.noteMinimum,
          classementMinimum: p.typePoste.classementMinimum,
          nbrExperienceMinimum: p.typePoste.nbrExperienceMinimum
        }
      });
      this.loadCandidates();
      console.log(this.posteDetail);
  }


  private loadCandidates(): void {
    this.candidatureService.getByPoste(this.posteId)
      .subscribe(list => {
        // mappe d’abord
        const mapped = list.map(c => {
          const u = c.Candidat!.Utilisateur!;
          const exp = c.Candidat!.Experiences ?? [];
          const dip = c.Candidat!.Diplomes ?? [];
          const statut: 'Complet'|'Incomplet'|'En attente' = c.EstComplete ? 'Complet' : 'Incomplet';
          return {
            id: c.Id,
            numero: c.NumeroEnregistrement,
            origin: c.Candidat!.PosteOccupe,
            appliedDate: new Date(c.DatePostulation).toISOString().split('T')[0],
            nomPrenom: `${u.Prenom} ${u.Nom}`,
            statutDossier: c.EstComplete ? 'Complet' : 'Incomplet',
            note: c.Candidat!.Note ?? 0,
            nbrExperience: c.Candidat!.NbrExperience ?? 0,
            ranking: c.Candidat!.Catégorie,             // on calculera ensuite
            documents: [],
            JoursAbsence: c.Candidat!.JoursAbsence,
            DateRetraite: c.Candidat!.DateRetraite,
            experiences: exp.map((e: any) => ({
              poste: e.Poste  || '',
              societe: e.Entreprise || '',
              dateDebut: new Date(e.DateDébut).toISOString().split('T')[0],
              dateFin:   new Date(e.DateFin).toISOString().split('T')[0],
              taches: e.Description || ''
            })),
            diplomas: dip.map((d: any) => ({
              intitule: d.Intitulé || '',
              etablissement: d.Etablissement || '',
              annee: d.AnnéeObtention
            })),
            commentaires: '',
            conforme:
            (c.Candidat!.NbrExperience  >= this.posteDetail!.typePoste.nbrExperienceMinimum
          && c.Candidat!.Catégorie      >= this.posteDetail!.typePoste.classementMinimum
          && (c.Candidat!.Note ?? 0)     >= this.posteDetail!.typePoste.noteMinimum
          && statut === 'Complet'
            ) ? 2 : 1

          } as Candidate;
        });

        console.log(mapped);

        this.candidates = mapped;
        this.applyFilters();
      });
  }

  private loadDocuments(candId: number): void {
    this.candidatureService.getPJ(candId).subscribe((pjArr: any[][]) => {
      console.log(pjArr);
      // flatten nested arrays
      const flat = pjArr.flat();
      const docs = flat.map(p => ({
        id: p.id,
        nom: p.nom as string,
        typeDoc: p.typeDoc === 0 ? 'CV' : 'CL' as 'CV' | 'CL'
      }));
      if (this.selectedModalCandidate?.id === candId) {
        this.selectedModalCandidate.documents = docs;
      }
    });
  }

  get totalCandidates(): number {
    return this.candidates.length;
  }

  // get traitementDossierCount(): number {
  //   return this.candidates.filter(c => c.CVStatus === 0 || c.CVStatus === 2).length;
  // }

  // get traitementConformiteCount(): number {
  //   return this.candidates.filter(c =>
  //     ((c.Conforme === 1 || c.Conforme === 2) && c.CVStatus === 0)
  //     || c.CVStatus === 0
  //     || ((c.Conforme === 1 || c.Conforme === 2) && c.CVStatus === 2)
  //   ).length;
  // }


  previewDocument(pjId: number): void {
    this.candidatureService.previewPJ(pjId).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      this.pdfUrl = url;
      // revoke old URL if any

    });
  }


  onResumeTab() {
    this.activeTab = 'resume';
    // clear any previous PDF
    this.pdfUrl = null;
  
    // find the first CV
    const cv = this.selectedModalCandidate?.documents.find(d => d.typeDoc === 'CV');
    if (cv) {
      this.previewDocument(cv.id);
    }
  }
  

  submitForm() {
    if (!this.selectedModalCandidate) return;
    const dto = {
      id: this.selectedModalCandidate.id,
      statut: this.selectedModalCandidate.statutDossier,
      commentaires: this.selectedModalCandidate.commentaires
    };
    // this.candidatureService.updateStatus(dto).subscribe(() => {
    //   // close modal and refresh
    //   this.closeModal();
    //   this.applyFilters();
    // });
  }
  


  applyFilters(): void {
    this.filtered = this.candidates.filter(c => {
      const matchesSearch = this.searchTerm
        ? c.nomPrenom.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          c.numero.includes(this.searchTerm)
        : true;
      const matchesDossier = this.filterDossier === 'Tous' || c.statutDossier === this.filterDossier;
      return matchesSearch && matchesDossier;
    });
  }

  onView(c: Candidate) {
    this.selectedModalCandidate = c;
    this.isModalOpen = true;
    this.loadDocuments(c.id); // charger les documents
    // charger documents si besoin…
  }

  onValidate(c: Candidate): void {
    // this.candidatService.validate(c.id).subscribe(() => {
    //   c.statutDossier = 'Complet';
    //   this.applyFilters();
      // TODO: show toast “Candidat conformé”
    // });
  }

  onRelance(c: Candidate): void {
    // Open modal to send email
    // TODO: implement modal
    console.log('Relance for', c);
  }

  validateAll() {
    this.filtered
      .filter(c => c.statutDossier === 'Complet')
      .forEach(c => this.onValidate(c));
  }
  
  relanceIncomplets() {
    this.filtered
      .filter(c => c.statutDossier !== 'Complet')
      .forEach(c => this.onRelance(c));
  }

  onCloseDrawer(): void {
    this.isDrawerOpen = false;
    this.selectedCandidate = undefined;
  }

  onReactivate(c: Candidate): void {
    // this.candidatureService.reactivate(c.id).subscribe(() => {
    //   c.statutDossier = 'Incomplet';
    //   this.applyFilters();
    // });
  }

  onDeny(c: Candidate): void {
    // this.candidatureService.deny(c.id).subscribe(() => {
    //   c.statutDossier = 'Incomplet';
    //   this.applyFilters();
    // });
  }



  goBack(): void {
    const decisionId = this.route.snapshot.paramMap.get('id');
    this.router.navigate(['/dao/candidature/', decisionId]);
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedModalCandidate = undefined;
  }
}
