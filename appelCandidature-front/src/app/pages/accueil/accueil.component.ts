import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { PvService } from '../../services/pv.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-accueil',
  standalone: true,
  templateUrl: './accueil.component.html',
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  styleUrls: ['./accueil.component.scss']
})
export class AccueilComponent {
  typePoste: any = ""; // Type de poste sélectionné
  criteria: any = ""; // Critères de sélection
  types: any[] = []; // Tableau pour stocker les types de postes
  menuState = {
    candidatures: false,
    organisation: false,
    user: false
  };

  constructor(private pvService: PvService) {}

  ngOnInit() {
    // Appel de la fonction pour charger les types de postes au démarrage du composant
    this.getTypePostes();
  }

  getTypePostes() {
    this.pvService.getAllTypes().subscribe(
      (data: any[]) => {
        this.types = data;
        console.log("Types chargés :", this.types);
      },
      (error) => {
        console.error("Erreur lors du chargement des types :", error);
      }
    );
  }

  search() {
    // Logique de recherche basée sur le type de poste et les critères
    console.log("Recherche effectuée avec le type :", this.typePoste, "et les critères :", this.criteria);
  }

  // Fonction pour afficher ou masquer le menu
  toggleMenu(menu: 'candidatures' | 'organisation' | 'user') {
    // Fermer tous les autres menus et ouvrir celui sélectionné
    this.menuState = {
      candidatures: false,
      organisation: false,
      user: false
    };
    // Ouvrir le menu sélectionné
    this.menuState[menu] = true;
  }

}