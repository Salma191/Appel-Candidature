import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verification-success',
  imports: [],
  templateUrl: './verification-success.component.html',
  styleUrl: './verification-success.component.scss'
})
export class VerificationSuccessComponent {

  constructor(private router: Router) {}

  goToLogin() {
    this.router.navigate(['/login']);
  }

}
