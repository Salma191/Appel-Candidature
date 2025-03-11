import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-accueil',
  templateUrl: './accueil.component.html',
  imports: [
    CommonModule,
    RouterModule
  ],
  styleUrls: ['./accueil.component.scss']
})
export class AccueilComponent {
  menuState = {
    candidatures: false,
    organisation: false,
    user: false
  };

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