import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SidebarService } from '../../services/sidebar.service';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';



@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [ 
    CommonModule,
    RouterModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SidebarComponent{
  isDecisionsMenuOpen = false;
isAppsMenuOpen = false;
  isSidebarOpen: boolean = false;
  isProjectsMenuOpen: boolean = false;
  isTeamMenuOpen: boolean = false;
  prenom: string | null = '';
  role: string | null = '';

  constructor(private sidebarService: SidebarService, private authService: AuthService, private router:Router) { }


  ngOnInit(): void {
    this.sidebarService.isSidebarOpen$.subscribe((isOpen) => {
      this.isSidebarOpen = isOpen;
    });

    this.prenom = this.authService.getPrenom();
    this.role = this.authService.getRole();
  }


  toggleMenu(menu: string) {
    if (menu === 'projects') {
      this.isProjectsMenuOpen = !this.isProjectsMenuOpen;
    } else if (menu === 'team') {
      this.isTeamMenuOpen = !this.isTeamMenuOpen;
    }
  }

  toggleSidebar(): void {
    this.sidebarService.toggleSidebar(); // Change l'état du menu
  }

  logout(): void {
    this.router.navigate(['/login']);
    this.authService.logout();
  }


}