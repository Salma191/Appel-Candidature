import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';
import { TableComponent } from '../../layout/table/table.component';
import { DecisionService } from '../../services/decision.service';
import { FormsModule } from '@angular/forms';
import { CandidatureService } from '../../services/candidature.service';
import { PosteService } from '../../services/poste.service';
import { AlertComponent } from '../../layout/alert/alert.component';

@Component({
  selector: 'app-poste',
  imports: [
    SidebarComponent,
    CommonModule,
    TableComponent,
    FormsModule,
    AlertComponent
  ],
  templateUrl: './poste.component.html',
  styleUrl: './poste.component.scss'
})
export class PosteComponent {

  postulerAlert = false;
  pageSize = 6;
  currentPage = 1;
  postesLater: number[] = [];

  selectedCandidatureId: number | null = null;
  pjCV: File | null = null;
  pjCL: File | null = null;

  selectedEntite: string = '';
  selectedTypePoste: string = '';
  entitesDisponibles: string[] = [];
  typesPosteDisponibles: string[] = [];
  appels: any[] = [];
  appelsOriginaux: any[] = [];
  searchTerm: string = '';
  filteredAppels: any[] = [];



  constructor(private decService: DecisionService, private candidatureService: CandidatureService, private posteService: PosteService) { }
  
  ngOnInit(): void {
    this.decService.getPosteAppel().subscribe((data) => {
      const appelsBruts = data.map((appel: any) => {
        const typePoste = appel.postes?.[0]?.typePoste.nom || 'Non spécifié';
        return {
          id: appel.id,
          titre: appel.reference,
          dateDebut: appel.datePublication,
          dateFin: appel.dateLimite,
          entite: appel.entite?.nom || 'Non spécifiée',
          typePoste: typePoste,
          postes: appel.postes.map((poste: any) => ({
            id: poste.id,
            intitule: poste.description,
            affectation: poste.entite?.nom || 'Non spécifiée',
            description: poste.exigence,
          })),
          isOpen: false
        };
      });
  
      this.appelsOriginaux = appelsBruts;
      this.entitesDisponibles = [...new Set(appelsBruts.map(appel => appel.entite))];
      this.typesPosteDisponibles = [...new Set(appelsBruts.map(appel => appel.typePoste))];
      this.applyFilters();
    });
  }
  
  applyFilters(): void {
    this.currentPage = 1;
    this.filteredAppels = this.appelsOriginaux.filter(appel => {
      const matchesEntite = this.selectedEntite ? appel.entite === this.selectedEntite : true;
      const matchesTypePoste = this.selectedTypePoste ? appel.typePoste === this.selectedTypePoste : true;
      const matchesSearch = this.searchTerm
        ? appel.titre?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          appel.postes.some((poste: any) =>
            poste.intitule?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
            poste.description?.toLowerCase().includes(this.searchTerm.toLowerCase()))
        : true;
  
      return matchesEntite && matchesTypePoste && matchesSearch;
    });
  }

  resetFilters(): void {
    this.selectedEntite = '';
    this.selectedTypePoste = '';
    this.searchTerm = '';
    this.applyFilters();
  }

  openUploadForm(candidatureId: number) {
    this.selectedCandidatureId = candidatureId;
  }
  
  onFileSelected(event: any, typeDoc: 'CV' | 'CL') {
    const file = event.target.files[0];
    if (typeDoc === 'CV') this.pjCV = file;
    else if (typeDoc === 'CL') this.pjCL = file;
  
    this.uploadPieceJointe(typeDoc, file);
  }
  
  uploadPieceJointe(typeDoc: 'CV' | 'CL', file: File) {
    let typeDocNumeric: number;
    if (typeDoc === 'CV') {
      typeDocNumeric = 0;
    } else if (typeDoc === 'CL') {
      typeDocNumeric = 1;
    }
    const reader = new FileReader();
    reader.onload = () => {
      const base64 = (reader.result as string).split(',')[1];
      const pj = {
          Nom: file.name,
          Fichier: base64,
          TypeDoc: typeDocNumeric
      };
  
      this.candidatureService.addPJ(this.selectedCandidatureId!, pj).subscribe({
        next: () => console.log(`${typeDoc} envoyé.`, pj),
        error: err => {
          console.error(err);
          const message = err.error?.message || JSON.stringify(err.error) || "Erreur inconnue";
          alert("Erreur d'envoi : " + message);
        }
        
      });
    };
    reader.readAsDataURL(file);
  }
  

  postuler(posteId: number, decisionId: number): void {
    // this.candidatureService.demarrer({ posteId, decisionId }).subscribe(
    //   (response) => {
    //     console.log(response);
    //     const candidatureId = response.candidatureId;
    //     this.openUploadForm(candidatureId);
    //     this.postulerAlert = true;
    //     alert("Votre candidature a été soumise avec succès !");
    //   },
    //   (error) => {
    //     console.error('Erreur lors de la soumission de la candidature :', error);
    //     alert("❌ Une erreur est survenue lors de la soumission.");
    //   }
    // );
    const candidatureId = 1;
         this.openUploadForm(candidatureId);
        this.postulerAlert = true;
      alert("Votre candidature a été soumise avec succès !");
  }

  finaliser(candidatureId: number) {
    this.candidatureService.finaliser(candidatureId).subscribe({
      next: () => {
        alert("✅ Candidature finalisée !");
        this.selectedCandidatureId = null;
        this.pjCV = null;
        this.pjCL = null;
      },
      error: err => {
        console.error(err);
        const message = err.error?.message || JSON.stringify(err.error) || "Erreur inconnue";
        alert("Erreur : " + message);
    }
    });
  }

  
  togglePosteLater(posteId: number) {
    if (this.isMarked(posteId)) {
      this.removePosteLater(posteId);
    } else {
      this.addPosteLater(posteId);
    }
  }
  
  isMarked(posteId: number): boolean {
    return this.postesLater.includes(posteId);
  }

  addPosteLater(posteId: number) {
    this.posteService.addPosteLater(posteId).subscribe(
      (response) => {
        console.log(response);
        this.postesLater.push(posteId);
        alert("Poste ajouté à votre liste de suivi !");
      },
      (error) => {
        console.error('Erreur lors de l\'ajout du poste à la liste de suivi :', error);
        alert("❌ Une erreur est survenue lors de l'ajout.");
      }
    );
  }

  removePosteLater(posteId: number) {
    this.posteService.removePosteLater(posteId).subscribe(
      (response) => {
        console.log(response);
        this.postesLater = this.postesLater.filter(id => id !== posteId);
        alert("Poste retiré de votre liste de suivi !");
      },
      (error) => {
        console.error('Erreur lors du retrait du poste de la liste de suivi :', error);
        alert("❌ Une erreur est survenue lors du retrait.");
      }
    );
  }
  

  
  get totalPages(): number[] {
    const pages = Math.ceil(this.filteredAppels.length / this.pageSize);
    return Array.from({ length: pages }, (_, i) => i + 1);
  }
  
  
  get paginatedAppels() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredAppels.slice(start, start + this.pageSize);
  }
  
  changePage(page: number) {
    this.currentPage = page;
  }
  
  
  toggle(index: number): void {
    this.filteredAppels[index].isOpen = !this.filteredAppels[index].isOpen;
  }

  close(){
    this.selectedCandidatureId = null;
  }
  
}
