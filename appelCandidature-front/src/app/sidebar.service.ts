import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private toggleSidebarSubject = new Subject<void>();
  toggleSidebar$ = this.toggleSidebarSubject.asObservable();

  isMobile(): boolean {
    return window.innerWidth <= 768; // Adjust the breakpoint as needed
  }

  toggleSidebar() {
    this.toggleSidebarSubject.next();
  }
}
