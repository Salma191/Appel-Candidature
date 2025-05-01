import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { TableComponent } from '../../layout/table/table.component';
import { CommonModule } from '@angular/common';
import { CandidatService } from '../../services/candidat.service';


interface Candidat {
  id: number;
  nom: string;
  prenom: string;
  email: string;
  telephone?: string;
  affectationActuelle?: string;
  posteOccupe?: string;
  adresse?: string;
  ville?: string;
  pays?: string;
  codePostal?: string;
  photoUrl?: string;
  note?: number;
  nbrExperience?: number;
  diplomes?: { nom: string; ecole: string; annee: string; niveau: string }[];
  experiences?: { poste: string; entreprise: string; duree: string; description: string }[];
  dateRetraite?: string;
  cvUrl?: string;
}


@Component({
  selector: 'app-profil',
  standalone: true,
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule
  ],
  templateUrl: './profil.component.html',
  styleUrls: ['./profil.component.scss']
})
export class ProfilComponent {

  id: number = 0;
  completion = 60; // pourcentage calculé dynamiquement
  user?: Candidat;
  actionsOpen: boolean = false;

  diplomeList: any[] = [];
  experienceList: any[] = [];
  defaultAvatar = 'https://freeaiavatargenerator.pro/wp-content/uploads/2023/03/ai-generated-gce3534016_1920.jpg';
  

  constructor(
    private candidatService: CandidatService,
  ) { }

  ngOnInit() {
    console.log("ngOnInit loaded");
    this.id = parseInt(localStorage.getItem('id') || '0');

    this.getCandidatById(this.id);
  }

  private getCandidatById(id: number) {
    this.candidatService.getUserById(id).subscribe({
      next: (resp: any) => {
        // On désosse la réponse reçue
        const util = resp.utilisateur ?? {};
        const rawExps = resp.experiences ?? [];
        const rawDip = resp.diplomes ?? [];

        this.user = {
          id:            resp.id,
          nom:           util.nom,
          prenom:        util.prenom,
          email:         util.email,
          telephone:     util.telephone,
          affectationActuelle:     resp.affectationActuelle,
          posteOccupe:   resp.posteOccupe,
          adresse:       resp.adresse,
          ville:         resp.ville,
          pays:          resp.pays,
          codePostal:    resp.codePostal,
          photoUrl:      resp.photoUrl,
          note:          resp.note,
          nbrExperience: resp.nbrExperience,
          dateRetraite:  resp.dateRetraite,
          diplomes:      rawDip.map((e: any) => ({
            nom:       e.intitulé || '',
            ecole:     e.etablissement || '',
            annee:      e.annéeObtention || '',
            niveau:    e.niveau || ''
          })),
          experiences: rawExps.map((e: any) => ({
            poste:       e.poste || '',
            entreprise:     e.entreprise || '',
            duree:       this.getDuration(e.dateDébut, e.dateFin),
            description: e.description || ''
          })),
  
          cvUrl:         resp.cvUrl
        };
        console.log('Candidat chargé:', this.user);
      },
      error: err => console.error('Erreur chargement candidat', err)
    });
  }



  // toggleDropdown(index: number) {
  //   this.items[index].isOpen = !this.items[index].isOpen;
  // }

  openProfileEditForm() {
    // Ouvrir le formulaire de modification du profil (modal, page de modification, etc.)
    console.log('Ouvrir le formulaire de modification du profil');
  }

  openAddDiplomeForm() {
    // Ouvrir le formulaire pour ajouter un diplôme
    console.log('Ouvrir le formulaire pour ajouter un diplôme');
  }

  openAddExperienceForm() {
    // Ouvrir le formulaire pour ajouter une expérience
    console.log('Ouvrir le formulaire pour ajouter une expérience');
  }

  private getDuration(startISO: string, endISO: string | null): string {
    const start = new Date(startISO);
    // Si dateFin n'est pas fournie, on prend aujourd'hui
    const end = endISO ? new Date(endISO) : new Date();
  
    let years  = end.getFullYear()  - start.getFullYear();
    let months = end.getMonth() - start.getMonth();
  
    // Ajustement si le mois de fin est avant le mois de début
    if (months < 0) {
      years--;
      months += 12;
    }
  
    const parts: string[] = [];
    if (years > 0) {
      parts.push(`${years} an${years > 1 ? 's' : ''}`);
    }
    if (months > 0) {
      parts.push(`${months} mois`);
    }
    // Si les deux sont à zéro, on peut retourner "0 mois" ou "Moins d'un mois"
    return parts.length > 0 ? parts.join(' ') : '0 mois';
  }
  


}
