import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { ModalComponent } from '../../layout/modal/modal.component';
import { PieceJointeUploadComponent } from '../../layout/piece-jointe-upload/piece-jointe-upload.component';

@Component({
  selector: 'app-suivi-candidature',
  standalone: true,
  imports: [
    SidebarComponent,
    ModalComponent,
    PieceJointeUploadComponent
  ],
  templateUrl: './suivi-candidature.component.html',
  styleUrl: './suivi-candidature.component.scss'
})
export class SuiviCandidatureComponent {
  showErrorModal = false;

  isUploadOpen = false;

  openUpload() {
    this.isUploadOpen = true;
  }

  closeUpload() {
    this.isUploadOpen = false;
  }

  handleConfirm() {
    console.log('PJ ajoutée avec succès');
    this.closeUpload();
  }


}
