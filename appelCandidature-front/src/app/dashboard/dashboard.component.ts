import { Component } from '@angular/core';
import { SidebarComponent } from '../layout/sidebar/sidebar.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    SidebarComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  // isSidebarVisible = false;

  // toggleSidebar() {
  //   this.isSidebarVisible = !this.isSidebarVisible;
  // }
}
